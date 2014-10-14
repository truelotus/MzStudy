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

namespace FileExplorerWebApplication
{
    public partial class Defalut : System.Web.UI.Page
    {

        //aspx 단에서 보여지는 부분
        public string mCurrentDirectoryPath = null;
        public string mParentDirectoryPath = null;
        public IEnumerable<string> mDirectories = null;
        public static string previousPath = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var url = Request.Url.AbsoluteUri;
            var moveDirUrl = Request.QueryString["move"];
            Move(moveDirUrl);
        }


        public void Move(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                if (previousPath.Equals("C:\\"))
                    path = Path.GetPathRoot("C:\\");
                else
                {
                    Console.WriteLine("Path not founded..Default path is MyDocuments.");
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
            }

            try
            {
                if ((System.IO.File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    //디렉토리 일 경우
                    Response.ContentType = "text/html";
                    //list = GetAllFiles(path);
                }
                else
                {
                    //파일일 경우 클라이언트로 출력한다.
                    string fileName = Path.GetFileName(path);
                    Stream stream = null;
                    byte[] buffer = new Byte[10000];

                    try
                    {
                        FileWebRequest fileRequest = (FileWebRequest)FileWebRequest.Create(path);
                        FileWebResponse fileResponse = (FileWebResponse)fileRequest.GetResponse();

                        if (fileRequest.ContentLength > 0)
                            fileResponse.ContentLength = fileRequest.ContentLength;

                        stream = fileResponse.GetResponseStream();

                        //클라이언트에 보내줄 형식과 이름
                        var response = HttpContext.Current.Response;
                        response.ContentType = "application/octet-stream";
                        response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                        int length;
                        do
                        {
                            if (response.IsClientConnected)
                            {
                                length = stream.Read(buffer, 0, 10000);
                                response.OutputStream.Write(buffer, 0, length);
                                response.Flush();
                            }
                            else
                            {
                                length = -1;
                            }
                        } while (length > 0);
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }

                previousPath = mCurrentDirectoryPath = path;
                mParentDirectoryPath = String.Format("?move=" + "{0}", HttpUtility.UrlEncode(Path.GetDirectoryName(path)));
            }
            catch (Exception ex)
            {
                //FileNotFoundedException
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
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
                return null;

            if ((System.IO.File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return new[] { Directory.EnumerateDirectories(path), Directory.EnumerateFiles(path) }.SelectMany(item => item);
            }
            return new[] { path };
        }

        public string GetSize(string item)
        {
            string size = "";
            if ((System.IO.File.GetAttributes(item) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DirectoryInfo info = new DirectoryInfo(item);
                size = "";
            }
            else
            {
                FileInfo info = new FileInfo(item);
                size = info.Length.ToString();
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

        public string GetIcon(string path)
        {
            if (IsFile(path))
                return "Resources/file.PNG";
            else
                return "Resources/folder.PNG";
        }

        public string GetShortName(string item)
        {
            return Path.GetFileName(item);
        }

        public string GetUrl(string item)
        {
            return String.Format("?move=" + "{0}", HttpUtility.UrlEncode(item));
        }

        public bool IsFile(string path)
        {
            if ((System.IO.File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                return false;
            else
                return true;
        }
    }
}