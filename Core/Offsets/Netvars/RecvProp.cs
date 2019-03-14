using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Offsets.Netvars
{
    public class RecvProp : ExternalClassWrapper
    {
        public int ProcOffset { get; set; }

        public RecvProp(IntPtr proc, int baseAdr) : base(proc, baseAdr) { }

        public int Table => Read<int>(BaseAddress + 0x28);
        public string Name => ReadString(Read<int>(BaseAddress), 64, System.Text.Encoding.Default);
        public int Offset => Read<int>(BaseAddress + 0x2C) + ProcOffset;
        public int Type => Read<int>(BaseAddress + 0x4);
        public int Elements => Read<int>(BaseAddress + 0x34);
        public int StringBufferCount => Read<int>(BaseAddress + 0xC);
    }
}
