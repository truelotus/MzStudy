﻿using System;
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
        static void Main(string[] args)
        {

            DownloadWebImage();
            Console.ReadLine();
        }

        public static void DownloadWebImage() 
        {
            String[] searchWordList = new String[] { "강아지", "고양이", "코끼리", "호랑이", "돌고래", "코알라", "비버", "다람쥐", "기린", "벌새" };

            ImageDownloadManager downManager = new ImageDownloadManager(searchWordList);

            downManager.StartListSearchDownload();
        }
    }
}
