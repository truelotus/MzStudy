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
            // younhome(https://github.com/truelotus/MzStudy.git)에서 수정함.

            //일반 List<string>과 동일하게 동작하는 List 클래스를 만들어 비교해보자.
            List<String> listOrigin = new List<string>();
            listOrigin.Add("첫번째 아이템");
            Console.WriteLine("==============================List start==============================");
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

            String[] arr = new String[10];
            arr[1] = "복사한 아이템";
            listOrigin.CopyTo(arr, 5);
            Console.WriteLine("----------------------------▽Copy To array----------------------------");
            ConsoleWriteAll(arr);

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

            arr = new String[10];
            arr[1] = "복사한 아이템";
            list.CopyTo(arr, 5);
            Console.WriteLine("----------------------▽Copy To array------------------------------------");
            ConsoleWriteAll(arr);

            Console.WriteLine("----------------------------▽foreach(double)----------------------------");
            ConsoleWriteAllWithForeach(list);
            
            //Where를 사용하여 람다식으로 아이템 추출하기.
            var strange = list.YounWhere(item => item.Equals("이상한 아이템"));
            ConsoleWriteAllWithForeach(strange);
            Console.WriteLine("");
            list.YounForeach(Console.WriteLine);

            Console.WriteLine("");

            var newList = new List<string>();
            list.YounForeach(newList.Add);


            //select 절은 쿼리가 실행될 때 생성할 값의 형식을 지정합니다.

            //Q.string list내에서 조건에 부합한다면 그 조건 결과 값으로 구성된. boolean list로 변환하기.
            var firstList = list.Select(item => item.Equals("첫번째 아이템"));
            foreach (var item in firstList)
            {
               Console.WriteLine(item);
            }
            Console.WriteLine("");

            var boolTypeList = list.YounSelect(item => IsFirst(item));
            foreach (var item in boolTypeList)
            {
               Console.WriteLine(item);
            }
            Console.WriteLine("");
            
            //Q.앞에 3글자만 잘린 리스트를 출력해보자
            var threeWordList = list.YounSelect(item => GetFrontThreeWord(item));
            //더 간결하게는 아래와 같이 표현 할 수 있다.
            //var threeWordList = list.Select(item => item.Substring(0, 3));
            //var threeWordList = list.Select(GetFrontThreeWord);
            threeWordList.YounForeach(Console.WriteLine);
            Console.WriteLine("");

            //Q.이번엔 3글자로 자른 다음에 첫 글짜가 "이"인 결과를 만들어 보자

            //var threeWordList = list.YounSelect(GetFrontThreeWord);

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

        private static string GetFrontThreeWord(string item) {

            return item.Substring(0, 3);
        }

        private static bool IsFirst(string item) 
        {
            return item.Equals("첫번째 아이템");
        }

        private static void Print(string item)
        {
            Console.WriteLine(item);
        }

        private static bool IsStrange(string item, string item2)
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
