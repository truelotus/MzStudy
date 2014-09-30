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

            Console.WriteLine("");

           // isThread = true;

            mWatch = new Stopwatch();
            mWatch.Start();

            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "돌고래", "코알라", "비버", "다람쥐", "기린", "벌새"};

            for (int i = 0; i < searchWordList.Length; i++)
            {
               SearchImageSaved(searchWordList[i]);
            }

            mWatch.Stop();
            string m = String.Format(("검색이 완료 되었습니다. 시간은 " + "{0}" + "초 입니다."), mWatch.Elapsed);
            Console.WriteLine(m);
            Console.ReadLine();
        }

        private void DividedArrray(int value)
        {
            String m = String.Format(("Start >" + "{0}" + " 개의 스레드로 작업을 나누어 동작합니다.."), value);
            Console.WriteLine(m);
            Console.WriteLine("");

            isThread = true;

            mWatch = new Stopwatch();
            mWatch.Start();

            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "돌고래", "코알라", "비버", "다람쥐", "기린", "벌새" };

            int share = searchWordList.Length / value;
            int remain = searchWordList.Length % value;

            List<string[]> arrList = new List<string[]>();

            for (int i = 0; i < share + remain; i++)
            {
                arrList.Add(new string[value]);
            }

            for (int i = 0; i < searchWordList.Length; i++)
            {
                //array[몫][나머지]
                arrList[i / value][i % value] = searchWordList[i];

            }


            //thread 큐에 쌓는다.
            for (int i = 0; i < arrList.Count; i++)
            {
                String[] arr = arrList[i];
                ThreadPool.QueueUserWorkItem(q =>
                {
                    for (int j = 0; j < arr.Length; j++)
                    {
                        SearchImageSaved(arr[j]);
                    }
                });
            }

            if (!isThread)
            {
                mWatch.Stop();
                m = String.Format(("검색이 완료 되었습니다. 시간은 " + "{0}" + "초 입니다."), mWatch.Elapsed);
                Console.WriteLine(m);
            }

            Console.ReadLine();


            //
            /* if (value == 1)
            {
                ThreadPool.QueueUserWorkItem(q =>
                {
                    for (int i = 0; i < searchWordList.Length; i++)
                    {
                        SearchImageSaved(searchWordList[i]);
                    }
                });
            }
            else
            {
                //TODO: 입력한 쓰레드 갯수만큼의 쓰레드로 작업을 나눠서 처리 하도록.

                List<String[]> list = new List<String[]>();

                int s = 0; //시작요소

                int number = searchWordList.Length / value;

                if (number <= 1)
                {
                    Console.WriteLine("나눌 수 없습니다.");
                }
                else
                {
                    for (int i = 0; i < searchWordList.Length; i++)
                    {
                        int oddnumber = number % 2;
                        if (oddnumber == 1)
                        {
                            if (i >= number - 1)
                                ++number;
                        }

                        String[] arr = new String[number];
                        if (s >= searchWordList.Length)
                        {
                            break;
                        }

                        Array.Copy(searchWordList, s, arr, 0, number);
                        list.Add(arr);
                        s = s + number;
                    }

                    //thread 큐에 쌓는다.
                    for (int i = 0; i < list.Count; i++)
                    {
                        String[] arr = list[i];
                        ThreadPool.QueueUserWorkItem(q =>
                        {
                            for (int j = 0; j < arr.Length; j++)
                            {
                                SearchImageSaved(arr[j]);
                            }
                        });
                    }

                    if (!isThread)
                    {
                        mWatch.Stop();
                        m = String.Format(("검색이 완료 되었습니다. 시간은 " + "{0}" + "초 입니다."), mWatch.Elapsed);
                        Console.WriteLine(m);
                    }
                }
                Console.ReadLine();
            }*/




            /* ThreadPool.QueueUserWorkItem(q => 
             {
                 for (int i = 0; i < searchWordList.Length/2; i++)
                 {
                     SearchImageSaved(searchWordList[i]);
                 }
             });

             ThreadPool.QueueUserWorkItem(q =>
             {
                 for (int i = searchWordList.Length / 2; i < searchWordList.Length; i++)
                 {
                     SearchImageSaved(searchWordList[i]);
                 }
             });*/



            /*for (int i = 0; i < searchWordList.Length; i++)
            {
                if (isThread)
                {
                    ThreadPool.QueueUserWorkItem(SearchImageSaved, searchWordList[i]);
                }
                else
                {
                    SearchImageSaved(searchWordList[i]);
                }
            }*/

        }


        private static void SearchImageSaved(Object word)
        {
            if (word == null)
            {
                return;
            }

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId+":"+word);
            
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
