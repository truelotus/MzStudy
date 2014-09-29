using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;


namespace StudyConsoleProject
{
    class Program
    {
        static Stopwatch mWatch = null;
        static bool isThread = false;
        static int num = 0;
        static void Main(string[] args)
        {

            isThread = true;

            if (!isThread)
            {
                mWatch = new Stopwatch();
                mWatch.Start();
            }
            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "토끼", "여우", "원숭이", "기린", "얼룩말", "사자"};
            for (int i = 0; i < searchWordList.Length; i++)
            {
                if (isThread)
                {
                    ThreadPool.QueueUserWorkItem(SearchImageSaved, searchWordList[i]);
                }
                else
                {
                    SearchImageSaved(searchWordList[i]);
                }
            }

            if (!isThread)
            {
                mWatch.Stop();
                String m = String.Format(("검색이 완료 되었습니다. 시간은 " + "{0}" + "초 입니다."), mWatch.Elapsed);
                Console.WriteLine(m);
            }
            //Console.WriteLine(m);
            Console.ReadLine();

        }

        private static void SearchImageSaved(Object word)
        {
            if (isThread)
            {
                mWatch = new Stopwatch();
                mWatch.Start();
            }
            

            string strURL = String.Format("https://www.google.co.kr/search?q=" 
                + "{0}" + "&newwindow=1&es_sm=93&biw=987&bih=991&source=lnms&tbm=isch&sa=X&ei=keQoVKy7IIaJ8QWZm4KwAg&ved=0CAYQ_AUoAQ#newwindow=1&tbm=isch&q=" 
                + "{0}" + "&imgdii=_", word.ToString());

            WebRequest webRequest = WebRequest.Create(strURL);
            WebResponse response = webRequest.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            stream.Close();
            reader.Close();

            IEnumerable<String> list = GetImageLinks(str).Distinct();

            string path = Environment.CurrentDirectory + "/images";
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
                       //Console.WriteLine(ex.ToString());
                    }

                }
            }
            string m = String.Format(("검색단어:" + "{0}" + "/ 다운로드 받은 갯수:" + "{1}"), word, n);
            Console.WriteLine(m);

            num++;
            if (isThread)
            {
                if (num == 10)
                {
                    mWatch.Stop();
                    m = String.Format(("검색이 완료 되었습니다. 시간은 " + "{0}" + "초 입니다."), mWatch.Elapsed);
                    Console.WriteLine(m);
                }
            }
            
            
            //Thread.Sleep(1000);
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
