using ChristWare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Offsets.Signatures
{
    public static class Scanner
    {
        public static bool TryFind(byte[] bytes, PatternAttribute pattern, out int offset) => 
            TryFind(bytes, pattern.Pattern as byte[], pattern.Wildcards as bool[], out offset);

        public static bool TryFind(byte[] bytes, byte[] patternBytes, bool[] wildcards, out int offset)
        {
            offset = default;

            for (offset = 0; offset < bytes.Length - patternBytes.Length; offset++)
                if (CheckMask(bytes, patternBytes, wildcards, offset))
                    return true;

            return false;
        }

        public static bool CheckMask(byte[] bytes, byte[] patternBytes, bool[] wildcards, int offset)
        {
            for (var i = 0; i < patternBytes.Length; i++)
            {
                if (wildcards[i])
                    continue;

                if (bytes[offset + i] != patternBytes[i])
                    return false;
            }
            return true;
        }
    }
}
