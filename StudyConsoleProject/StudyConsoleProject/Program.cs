using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;


namespace StudyConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "토끼", "뱀", "원숭이", "기린", "얼룩말", "사자"};
            for (int i = 0; i < searchWordList.Length; i++)
            {
                SearchImageSaved(searchWordList[i]);
            }
            
        }

        private static void SearchImageSaved(string word)
        {
            //String strURL = "https://www.google.co.kr/search?q=tigertiger&imgdii=_";
            string strURL = String.Format("https://www.google.co.kr/search?q=" + "{0}" + "&newwindow=1&es_sm=93&biw=987&bih=991&source=lnms&tbm=isch&sa=X&ei=keQoVKy7IIaJ8QWZm4KwAg&ved=0CAYQ_AUoAQ#newwindow=1&tbm=isch&q=" + "{0}" + "&imgdii=_", word);
            WebRequest webRequest = WebRequest.Create(strURL);
            WebResponse response = webRequest.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            stream.Close();
            reader.Close();

            IEnumerable<String> originList = GetImageLinks(str);

            IEnumerable<String> list = originList.Distinct();

            String path = Environment.CurrentDirectory + "/images";
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Exists == false)
            {
                di.Create();
            }

            WebClient client = new WebClient();
            int n = 0;
            foreach (var item in list)
            {

                string fileName = String.Format("{0}"+"_"+"{1}"+ ".jpg",word,n);
                if (!YounExtention.IsNullOrEmpty(item))
                {
                    try
                    {
                        n++;
                        client.DownloadFile(item, path + "/" + fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }
            }
        }
        static IEnumerable<string> GetImageLinks(string inputHTML)
        {
            const string pattern = @"<img\b[^\<\>]+?\bsrc\s*=\s*[""'](?<L>.+?)[""'][^\<\>]*?\>";
            return Regex.Matches(inputHTML, pattern, RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Select(match => match.Groups["L"].Value);
        }
    }
}
