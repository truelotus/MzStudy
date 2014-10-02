using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;


namespace StudyConsoleProject
{

    public class ImageItem
    {
        public string name { get; set; }
        public int index { get; set; }
        public string url { get; set; }
    }

    public class ImageContext
    {
        public WebRequest webRequest { get; set; }
        public Complete complete { get; set; }
        public string word { get; set; }
        public string downloadedfFileName { get; set; }
    }

    public class Complete
    {
        Action completedAction;
        int count;
        int totalCount;
        public Complete(int total, Action action)
        {
            totalCount = total;
            completedAction = action;
        }

        public void Done()
        {
            count++;
            if (totalCount == count)
            {
                completedAction();
            }
        }
    }

    class Program
    {
        static Stopwatch mWatch = null;
        static bool isThread = false;
        static int num = 0;
        static int mTotalListCount = 0;

        static Dictionary<string, IEnumerable<string>> mDic = new Dictionary<string, IEnumerable<string>>();




        static void Main(string[] args)
        {

            Console.Title = "Study";

            // isThread = true;

            mWatch = new Stopwatch();
            mWatch.Start();


            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "돌고래", "코알라", "비버", "다람쥐", "기린", "벌새" };
            mTotalListCount = searchWordList.Length;
            Complete com = new Complete(searchWordList.Length, DownloadImageFromDic);
            for (int i = 0; i < searchWordList.Length; i++)
            {
                SearchImageSavedAsync(searchWordList[i], com);
            }

            /*mWatch.Stop();
            string m = String.Format(("검색이 완료 되었습니다. 시간은 " + "{0}" + "초 입니다."), mWatch.Elapsed);
            Console.WriteLine(m);*/
            Console.ReadLine();
        }

        /// <summary>
        /// Dictionary내 이미지 배열을 로컬에 다운로드 한다.
        /// </summary>
        public static void DownloadImageFromDic()
        {
            string path = Environment.CurrentDirectory + "/images";
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Exists == false)
            {
                di.Create();
            }

            //원하는 스레드 갯수 설정 할 수 있다.
            int threadCount = 4;

            int share = mDic.Count() / threadCount;
            int remain = mDic.Count() % threadCount;

            //string[] urlList = mDic.Keys.SelectMany(key => mDic[key]).ToArray();

            List<string[]> tempArrayList = new List<string[]>();

            List<ImageItem>[] threadArray = new List<ImageItem>[threadCount];

            for (var i = 0; i < threadCount; ++i)
            {
                threadArray[i] = new List<ImageItem>();
            }

            for (int i = 0; i < threadCount; i++)
            {
                string[] arr = new string[(mDic.Count * share) * threadCount];
                tempArrayList.Add(arr);
            }

            List<ImageItem> list = new List<ImageItem>();

            foreach (var key in mDic.Keys)
            {
                var i = 1;
                foreach (var url in mDic[key])
                {
                    list.Add(new ImageItem() { name = key, url = url, index = i - 1 });
                    ++i;
                }
            }


            for (var i = 0; i < list.Count; ++i)
            {
                threadArray[i % threadCount].Add(list[i]);
            }


            ////실제 url주소를 각각 넣어준다.
            //foreach (var list in mDic.Values)
            //{
            //   //dic list count
            //   string[] arr = list.ToArray<string>();

            //   Array.Copy(arr, 0, arrList[], r, arr.Length);
            //   r = r + arr.Length;
            //   n++;
            //    // arrList[n / value] = arr; 
            //    // arrList[n / value][n % value] = item;
            //}

            //counting.
            int m = 0;
            int e = 0;

            //분리된 리스트를 가지고 thread 큐에 넣는다.
            //dic의 키값과 해당 키값의 각 다운로드 받은 이미지 갯수...
            foreach (var set in threadArray)
            {
                var refList = set;

                bool isSuccess = ThreadPool.QueueUserWorkItem(q =>
                {
                    WebClient client = new WebClient();

                    for (int i = 0; i < refList.Count; i++)
                    {
                        try
                        {
                            if (refList[i] == null)
                            {
                                Console.WriteLine("refList[i] is null..");
                            }
                            else
                            {
                                string name = refList[i].name;
                                int index = refList[i].index;
                                string url = refList[i].url;
                                string downloadFilePath = String.Format("{0}" + "/" + "{1}" + "{2}" + ".jpg", path, name + "_", index);

                                client.DownloadFile(url, downloadFilePath);

                                Console.Write("Save completed --- count:");
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write(m);
                                Console.WriteLine(" ");
                                Console.ResetColor();

                                m++;
                            }
                        }
                        catch (Exception)
                        {
                            e++;
                            Console.Write("DownloadFile Exception...count:");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(e);
                            Console.WriteLine(" ");
                            Console.ResetColor();
                        }
                    }
                });
                Console.Write("ThreadPool 큐 저장 성공 여부 : ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(isSuccess);
                Console.WriteLine(" ");
                Console.ResetColor();
            }




            /* foreach (var key in mDic.Keys)
             {
                 var list = mDic[key];
                 int n = 1;
                 foreach (var item in list)
                 {
                     try
                     {
                         //string downloadFilePath = path + "/" + key + n + ".jpg";
                         string downloadFilePath = String.Format("{0}" + "/" + "{1}" + "{2}" + ".jpg",path,key,n);
                         Console.WriteLine(item);
                         client.DownloadFile(item, downloadFilePath);
                         n++;
                         m++;
                     }
                     catch (Exception)
                     {

                     }
                 }
                
             }
              
             Console.WriteLine(" ");
             Console.WriteLine("Total download count :" +m);
             */

        }

