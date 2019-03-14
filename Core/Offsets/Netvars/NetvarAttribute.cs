using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Offsets
{
    public class NetvarAttribute : Attribute
    {
        public string Table { get; set; }
        public string Name { get; set; }
        public int Offset { get; set; }

        public NetvarAttribute(string table, string name, int offset = 0)
        {
            Table = table;
            Name = name;
            Offset = offset;
        }
    }
}
