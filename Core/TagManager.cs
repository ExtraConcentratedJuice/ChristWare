using ChristWare.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public class TagManager
    {
        private readonly string tag;
        private int shiftCount = 0;

        public TagManager(string tag)
        {
            if (tag.Length > 15)
                throw new ArgumentException("A tag length of 15 or less is expected.", nameof(tag));

            this.tag = tag;
        }

        public string ShiftOne()
        {
            if (shiftCount >= tag.Length)
                shiftCount = 0;


            return tag.Shift(shiftCount++);
        }

        public byte[] ShiftOneBytes() => Encoding.UTF8.GetBytes(ShiftOne() + '\0');
    }
}
