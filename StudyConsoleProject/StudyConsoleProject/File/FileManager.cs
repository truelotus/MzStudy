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
        public IEnumerable<string> GetMyDocumentList()
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

        public IEnumerable<string> GetDirectoryList(string path)
        {
            IEnumerable<string> list = null;
            //임시: 특수 폴더 일 경우에는 SpecialFolder를 사용합니다.
            if (path.Contains("My"))
            {
                if (path.Contains("My Music"))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                }else if (path.Contains("My Vidios"))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                }
                else if (path.Contains("My Pictures"))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                }
            }

            try
            {
                list = Directory.EnumerateDirectories(path);
                
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
            return list;
        }
    }
}
