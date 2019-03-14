using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class ESP : Component, IEntityHandler
    {
        public override string Name => "ESP";
        public override HotKey DefaultHotkey => new HotKey('p');

        public ESP(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration) 
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
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

            var r = entityTeamId != teamId ? configuration.Value.EnemyR : configuration.Value.FriendlyR;
            var g = entityTeamId != teamId ? configuration.Value.EnemyG : configuration.Value.FriendlyG;
            var b = entityTeamId != teamId ? configuration.Value.EnemyB : configuration.Value.FriendlyB;

            Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x4, r / 255F); // R
            Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x8, g / 255F); // G
            Memory.Write<float>(processHandle, manager + glow * 0x38 + 0xC, b / 255F); // B
            Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x10, 1F); // Alpha
            Memory.Write<int>(processHandle, manager + glow * 0x38 + 0x24, 1); // Toggle
        }
    }
}
