using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Offsets.Netvars
{
    public class RecvTable : ExternalClassWrapper
    {
        public RecvTable(IntPtr proc, int baseAdr) : base(proc, baseAdr) { }

        public int GetPropById(int id) => Read<int>(BaseAddress) + (id * 0x3C);
        public string TableName => ReadString(Read<int>(BaseAddress + 0xC), 32, System.Text.Encoding.Default);
        public int PropCount => Read<int>(BaseAddress + 0x4);
        public bool Readable => Memory.CanRead(processHandle, BaseAddress, 0x10);
    }
}
