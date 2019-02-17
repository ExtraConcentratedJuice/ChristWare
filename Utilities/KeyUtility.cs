using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class KeyUtility
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        public static bool IsKeyDown(int key) => GetAsyncKeyState(key) != 0;
    }
}
