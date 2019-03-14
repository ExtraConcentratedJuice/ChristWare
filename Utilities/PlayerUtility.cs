using ChristWare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class PlayerUtility
    {
        public static Vector3 ReadBone(IntPtr procHandle, int player, int position)
        {
            var boneMatrix = Memory.Read<int>(procHandle, player + Netvars.m_dwBoneMatrix);

            position *= 0x30;

            return new Vector3
            {
                X = Memory.Read<float>(procHandle, boneMatrix + position + 0x0C),
                Y = Memory.Read<float>(procHandle, boneMatrix + position + 0x1C),
                Z = Memory.Read<float>(procHandle, boneMatrix + position + 0x2C),
            };
        }
    }
}
