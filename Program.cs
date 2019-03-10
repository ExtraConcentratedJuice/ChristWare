using ChristWare.Core;
using ChristWare.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            handler += OnClose;
            SetConsoleCtrlHandler(handler, true);
            Console.WindowWidth = 152;
            Console.WindowHeight = 48;

            var configuration = new ConfigurationManager<ChristConfiguration>("christconfig.json");
            new ChristWare("csgo", configuration).Run();
        }
    }
}
