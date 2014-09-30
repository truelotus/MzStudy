using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudyConsoleProject
{
    class ConsoleList
    {
        public ConsoleList() 
        {
            OutPutTest();
        }

        private void OutPutTest() 
        {
            //List class 만들기. younhome(https://github.com/truelotus/MzStudy.git)에서 수정함.
            List<String> listOrigin = new List<string>();
            listOrigin.Add("첫번째 아이템");
            Console.WriteLine("==============================.Net List start==============================");
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

            //ConsoleWriteAllWithForeach(listOrigin);

            // ConsoleWriteAll(listOrigin);

            String[] Array = new String[10];
            Array[1] = "복사한 아이템";
            listOrigin.CopyTo(Array, 5);
            Console.WriteLine("----------------------------▽Copy To array----------------------------");
            ConsoleWriteAll(Array);

            Console.WriteLine("----------------------------▽foreach----------------------------");
            ConsoleWriteAllWithForeach(listOrigin);


            YounList<String> list = new YounList<string>();
            list.Add("첫번째 아이템");
            Console.WriteLine("==============================YounList start==============================");

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

            Array = new String[10];
            Array[1] = "복사한 아이템";
            list.CopyTo(Array, 5);
            Console.WriteLine("----------------------▽Copy To array------------------------------------");
            ConsoleWriteAll(Array);

            Console.WriteLine("----------------------------▽foreach(double)----------------------------");
            ConsoleWriteAllWithForeach(list);

            //where 절은 쿼리 식에서 반환할 데이터 소스의 요소를 쿼리 식에 지정하는 데 사용됩니다. 
            //var m = list.YounWhere(GetEqualsItem);

            var m = list.YounWhere(item => item.Equals("이상한 아이템"));

            ConsoleWriteAllWithForeach(m);
            Console.WriteLine("");
            list.YounForeach(Console.WriteLine);

            Console.WriteLine("");

            var newList = new List<string>();
            list.YounForeach(newList.Add);


            //select 절은 쿼리가 실행될 때 생성할 값의 형식을 지정합니다.

            //Q.string list내에서 조건에 부합한다면 그 조건결과값으로 구성된. boolean list로 변환하기.
            var m1 = list.YounSelect(item => item.Equals("첫번째 아이템"));
            foreach (var item in m1)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("");

            var m2 = list.YounSelect(item => IsEquals(item));
            foreach (var item in m2)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("");

            //Q.앞에 3글자만 잘린 리스트를 출력해보자
            var m3 = list.YounSelect(item => GetFrontThreeWord(item));
            //더 간결하게는 아래와 같다.
            //var m3 = list.Select(item => item.Substring(0, 3));
            //var m3 = list.Select(GetFrontThreeWord);
            m3.YounForeach(Console.WriteLine);
            Console.WriteLine("");

            //Q.이번엔 3글자로 자른 다음에 첫 글짜가 "이"인 결과를 만들어 보세요
            //var m4 = list.YounSelect(GetFrontThreeWord);
            //m4.YounWhereTwo(GetWord);

            list.YounSelect(GetFrontThreeWord)
                .YounWhere(item => item.Contains("이"))
                .YounForeach(Console.WriteLine);
            Console.WriteLine("");


            Console.ReadLine();
        }

        private static bool GetWord(string item)
        {
            if (item.Contains("이"))
                return true;
            else
                return false;
        }

        private static string GetFrontThreeWord(string item)
        {

            return item.Substring(0, 3);
        }

        private static bool IsEquals(string item)
        {
            return item.Equals("첫번째 아이템");
        }

        private static string GetEqualsItem(string item)
        {
            if (item.Equals("첫번째 아이템"))
            {
                return item;
            }
            return null;
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
