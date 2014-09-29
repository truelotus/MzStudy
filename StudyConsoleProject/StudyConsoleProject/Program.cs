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
            String strURL = "http://www.naver.com";
            WebRequest webRequest = WebRequest.Create(strURL);
            WebResponse response = webRequest.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String str = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            IEnumerable<String> list = GetImageLinks(str);
            foreach (var item in list)
            {
                Console.WriteLine(item);
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
