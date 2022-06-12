using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class BombTimer : Component, IEntityHandler
    {
        public override string Name => "Bomb Timer/ESP";
        public override HotKey DefaultHotkey => new HotKey('8');

        public BombTimer(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
    : base(processHandle, clientAddress, engineAddress, configuration)
        {
        }

        public void HandleEntity(int entity)
        {
            var manager = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwGlowObjectManager);
            var glow = Memory.Read<int>(processHandle, entity + Netvars.m_iGlowIndex);
            var bomb = Memory.Read<int>(processHandle, (int)clientAddress + Netvars.m_flDefuseLength);
            var color = configuration.Value.FriendlyColor;
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            // Signatures.dwEntityList 
            //Console.WriteLine("ooga booga" + glow);
            for (int i = 1; i <= 32; i++)
            {
                var entities = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + i * 0x10);

                if (entities != 0)
                {
                    Console.WriteLine(bomb);
                }
            }
                    //  Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x4, color.R / 255F); // R
                    //  Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x8, color.G / 255F); // G
                    // Memory.Write<float>(processHandle, manager + glow * 0x38 + 0xC, color.B / 255F); // B
                    // Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x10, 1F); // Alpha
                 //   Memory.Write<int>(processHandle, manager + glow * 0x38 + 0x24, 1); // Toggle
        }
    }
}
