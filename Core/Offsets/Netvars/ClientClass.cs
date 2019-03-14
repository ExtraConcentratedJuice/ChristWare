using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Offsets.Netvars
{
    public class ClientClass : ExternalClassWrapper
    {
        public ClientClass(IntPtr proc, int baseAdr) : base(proc, baseAdr) { }

        public int ClassId => Read<int>(BaseAddress + 0x14);
        public string ClassName => ReadString(Read<int>(BaseAddress + 0x8), 64, System.Text.Encoding.Default);
        public int Next => Read<int>(BaseAddress + 0x10);
        public int Table => Read<int>(BaseAddress + 0xC);
        public bool Readable => Memory.CanRead(processHandle, BaseAddress, 0x28);
    }
}
