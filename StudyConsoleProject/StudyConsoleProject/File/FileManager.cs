using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Web.UI;

namespace StudyConsoleProject.File
{
    class FileManager
    {
        public IEnumerable<string> mMyDocumentPahtList;
        public FileManager() 
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            mMyDocumentPahtList = Directory.EnumerateDirectories(path);

            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //}
        }
        public IEnumerable<string> GetList()
        {
            return mMyDocumentPahtList;
        }
        public string GetHtmlView() 
        {
            XmlDocument doc = new XmlDocument();
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTMLPage1.htm");
            string path = "C:\\Users\\mgz730.EBAYKOREA\\mgz_sample\\sample\\StudyConsoleProject\\StudyConsoleProject\\View\\HTMLPage1.htm";
            doc.Load(path);

            return doc.ToString();
        }
    }
}
