using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class Chams : Component, IEntityHandler
    {
        public override string Name => "Chams";
        public override HotKey DefaultHotkey => new HotKey(',');

        public Chams(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
        }

        public void HandleEntity(int entity)
        {
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var teamId = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);

            var entityTeamId = Memory.Read<int>(processHandle, entity + Netvars.m_iTeamNum);

            var r = entityTeamId != teamId ? configuration.Value.EnemyChamsR : configuration.Value.FriendlyChamsR;
            var g = entityTeamId != teamId ? configuration.Value.EnemyChamsG : configuration.Value.FriendlyChamsG;
            var b = entityTeamId != teamId ? configuration.Value.EnemyChamsB : configuration.Value.FriendlyChamsB;

            var entityRender = entity + Netvars.m_clrRender;

            Memory.Write<byte>(processHandle, entityRender, r); // R
            Memory.Write<byte>(processHandle, entityRender + 0x1, g); // G
            Memory.Write<byte>(processHandle, entityRender + 0x2, b); // B

            var ambient = Memory.Read<int>(processHandle, (int)engineAddress + Signatures.model_ambient_min - 0x2C);
            Memory.Write<int>(processHandle, (int)engineAddress + Signatures.model_ambient_min, BitUtility.XorFloat(configuration.Value.ChamsBrightness, ambient));
        }

        protected override void OnDisable()
        {
            for (int i = 1; i <= 32; i++)
            {
                var entity = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + i * 0x10);

                if (entity != 0)
                {
                    var entityRender = entity + Netvars.m_clrRender;

                    Memory.Write<byte>(processHandle, entityRender, 255); // R
                    Memory.Write<byte>(processHandle, entityRender + 0x1, 255); // G
                    Memory.Write<byte>(processHandle, entityRender + 0x2, 255); // B
                }
            }

            var ambient = Memory.Read<int>(processHandle, (int)engineAddress + Signatures.model_ambient_min - 0x2C);
            Memory.Write<int>(processHandle, (int)engineAddress + Signatures.model_ambient_min, BitUtility.XorFloat(0F, ambient));
        }
    }
}
