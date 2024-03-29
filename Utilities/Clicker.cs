﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class Clicker
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void TriggerLeftClick(int delay)
        {
            Click(MOUSEEVENTF_LEFTDOWN);
            Thread.Sleep(delay);
            Click(MOUSEEVENTF_LEFTUP);
        }
        public static void TriggerRightClick(int delay)
        {
            Click(MOUSEEVENTF_RIGHTDOWN);
            Thread.Sleep(delay);
            Click(MOUSEEVENTF_RIGHTUP);
        }

        private static void Click(uint flags) => mouse_event(flags, 0, 0, 0, 0);
    }
}
