using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.ServiceModel.Web;
using System.IO;
using StudyConsoleProject.File;

namespace StudyConsoleProject.Server
{
    public class KoKo : IKoKo
    {
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
            FileManager manager = new FileManager();

            IEnumerable<string> list = manager.GetMyDocumentList();

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            Stream memStream = GetLinkWriteStream(list);

            return memStream;   
        }


        public Stream MovePath(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("경로가 없습니다.");
                return null;
            }

            FileManager manager = new FileManager();
           // string url = HttpUtility.UrlEncode(path, System.Text.Encoding.GetEncoding("euc-kr"));
            IEnumerable<string> list = manager.GetDirectoryList(path);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            Stream memStream = GetLinkWriteStream(list);
            
            return memStream;   
        }

        private Stream GetLinkWriteStream(IEnumerable<string> list) 
        {
            if (list==null)
            {
                return null;
            }
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            streamWriter.WriteLine("<html>");
            streamWriter.WriteLine("<body>");
            foreach (var item in list)
            {
                streamWriter.WriteLine("<p/>");
                string uriTemplate = String.Format("GetMyDocumentList?move=" + "{0}", item);
                //string trim = uriTemplate.Replace(" ", "");
                string url = HttpUtility.UrlEncode(uriTemplate, System.Text.Encoding.GetEncoding("euc-kr"));
                string str = "<a href=" + url + ">" + item + "</a>";
                streamWriter.Write(str);
                streamWriter.WriteLine(" ");
            }
            streamWriter.WriteLine("</body>");
            streamWriter.WriteLine("</html>");
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }
    }
}
