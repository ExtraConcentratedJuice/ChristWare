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

            var color = entityTeamId != teamId ? configuration.Value.EnemyChamsColor : configuration.Value.FriendlyChamsColor;
            //if (!configuration.Value.TeamESP && entityTeamId == teamId)
            //{
            //    return;
            //}
            var entityRender = entity + Netvars.m_clrRender;

            Memory.Write<RGBColor>(processHandle, entityRender, color);

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

                    Memory.Write<RGBColor>(processHandle, entityRender, new RGBColor { R = 255, G = 255, B = 255 });
                }
            }

            var ambient = Memory.Read<int>(processHandle, (int)engineAddress + Signatures.model_ambient_min - 0x2C);
            Memory.Write<int>(processHandle, (int)engineAddress + Signatures.model_ambient_min, BitUtility.XorFloat(0F, ambient));
        }
    }
}
