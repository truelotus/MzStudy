using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudyConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            YounList<String> list = new YounList<string>();
            list.Add("첫번째 아이템");
            list.Add("두번째 아이템");
            Console.WriteLine(list[0]);
        }
    }
}
