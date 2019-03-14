using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PatternAttribute : Attribute
    {
        private readonly string signature;
        private readonly string module;

        private readonly byte[] patternBytes;
        private readonly bool[] wildcards;

        public IReadOnlyList<byte> Pattern { get => patternBytes; }
        public IReadOnlyList<bool> Wildcards { get => wildcards; }

        public PatternAttribute(string module, string signature)
        {
            this.module = module;
            this.signature = signature;

            var split = signature.Split(' ');

            this.patternBytes = new byte[split.Length];
            this.wildcards = new bool[split.Length];

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i] == "?")
                    wildcards[i] = true;
                else
                    patternBytes[i] = byte.Parse(split[i], NumberStyles.HexNumber);
            }
        }
    }
}
