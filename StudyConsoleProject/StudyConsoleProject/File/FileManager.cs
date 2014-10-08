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
   public static class FileManager
    {
       public static IEnumerable<string> GetMyDocumentList()
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            IEnumerable<string> myDocumentDirs = Directory.EnumerateDirectories(path);
            return myDocumentDirs;
        }

       public static string GetHtmlView() 
        {
            XmlDocument doc = new XmlDocument();
            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTMLPage1.htm");
            string path = "C:\\Users\\mgz730.EBAYKOREA\\mgz_sample\\sample\\StudyConsoleProject\\StudyConsoleProject\\View\\HTMLPage1.htm";
            doc.Load(path);

            return doc.ToString();
        }

       public static IEnumerable<string> GetDirectoryList(string path)
        {
            IEnumerable<string> list = null;
            String[] temp = null;

            //임시 : 특수 폴더 일 경우에는 SpecialFolder 경로를 사용합니다.
            if (path.Contains("My"))
            {
                if (path.Contains("My Music"))
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                else if (path.Contains("My Vidios"))
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                else if (path.Contains("My Pictures"))
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }

            try
            {
                list = Directory.EnumerateDirectories(path);
                Console.WriteLine("previous copy only Dir list count is {0}", list.Count());

                var fileList = Directory.EnumerateFiles(path);
                Console.WriteLine("only file list count is {0}", fileList.Count());

                temp = list.ToArray();

                if (fileList.Count() > 0)
                {
                    if (list != null)
                        temp = fileList.ToArray();

                    Array.Resize(ref temp, fileList.Count());
                    Console.WriteLine("resize Dir list count is {0}", temp.Count());
                    Array.Copy(fileList.ToArray(), temp, fileList.Count() - 1);
                    Console.WriteLine("after copy Dir list count is {0}", temp.Count());
                }
                
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }

            if (temp == null)
                return null;


            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("return list item is null. count is ", i);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("return item : ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(temp[i]);
                    Console.ResetColor();
                }
            }
            return temp;
        }
    }
}
