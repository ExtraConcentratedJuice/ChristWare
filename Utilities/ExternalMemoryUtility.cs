using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    [Flags]
    public enum AllocationType
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000
    }

    [Flags]
    public enum MemoryProtection
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        GuardModifierflag = 0x100,
        NoCacheModifierflag = 0x200,
        WriteCombineModifierflag = 0x400
    }

    public static class ExternalMemoryUtility
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
            uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
            int dwSize, AllocationType dwFreeType);

        [DllImport("kernel32.dll")]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
            int dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            IntPtr lpThreadAttributes, IntPtr dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, IntPtr dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public static IntPtr Allocate(IntPtr handle, IntPtr location, uint size) =>
            VirtualAllocEx(handle, location, size, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);

        public static void Free(IntPtr handle, IntPtr location, int size) =>
            VirtualFreeEx(handle, location, size, AllocationType.Release);

        public static void ExecuteShellcode(IntPtr handle, IntPtr address, byte[] code)
        {
            int written = default;

            VirtualProtectEx(handle, address, code.Length, (uint)MemoryProtection.ExecuteReadWrite, out var old);

            Memory.WriteProcessMemory((int)handle, (int)address, code, code.Length, ref written);
            var threadHandle = CreateRemoteThread(handle, IntPtr.Zero, IntPtr.Zero, address, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            WaitForSingleObject(threadHandle, 0xFFFFFFFF);

            VirtualProtectEx(handle, address, code.Length, old, out var _);

            CloseHandle(threadHandle);
        }
    }
}
