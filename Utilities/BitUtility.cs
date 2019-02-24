using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class BitUtility
    {
        public static int XorFloat(float input, int xor) => BitConverter.ToInt32(BitConverter.GetBytes(input), 0) ^ xor;
    }
}
