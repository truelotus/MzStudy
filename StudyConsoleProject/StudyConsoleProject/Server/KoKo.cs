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
        public static string previousPath = String.Empty;


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

        /// <summary>
        /// url 변경 시 Stream 작성하여 서버로 반환한다.
        /// </summary>
        /// <param name="path">디렉토리 경로</param>
        /// <returns></returns>
        public Stream Move(string path)
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
                    //브라우저에서 보여질 파일 명/타입을 지정한다.
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
                    WebOperationContext.Current.OutgoingResponse.Headers.Set("content-disposition", "attachment;filename=" 
                        + HttpUtility.UrlEncode(Path.GetFileName(path)));
                    return System.IO.File.OpenRead(path);
                }
            }
            catch (Exception ex)
            {
                //FileNotFoundedException
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            previousPath = path;
            return GetHtmlStream(list, path);
        }

        /// <summary>
        /// 경로와 경로에 존재하는 파일목록을 가지고 Html을 만들어 Stram에 작성한다.
        /// </summary>
        /// <param name="list">파일목록</param>
        /// <param name="parentDirPath">부모 경로</param>
        /// <returns>Html stream</returns>
        private Stream GetHtmlStream(IEnumerable<string> list, string parentDirPath)
        {
            if (list == null)
                return null;

            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            streamWriter.WriteLine("<html>");
            streamWriter.WriteLine("<header>");
            streamWriter.WriteLine("<title>");
            streamWriter.WriteLine("Web local file explorer");
            streamWriter.WriteLine("</title>");
            streamWriter.WriteLine("</header>");
            streamWriter.WriteLine("<body>");

            string currentDirPath = String.Format("GetMyDocumentList?move=" + "{0}", HttpUtility.UrlEncode(Path.GetDirectoryName(parentDirPath)));

            streamWriter.WriteLine("<h1>");
            streamWriter.WriteLine(parentDirPath);
            streamWriter.WriteLine("</h1>");
            if (!parentDirPath.Equals("C:/"))
            {
                streamWriter.WriteLine("<a href=" + currentDirPath + ">" + "[Go to parent directory..]" + "</a>");
            }

            //테이블을 그린다.
            streamWriter.WriteLine("<table>");
            int i = 0;
            foreach (var item in list)
            {

                if (item != null)
                {

                    long size = 0;
                    DateTime lastTime;

                    if (!IsFile(item))
                    {

                        DirectoryInfo info = new DirectoryInfo(item);
                        size = 0;
                        lastTime = info.LastWriteTime;
                    }
                    else
                    {
                        FileInfo info = new FileInfo(item);
                        size = info.Length;
                        lastTime = info.LastWriteTime;
                    }
                    currentDirPath = String.Format("GetMyDocumentList?move=" + "{0}", HttpUtility.UrlEncode(item));

                    string uriPath = "<a href=" + currentDirPath + ">" + Path.GetFileName(item) + "</a>";
                    if (i == 0)
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
                    if (!IsFile(item))
                    {
                        streamWriter.Write("<img src='/fordericon'/>");
                    }
                    else
                    {
                        streamWriter.Write("<img src='/fileicon'/>");
                    }
                    
                    streamWriter.Write(uriPath);
                    streamWriter.WriteLine("</td>");

                    streamWriter.WriteLine("<td>");
                    if (!IsFile(item))
                    {
                        streamWriter.Write(String.Empty);
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

        public bool IsFile(string path)
        {
            if ((System.IO.File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                return false;
            else
                return true;
        }

        public Stream GetFolderIcon()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
            return System.IO.File.OpenRead(@"..\..\Resources\folder.PNG");
        }

        public Stream GetFileIcon()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
            return System.IO.File.OpenRead(@"..\..\Resources\file.PNG");
        }

    }
}
