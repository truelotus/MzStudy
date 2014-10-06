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
using StudyConsoleProject.Server.CameraControllerCore.Server;
using StudyConsoleProject.File;


namespace StudyConsoleProject
{

    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new KoServer())
            {
                server.Run();

                Console.ReadLine();
            }


            //FileExplorerForm form = new FileExplorerForm();
            //form.ShowDialog();

            //웹 이미지 다운로드 샘플 동작 코드
            //WebImageDownloadManagerExecute();

            //APM 샘플 동작코드 
            //APMTechExam ex = new APMTechExam();
            //ex.ReadFileStreamOfApm();
            //ex.ReadFileStreamOfPolling();
            Console.ReadLine();
        }

        /// <summary>
        /// 웹 검색(goole) 대상을 지정하여 검색을 시작 합니다.
        /// </summary>
        public static void WebImageDownloadManagerExecute() 
        {
            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "돌고래", "코알라", "비버", "다람쥐", "기린", "벌새" };

            ImageDownloadManager downManager = new ImageDownloadManager(searchWordList);

            downManager.StartListSearchDownload();
        }
    }
}
