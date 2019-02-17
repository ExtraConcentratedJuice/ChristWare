﻿using System;
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

        public static bool TryGetProcessHandle(string name, out Process process, out IntPtr handle)
        {
            handle = default;

            process = Process.GetProcessesByName(name).FirstOrDefault();

            if (process == null)
                return false;

            handle = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, process.Id);

            return true;
        }

        public static bool TryGetProcessModule(Process process, string moduleName, out IntPtr address)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            address = default;

            foreach (var x in process.Modules.OfType<ProcessModule>())
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
