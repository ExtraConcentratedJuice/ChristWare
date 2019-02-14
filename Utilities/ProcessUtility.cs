using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class ProcessUtility
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        public const int PROCESS_VM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_VM_OPERATION = 0x0008;

        public static bool TryGetProcessHandle(string name, out IntPtr handle)
        {
            handle = default;

            var proc = Process.GetProcessesByName(name).FirstOrDefault();

            if (proc == null)
                return false;

            handle = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, proc.Id);

            return true;
        }

        public static bool TryGetProcessModule(string procName, string moduleName, out IntPtr address)
        {
            address = default;

            var proc = Process.GetProcessesByName(procName).FirstOrDefault();

            if (proc == null)
                return false;

            foreach (var x in proc.Modules.OfType<ProcessModule>())
            {
                if (x.ModuleName == moduleName)
                {
                    address = x.BaseAddress;
                    return address != IntPtr.Zero;
                }
            }

            return false;
        }
    }
}
