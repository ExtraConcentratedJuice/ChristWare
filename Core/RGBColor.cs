using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public struct RGBColor
    {
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public byte R { get; set; }
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public byte G { get; set; }
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public byte B { get; set; }
    }
}
