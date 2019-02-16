using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class ConsoleUtility
    {
        public static void WriteLineColor(object text, ConsoleColor color = ConsoleColor.White, bool takeLine = true)
        {
            Console.ForegroundColor = color;

            if (takeLine)
                Console.WriteLine(text + new String(' ', Console.WindowWidth - text.ToString().Length - 2));
            else
                Console.WriteLine(text);

            Console.ResetColor();
        }

        public static void WriteColor(object text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
