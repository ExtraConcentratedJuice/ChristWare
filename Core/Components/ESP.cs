using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class ESP : Component
    {
        public override string Name { get => "ESP"; }
        public override char Hotkey { get => 'p'; }

        public ESP(IntPtr processHandle, IntPtr clientAddress, ChristConfiguration configuration) 
            : base(processHandle, clientAddress, configuration)
        {
        }

        protected override void OnDisable()
        {
            Beeper.Beep(195, 215);

        }

        protected override void OnEnable()
        {
            Beeper.Beep(783, 215);
        }

        public override void OnTick()
        {
            var manager = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwGlowObjectManager);
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var teamId = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);

            for (int i = 1; i <= 32; i++)
            {
                var entity = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + i * 0x10);

                if (entity != 0)
                {
                    var health = Memory.Read<int>(processHandle, entity + Netvars.m_iHealth);

                    if (health <= 0)
                        continue;

                    var glow = Memory.Read<int>(processHandle, entity + Netvars.m_iGlowIndex);
                    var entityTeamId = Memory.Read<int>(processHandle, entity + Netvars.m_iTeamNum);

                    var r = entityTeamId != teamId ? configuration.EnemyR : configuration.FriendlyR;
                    var g = entityTeamId != teamId ? configuration.EnemyG : configuration.FriendlyG;
                    var b = entityTeamId != teamId ? configuration.EnemyB : configuration.FriendlyB;

                    Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x4, r / 255F); // R
                    Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x8, g / 255F); // G
                    Memory.Write<float>(processHandle, manager + glow * 0x38 + 0xC, b / 255F); // B
                    Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x10, 1F); // Alpha
                    Memory.Write<int>(processHandle, manager + glow * 0x38 + 0x24, 1); // Toggle
                }
            }
        }
    }
}
