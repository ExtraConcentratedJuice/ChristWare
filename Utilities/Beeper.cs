using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class Beeper
    {
        public static void Beep(int frequency, int timeInMilliseconds)
        {
            if (timeInMilliseconds >= 1000)
            {
                new Thread(() => Console.Beep(frequency, timeInMilliseconds)).Start();
            }
            else
            {
                ThreadPool.QueueUserWorkItem(_ => Console.Beep(frequency, timeInMilliseconds), null);
            }
        }
    }
}
