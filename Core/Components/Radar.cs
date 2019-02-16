using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class Radar : Component, IEntityHandler
    {
        public override string Name => "Radar";
        public override char Hotkey => 'o';

        public Radar(IntPtr processHandle, IntPtr clientAddress, ChristConfiguration configuration)
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

        public void HandleEntity(int entity)
        {
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var teamId = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);

            var health = Memory.Read<int>(processHandle, entity + Netvars.m_iHealth);

            if (health <= 0)
                return ;

            var entityTeamId = Memory.Read<int>(processHandle, entity + Netvars.m_iTeamNum);

            if (entityTeamId != teamId)
                Memory.Write<int>(processHandle, entity + Netvars.m_bSpotted, 1);
        }
    }
}
