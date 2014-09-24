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
            List<String> listOrigin = new List<string>();
            listOrigin.Add("첫번째 아이템");
            Console.WriteLine("----------------------기존의 리스트 출력------------------------------------");
            Console.WriteLine(listOrigin[0]);

            listOrigin.Add("두번째 아이템");

            listOrigin.Add("첫번째 아이템");

            listOrigin.Add("두번째 아이템");
            ConsoleWriteAll(listOrigin);
            
            listOrigin.Remove("첫번째 아이템");
            ConsoleWriteAll(listOrigin);

            listOrigin.Insert(1, "이상한 아이템");
            ConsoleWriteAll(listOrigin);

            Console.WriteLine(listOrigin.IndexOf("이상한 아이템"));
            
            ConsoleWriteAllWithForeach(listOrigin);

           // ConsoleWriteAll(listOrigin);

            String[] Array = new String[10];
            Array[1] = "복사한 아이템";

           

            listOrigin.CopyTo(Array, 5);
            Console.WriteLine("----------------------▽Copy To array------------------------------------");
            ConsoleWriteAll(Array);
            
            

            YounList<String> list = new YounList<string>();
            list.Add("첫번째 아이템");
            Console.WriteLine("----------------------YounList 리스트 출력------------------------------------");
            
            Console.WriteLine(list[0]);

            list.Add("두번째 아이템");

            list.Add("첫번째 아이템");

            list.Add("두번째 아이템");
            ConsoleWriteAll(list);

            list.Remove("첫번째 아이템");
            ConsoleWriteAll(list);

            list.Insert(1, "이상한 아이템");
            ConsoleWriteAll(list);

            Console.WriteLine(list.IndexOf("이상한 아이템"));

            ConsoleWriteAllWithForeach(list);

            Array = new String[10];
            Array[1] = "이상한 아이템";

           
            if (Array.IsNullList())
            {
                Console.WriteLine("array is null");
            }
            else
            {
                Console.WriteLine("array is not null");
            }

            var m = list.YounWhere(item => item.Equals("이상한 아이템"));
            ConsoleWriteAllWithForeach(m);

            list.YounForeach(Console.WriteLine);

            var newList = new List<string>();
            list.YounForeach(newList.Add);
            
            list.CopyTo(Array, 5);
            Console.WriteLine("----------------------▽Copy To array------------------------------------");
            ConsoleWriteAll(Array);
            
            Console.ReadLine();
        }

        private static void Print(string item)
        {
            Console.WriteLine(item);
        }

        private static bool Test(string item, string item2)
        {
            return item.Equals("이상한 아이템");
        }

        private static void ConsoleWriteAll(IList<string> targetList)
        {
            for (var i = 0; i < targetList.Count; ++i)
            {
                Console.WriteLine(targetList[i]);
            }
        }

        private static void ConsoleWriteAllWithForeach(IEnumerable<string> targetList)
        {
            foreach (var item in targetList)
            {
                Console.WriteLine(item);
            }
        }
    }
}
