using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.ServiceModel.Web;
using System.IO;
using StudyConsoleProject.File;
using System.Net.Mime;
using System.Net;

namespace StudyConsoleProject.Server
{
    public class KoKo : IKoKo
    {
        public string mDirPath;

        public string Test()
        {

            Console.WriteLine(WebOperationContext.Current.IncomingRequest); // 웹요청이다!
            Console.WriteLine(WebOperationContext.Current.OutgoingResponse); // 응답 - Client 한테 보내어진다.
            return "Let's Ko!";
        }


        public string Test2(string text)
        {
            return text;
        }


        public Stream Test3(string text)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            streamWriter.WriteLine(text);
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }

        public Stream GetMyDocumentList()
        {

            IEnumerable<string> list = FileManager.GetMyDocumentList();

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            Stream memStream = GetPageLinkStream(list, false,"");

            return memStream;
        }


        public Stream Move(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("not found path! path is C:/ ");
                path = "C:/";
            }

            bool isFile = false;

            IEnumerable<string> list = null;

            try
            {
                if ((System.IO.File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    list = FileManager.GetDirectoryList(path);
                }
                else
                {
                    string fileName = Path.GetFileName(path);
                    //브라우저가 보여줄 파일 명/타입을 지정한다.
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
                    WebOperationContext.Current.OutgoingResponse.Headers.Set("content-disposition", "attachment;filename=" + fileName);
                    list = new string[] { path };
                    isFile = true;
                }
            }
            catch (Exception ex)
            {
                //TODO: FileNotFoundedException 일 경우.. 브라우저에 에러 팝업 띄어줘야한다.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            Stream memStream = GetPageLinkStream(list, isFile, path);

            return memStream;
        }

        private Stream GetPageLinkStream(IEnumerable<string> list, bool isFile,string parentPath)
        {
            if (list == null)
                return null;

            if (isFile)
            {
                try
                {
                    FileStream fileStream = null;
                    var item = list.FirstOrDefault();
                    
                    fileStream = new FileStream(item, FileMode.Open,FileAccess.Read);
                    return fileStream;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }


            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            streamWriter.WriteLine("<html>");
            streamWriter.WriteLine("<header>");
            streamWriter.WriteLine("<title>");
            streamWriter.WriteLine("Web local file explorer");
            streamWriter.WriteLine("</title>");
            streamWriter.WriteLine("</header>");
            streamWriter.WriteLine("<body>");
            
            //
            string uriTemplate = String.Format("GetMyDocumentList?move=" + "{0}", Path.GetDirectoryName(Path.GetDirectoryName(list.FirstOrDefault())));
            string url = HttpUtility.UrlEncode(uriTemplate, System.Text.Encoding.GetEncoding("euc-kr"));

            streamWriter.WriteLine("<h1>");
            streamWriter.WriteLine(Path.GetDirectoryName(list.FirstOrDefault())+" Index");
            streamWriter.WriteLine("</h1>");
            if (!parentPath.Equals("C:/"))
            {
                string parentUrlButton = "<a href=" + url + ">" + "[Go to parent directory..]" + "</a>";
                streamWriter.WriteLine(parentUrlButton);
            }
            string iconPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Resources\folder.PNG";
            streamWriter.WriteLine("<table>");
            int i = 0;
            foreach (var item in list)
            {
               
                if (item!=null)
                {
                    streamWriter.WriteLine("<tr>");
                    long size = 0;
                    DateTime lastTime;
                    bool isDir = false;
                    if ((System.IO.File.GetAttributes(item) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        isDir = true;
                        DirectoryInfo info = new DirectoryInfo(item);
                        size = 0;
                        lastTime = info.LastWriteTime;
                    }
                    else
                    {
                        FileInfo info = new FileInfo(item);
                        size = 0;
                        lastTime = info.LastWriteTime;
                    }
                    uriTemplate = String.Format("GetMyDocumentList?move=" + "{0}", item);
                    url = HttpUtility.UrlEncode(uriTemplate, System.Text.Encoding.GetEncoding("euc-kr"));
                    string uriPath = "<a href=" + url + ">" + Path.GetFileName(item) + "</a>";
                    if (i==0)
                    {
                        streamWriter.WriteLine("<tr>");
                        streamWriter.WriteLine("<td>");
                        streamWriter.WriteLine("<b>");
                        streamWriter.WriteLine("Name");
                        streamWriter.WriteLine("</td>");
                        streamWriter.WriteLine("<td>");
                        streamWriter.WriteLine("<b>");
                        streamWriter.WriteLine("Size");
                        streamWriter.WriteLine("</td>");
                        streamWriter.WriteLine("<td>");
                        streamWriter.WriteLine("<b>");
                        streamWriter.WriteLine("DateModified");
                        streamWriter.WriteLine("</td>");
                        streamWriter.WriteLine("</tr>");
                        streamWriter.WriteLine("<hr/>");
                    }
                    streamWriter.WriteLine("<td>");
                    streamWriter.Write("<img src=" + iconPath + "</img>");
                    streamWriter.Write(uriPath);
                    streamWriter.WriteLine("</td>");
                    streamWriter.WriteLine("<td>");
                    if (isDir)
                    {
                        streamWriter.Write("");
                    }
                    else
                    {
                        streamWriter.Write(size);
                    }
                    streamWriter.WriteLine("</td>");
                    streamWriter.WriteLine("<td>");
                    streamWriter.Write(lastTime);
                    streamWriter.WriteLine("</td>");
                    streamWriter.WriteLine(" ");
                    streamWriter.WriteLine("<tr>");
                    i++;
                }
                
            }

            streamWriter.WriteLine("<table>");
            streamWriter.WriteLine("</body>");
            streamWriter.WriteLine("</html>");
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }
    }
}
