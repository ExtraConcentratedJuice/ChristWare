using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class WindowUtility
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        public static string GetActiveWindowName()
        {
            var buffer = new StringBuilder(256);
            return GetWindowText(GetForegroundWindow(), buffer, 256) > 0 ? buffer.ToString() : null;
        }
    }
}
