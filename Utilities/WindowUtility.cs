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
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string GetActiveWindowName()
        {
            var buffer = new StringBuilder(256);
            return GetWindowText(GetForegroundWindow(), buffer, 256) > 0 ? buffer.ToString() : null;
        }
    }
}
