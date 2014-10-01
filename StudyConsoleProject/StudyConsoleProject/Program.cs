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
    class Program
    {
        static Stopwatch mWatch = null;
        static bool isThread = false;
        static int num = 0;
        static int mTotalListCount = 0;

        static Dictionary<string, IEnumerable<string>> mDic = new Dictionary<string, IEnumerable<string>>();


        static void Main(string[] args)
        {

            Console.WriteLine("");

            // isThread = true;

            mWatch = new Stopwatch();
            mWatch.Start();

            
            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "돌고래", "코알라", "비버", "다람쥐", "기린", "벌새" };
            mTotalListCount = searchWordList.Length;
            Complete com = new Complete(searchWordList.Length, DownloadImageDic);
            for (int i = 0; i < searchWordList.Length; i++)
            {
                SearchImageSavedAsync(searchWordList[i],com);
            }

            /*mWatch.Stop();
            string m = String.Format(("검색이 완료 되었습니다. 시간은 " + "{0}" + "초 입니다."), mWatch.Elapsed);
            Console.WriteLine(m);*/
            Console.ReadLine();
        }

        public static void DownloadImageDic() 
        {
            //completed!
            string path = Environment.CurrentDirectory + "/images";
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Exists == false)
            {
                di.Create();
            }

            
            // 동시에 주어진 값(4) 갯수의 스레드로 다운로드 작업 할 수 있도록..
            int value = 2;

            // List<IEnumerable<string>> total = new List<IEnumerable<string>>();

            int share = mDic.Count() / value;
            int remain = mDic.Count() % value;

            List<string[]> arrList = new List<string[]>();

            int rt = 0;

            rt = value +remain; 


            //주어진 값 갯수만큼 배열 생성한다.
            //한 배열 당 주어진 값만큼의 배열이 들어가므로
            //배열의 크기는 dic에 있는(단어의 배열 갯수*몫) * 나눌 값 
            for (int i = 0; i < rt; i++)
            {
                string[] array = null;
                array = new string[(mDic.Count * share) * value];
                arrList.Add(array);
            }

            int n = 0;
            int r = 0;

            string[] urlList = mDic.Keys.SelectMany(key => mDic[key]).ToArray();

            List<string>[] arr = new List<string>[value];
            for (var i = 0; i < value; ++i)
            {
                arr[i] = new List<string>();
            }

            for (var i = 0; i < urlList.Length; ++i)
            {
                arr[i%value].Add(urlList[i]);
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

            int m = 0;
            int e = 0;
            //분리된 리스트를 가지고 thread 큐에 넣는다.
            foreach (var list in arr)
            {
                var refList = list;

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
                                string downloadFilePath = String.Format("{0}" + "/" + "{1}" + "{2}" + ".jpg", path, "image_", i);
                                
                                client.DownloadFile(refList[i], downloadFilePath);

                                Console.WriteLine(m);

                                m++;
                            }
                        }
                        catch (Exception)
                        {
                            e++;
                            Console.WriteLine("DownloadFile Exception/ count:" + e);
                        }
                    }
                });

                Console.WriteLine("ThreadPool 큐 저장 성공 여부 : " + isSuccess);

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

        //1.일반 적인 APM 패턴
        private static void SearchImageSavedAsync(Object word,Complete com)
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

            ImageContext info = new ImageContext();
            info.webRequest = webRequest;
            info.word = word as string;
            info.complete = com;

            webRequest.BeginGetResponse(ResponseCallBack, info);
            

            //webRequest.BeginGetResponse(ResponseCallBack, info);

            //System.IO.Stream stream = response.GetResponseStream();

        }

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

            //mDic내의 아이템들을 다운로드 한다.
            complete.Done();

            //WebClient는 WebResponse의 발전된 형태이다.(이벤트 베이스 비동기 패턴을 사용 할 수 있다.)

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
        ///  file 다운로드 완료 시 로그 찍는다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) 
        {
            var info = e.UserState as string;
            //파일 다운로드.
            Console.WriteLine("");
            string m = String.Format(("{0}"+" download completed."),info);
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
