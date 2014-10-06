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

            IEnumerable<string> list = manager.GetList();

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";

            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            foreach (var item in list)
            {
                streamWriter.WriteLine(item);
                streamWriter.WriteLine(" ");
            }
            
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;   
        }


        public string GetHtmlView()
        {
            FileManager manager = new FileManager();
            return manager.GetHtmlView();
        }
    }
}
