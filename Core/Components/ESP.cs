using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class ESP : Component, IEntityHandler
    {
        public override string Name => "ESP";
        public override HotKey DefaultHotkey => new HotKey('p');

        public ESP(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ChristConfiguration configuration) 
            : base(processHandle, clientAddress, engineAddress, configuration)
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

        public void HandleEntity(int entity)
        {
            var manager = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwGlowObjectManager);
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var teamId = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);

            var health = Memory.Read<int>(processHandle, entity + Netvars.m_iHealth);

            if (health <= 0)
                return;

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
