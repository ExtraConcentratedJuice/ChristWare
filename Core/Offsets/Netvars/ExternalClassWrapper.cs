using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Offsets
{
    // This might be useful elsewhere...
    public class ExternalClassWrapper
    {
        protected readonly IntPtr processHandle;
        public int BaseAddress { get; set; }

        public ExternalClassWrapper(IntPtr processHandle, int baseAddress)
        {
            this.processHandle = processHandle;
            this.BaseAddress = baseAddress;
        }

        protected T Read<T>(int address) where T: struct => Memory.Read<T>(processHandle, address);
        protected string ReadString(int address, int size, Encoding encoding) => Memory.ReadString(processHandle, address, size, encoding);
    }
}
