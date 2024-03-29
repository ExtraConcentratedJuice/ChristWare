﻿using ChristWare.Core;
using ChristWare.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare
{
    public class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler handler, bool add);

        private delegate bool ConsoleCtrlHandler(int sig);
        private static ConsoleCtrlHandler handler;

        private static bool OnClose(int sig)
        {
            ChristWare.ExitSequence();
            return true;
        }

        static void Main(string[] args)
        {
            if (Assembly.GetExecutingAssembly().GetType("ChristWare") != null)
            {
                Console.WriteLine("Assembly is not obfuscated, closing now to prevent VAC sigging...");
                Environment.Exit(0);
            }

            handler += OnClose;
            SetConsoleCtrlHandler(handler, true);

            Console.WindowWidth = Math.Min(154, Console.LargestWindowWidth);
            Console.WindowHeight = Math.Min(58, Console.LargestWindowHeight);

            //ChristWareUI.CreateWindow();

            //Console.ReadLine();

            var configuration = new ConfigurationManager<ChristConfiguration>("christconfig.json");
            new ChristWare("csgo", configuration).Run();
        }
    }
}
