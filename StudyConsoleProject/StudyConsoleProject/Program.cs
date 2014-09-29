﻿using System;
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
            String strURL = "http://www.naver.com";
            WebRequest webRequest = WebRequest.Create(strURL);
            WebResponse response = webRequest.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String str = reader.ReadToEnd();
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

            foreach (var item in list)
            {
                String name = Path.GetTempFileName()+".jpg";
                if (!YounExtention.IsNullOrEmpty(item))
                {
                    try
                    {
                        client.DownloadFile(item, di + name);
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
