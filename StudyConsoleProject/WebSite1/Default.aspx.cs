using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Text;
using System.Web.Configuration;
using System.Net.Mime;
using System.Net;
using System.ServiceModel.Web;

public partial class Default : System.Web.UI.Page
{

	//aspx 단에서 보여지는 부분
	public string mCurrentDirectoryPath = null;
	public string mParentDirectoryPath = null;
	public IEnumerable<string> mDirectories = null;


	protected void Page_Load(object sender, EventArgs e)
	{
		var url = Request.Url.AbsoluteUri;
		var moveDirUrl = Request.QueryString["move"];

		if (!String.IsNullOrEmpty(moveDirUrl))
		{
			
			Move(moveDirUrl);
		}
		else
		{
			//기본 경로 설정.
			mCurrentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			mParentDirectoryPath = HttpUtility.UrlEncode(Path.GetDirectoryName(mCurrentDirectoryPath));
			Move(mCurrentDirectoryPath);
		}
	}

	public static IEnumerable<string> GetMyDocumentDirList()
	{
		String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		IEnumerable<string> myDocumentDirs = Directory.EnumerateDirectories(path);
		return myDocumentDirs;
	}

	public static IEnumerable<string> GetAllFiles(string path)
	{
		if (String.IsNullOrEmpty(path))
		{
			return null;
		}
		return new[] { Directory.EnumerateDirectories(path), Directory.EnumerateFiles(path) }.SelectMany(item => item);
	}

	public void Move(string path)
	{
		if (String.IsNullOrEmpty(path))
		{
			Console.WriteLine("not found path!");
			path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		IEnumerable<string> list = null;

		try
		{
			if ((System.IO.File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
			{
				Response.ContentType = "text/html";
				//WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
				list = GetAllFiles(path);
			}
			else
			{
				string fileName = Path.GetFileName(path);
				//브라우저가 보여줄 파일 명/타입을 지정한다.
				Response.ContentType = "application/octet-stream";
				Response.Headers.Set("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
			}
		}
		catch (Exception ex)
		{
			//TODO: FileNotFoundedException 일 경우.. 브라우저에 에러 팝업 띄어줘야한다.
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(ex.Message);
			Console.ResetColor();
		}

	}

	private Stream GetPageLinkStream(IEnumerable<string> list, string parentPath)
	{
		if (list == null)
			return null;

		var memStream = new MemoryStream();
		var streamWriter = new StreamWriter(memStream);


		string url = String.Format("?move=" + "{0}", HttpUtility.UrlEncode(Path.GetDirectoryName(parentPath)));
		mCurrentDirectoryPath = parentPath;
		if (!parentPath.Equals("C:/"))
		{
			string parentUrlButton = "<a href=" + url + ">" + "[Go to parent directory..]" + "</a>";

		}
		mParentDirectoryPath = url;

		return memStream;
	}

	public long GetSize(string item)
	{
		long size = 0;
		if ((System.IO.File.GetAttributes(item) & FileAttributes.Directory) == FileAttributes.Directory)
		{
			DirectoryInfo info = new DirectoryInfo(item);
			size = 0;

		}
		else
		{
			FileInfo info = new FileInfo(item);
			size = info.Length;
		}

		return size;
	}

	public DateTime GetModifiedDate(string item)
	{
		DateTime dateTime;
		if ((System.IO.File.GetAttributes(item) & FileAttributes.Directory) == FileAttributes.Directory)
		{
			DirectoryInfo info = new DirectoryInfo(item);
			dateTime = info.LastWriteTime;

		}
		else
		{
			FileInfo info = new FileInfo(item);
			dateTime = info.LastWriteTime;
		}

		return dateTime;
	}

	public string GetFolderIcon()
	{
		return "http://localhost:50157/WebSite1/Resources/folder.PNG";
	}

	public string GetShortName(string item) 
	{
		return Path.GetFileName(item);
	}

	public string GetUrl(string item) 
	{
		return String.Format(Request.Url.AbsoluteUri + "?move=" + "{0}", HttpUtility.UrlEncode(item));
	}
}