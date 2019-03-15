using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Offsets.Signatures
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PatternAttribute : Attribute
    {
        private readonly string signature;
        public string Module { get; }
        public int Extra { get; }
        public bool Relative { get; }
        private readonly int[] offsets;

        private readonly byte[] patternBytes;
        private readonly bool[] wildcards;

        public IReadOnlyList<byte> Pattern { get => patternBytes; }
        public IReadOnlyList<bool> Wildcards { get => wildcards; }
        public IReadOnlyList<int> Offsets { get => offsets; }

        public PatternAttribute(string module, string signature, int extra, bool relative, int[] offsets)
        {
            this.Module = module;
            this.signature = signature;
            this.Extra = extra;
            this.Relative = relative;
            this.offsets = offsets;

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
