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
            Console.WriteLine(listOrigin[0]);

            listOrigin.Add("두번째 아이템");
            ConsoleWriteAll(listOrigin);
            
            listOrigin.Remove("첫번째 아이템");
            ConsoleWriteAll(listOrigin);

            ConsoleWriteAllWithForeach(listOrigin);

            Console.WriteLine("-------------------------------------------------------");

            YounList<String> list = new YounList<string>();
            list.Add("첫번째 아이템");
            Console.WriteLine(list[0]);

            list.Add("두번째 아이템");
            ConsoleWriteAll(list);

            list.Remove("첫번째 아이템");
            ConsoleWriteAll(list);

            //ConsoleWriteAllWithForeach(list);

            Console.ReadLine();
            
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
