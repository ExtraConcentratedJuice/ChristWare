using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Extensions
{
    public static class StringExtensions
    {
        public static string Shift(this string s, int count)
        {
            return s.Remove(0, count) + s.Substring(0, count);
        }
    }
}
