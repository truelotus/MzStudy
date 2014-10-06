using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace StudyConsoleProject.File
{
    class APMTechExam
    {

        /// <summary>
        /// APM 작업 완료 대기 랑데부 테크닉
        /// </summary>
        public void ReadFileStreamOfApm() 
        {
            //Resources디렉토리의 Penguins.jpg 파일을 열면서 비동기로 I/O작업을 진행 할 것을 요청한다.
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\Penguins.jpg";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, 
                    FileShare.Read, 1024, FileOptions.Asynchronous);
            
            Byte[] data = new Byte[100];
            
            //FileStream을 통해서 진행될 읽기 비동기 작업을 시작한다.
            IAsyncResult ar = fs.BeginRead(data, 0, data.Length, null, null);

            //다른 작업 코드들이 이곳에서 실행 될 수 있다.*

            //비동기 작업이 완료 될때 까지 대기한 후 작업이 완료되면 결과를 반환 한다.
            Int32 bytesRead = fs.EndRead(ar);

            fs.Close();

            Console.WriteLine("Number od bytes read = {0}", bytesRead);
            Console.WriteLine(BitConverter.ToString(data,0,bytesRead));
        }

        /// <summary>
        /// APM 폴링 랑데부 테크닉 -IsCompleted 속성 이용
        /// </summary>
        public void ReadFileStreamOfPolling() 
        {
            //Resources디렉토리의 Penguins.jpg 파일을 열면서 비동기로 I/O작업을 진행 할 것을 요청한다.
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\Penguins.jpg";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read,
                    FileShare.Read, 1024, FileOptions.Asynchronous);

            Byte[] data = new Byte[100];

            //FileStream을 통해서 진행될 읽기 비동기 작업을 시작한다.
            IAsyncResult ar = fs.BeginRead(data, 0, data.Length, null, null);

            while (!ar.IsCompleted)
            {
                Console.WriteLine("Operation not completed; still waiting...");
                Thread.Sleep(10);
            }

            //결과를 얻는다. 노트: EndRead 메서드는 이 스레드를 중지하지 않을 것이다.
            Int32 bytesRead = fs.EndRead(ar);

            fs.Close();

            Console.WriteLine("Number od bytes read = {0}", bytesRead);
            Console.WriteLine(BitConverter.ToString(data, 0, bytesRead));

        }

        /// <summary>
        /// APM 폴링 랑데부 테크닉 -AsyncwaitHandle 속성 이용
        /// </summary>
        public void ReadFileStreamOfPolling2() 
        {
            //Resources디렉토리의 Penguins.jpg 파일을 열면서 비동기로 I/O작업을 진행 할 것을 요청한다.
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\Penguins.jpg";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read,
                    FileShare.Read, 1024, FileOptions.Asynchronous);

            Byte[] data = new Byte[100];

            //FileStream을 통해서 진행될 읽기 비동기 작업을 시작한다.
            IAsyncResult ar = fs.BeginRead(data, 0, data.Length, null, null);

            //AsyncwaitHandle 속성 이용
            while (!ar.AsyncWaitHandle.WaitOne(10,false))
            {
                Console.WriteLine("Operation not completed; still waiting...");
            }

            //결과를 얻는다.
            Int32 bytesRead = fs.EndRead(ar);

            fs.Close();

            Console.WriteLine("Number od bytes read = {0}", bytesRead);
            Console.WriteLine(BitConverter.ToString(data, 0, bytesRead));
        }

    }
}