        //1.일반 적인 APM 패턴
        private static void SearchImageSavedAsync(Object word, Complete com)
        {
            if (word == null)
            {
                return;
            }

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" + word);

            string strURL = String.Format("https://www.google.co.kr/search?q="
                + "{0}" + "&newwindow=1&es_sm=93&biw=987&bih=991&source=lnms&tbm=isch&sa=X&ei=keQoVKy7IIaJ8QWZm4KwAg&ved=0CAYQ_AUoAQ#newwindow=1&tbm=isch&q="
                + "{0}" + "&imgdii=_", word.ToString());

            WebRequest webRequest = WebRequest.Create(strURL);

            ImageContext info = new ImageContext() { webRequest = webRequest, word = word as string, complete = com };

            webRequest.BeginGetResponse(ResponseCallBack, info);
        }

        /// <summary>
        /// 비동기 콜백 메서드
        /// </summary>
        /// <param name="result"></param>
        private static void ResponseCallBack(IAsyncResult result)
        {
            var info = result.AsyncState as ImageContext;
            var request = info.webRequest;
            var word = info.word;
            var complete = info.complete;

            var response = request.EndGetResponse(result);

            string path = Environment.CurrentDirectory + "/images";
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Exists == false)
            {
                di.Create();
            }

            System.IO.Stream stream = response.GetResponseStream();
            IEnumerable<String> list = GetImageLinks(GetStreamToString(stream)).Distinct();

            mDic.Add(word.ToString(), list);

            //mDic내의 아이템들을 다운로드를 시작 한다.
            complete.Done();
            

            //WebClient는 WebResponse의 발전된 형태이다.(이벤트 베이스 비동기 패턴을 사용 할 수 있다.
            /*
             WebClient client = new WebClient();
             client.DownloadFileCompleted += DownloadFileCompleted; 
             int n = 0;
              
              
              foreach (var item in list)
             {
                 string fileName = String.Format("{0}" + "_" + "{1}" + ".jpg", word, n);
                 if (!String.IsNullOrEmpty(item))
                 {
                     try
                     {
                         n++;
                         string downloadFilePath = path + "/" + fileName;
                         info.downloadedfFileName = fileName;
                         Console.WriteLine(item);
                         //client.DownloadFile(item, downloadFilePath);
                         client.DownloadFileAsync(new Uri(item), downloadFilePath, fileName);
                        
                     }
                     catch (Exception)
                     {

                     }
                 }
                 else
                 {
                     Console.WriteLine("");

                     Console.WriteLine("Not found item...!");
                 }
             }/*

            
             /*Console.WriteLine("");
             string m = String.Format(("Word:" + "{0}" + "/ Download Count:" + "{1}"), word, n);
             Console.WriteLine(m);

           
             num++;

             //
             if (num == mTotalListCount)
             {
                 client.DownloadFileCompleted -= DownloadFileCompleted;
                 mWatch.Stop();
                 Console.WriteLine("");
                 m = String.Format(("completed. Time: " + "{0}"), mWatch.Elapsed);
                 Console.WriteLine(m);
             }*/
        }

        /// <summary>
        ///  file 다운로드 완료 시 로그 출력 동작을 한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var info = e.UserState as string;
            //파일 다운로드.
            Console.WriteLine("");
            string m = String.Format(("{0}" + " download completed."), info);
            Console.WriteLine(m);
        }

        private static string GetStreamToString(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            stream.Close();
            reader.Close();

            return str;
        }

        private void DividedArray(int value)
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
        }

        private static void SearchImageSaved(Object word)
        {
            if (word == null)
            {
                return;
            }

            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" + word);

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

                string fileName = String.Format("{0}" + "_" + "{1}" + ".jpg", word, n);
                if (!YounExtention.IsNullOrEmpty(item))
                {
                    try
                    {
                        n++;
                        client.DownloadFile(item, path + "/" + fileName);
                    }
                    catch (Exception)
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
