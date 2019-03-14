using ChristWare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class ScannerUtility
    {
        public static bool TryFind(byte[] bytes, PatternAttribute pattern, out int offset) => 
            TryFind(bytes, pattern.Pattern as byte[], pattern.Wildcards as bool[], out offset);

        public static bool TryFind(byte[] bytes, byte[] patternBytes, bool[] wildcards, out int offset)
        {
            var found = false;

            for (offset = 0; offset < bytes.Length - patternBytes.Length; offset++)
            {
                if (found)
                    break;

                found = true;

                for (var k = 0; k < patternBytes.Length; k++)
                {
                    if (wildcards[k])
                        continue;

                    if (bytes[offset + k] != patternBytes[k])
                    {
                        found = false;
                        break;
                    }
                }
            }

            return found;
        }
    }
}
