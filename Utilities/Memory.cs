using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class Memory
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public static T Read<T>(IntPtr handle, int address) where T : struct => Read<T>(handle, address, out var _);
        public static void Write<T>(IntPtr handle, int address, T value) where T : struct => Write<T>(handle, address, value, out var _);

        public static T Read<T>(IntPtr handle, int address, out int bytesRead) where T : struct
        {
            bytesRead = default;

            var size = Marshal.SizeOf(typeof(T));
            byte[] temp = new byte[size];

            ReadProcessMemory((int)handle, address, temp, temp.Length, ref bytesRead);

            var bytesHandle = GCHandle.Alloc(temp, GCHandleType.Pinned);

            try
            {
                return (T)Marshal.PtrToStructure(bytesHandle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                bytesHandle.Free();
            }
        }

        public static void Write<T>(IntPtr handle, int address, T value, out int bytesWritten) where T : struct
        {
            bytesWritten = default;

            var size = Marshal.SizeOf(typeof(T));
            byte[] temp = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, temp, 0, size);
            Marshal.FreeHGlobal(ptr);

            WriteProcessMemory((int)handle, address, temp, size, ref bytesWritten);
        }

        public static string ReadString(IntPtr handle, int address, int size, Encoding encoding) => ReadString(handle, address, size, encoding, out var _);

        public static string ReadString(IntPtr handle, int address, int size, Encoding encoding, out int bytesRead)
        {
            bytesRead = default;
            byte[] buffer = new byte[size];

            ReadProcessMemory((int)handle, address, buffer, size, ref bytesRead);

            string text = encoding.GetString(buffer);
            var nullTermIndex = text.IndexOf('\0');

            //return nullTermIndex == -1 ? text : text.Substring(0, nullTermIndex);
            return text;
        }
    }
}
