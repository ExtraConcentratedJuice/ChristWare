using ChristWare.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class TriggerBot : Component, ITickHandler
    {
        public override string Name => "TriggerBot";
        public override char Hotkey => 'i';

        private volatile bool firing;

        public TriggerBot(IntPtr processHandle, IntPtr clientAddress, ChristConfiguration configuration)
            : base(processHandle, clientAddress, configuration)
        {
            new Thread(CheckFire).Start();
        }

        public void OnTick()
        {
            // Keycode for back mouse button
            if (!KeyUtility.IsKeyDown(0x05))
                return;

            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var team = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);
            var inCrosshair = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iCrosshairId);

            if (inCrosshair > 0 && inCrosshair < 32)
            {
                var entity = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + (inCrosshair - 1) * 0x10);

                if (entity != 0)
                {
                    var otherTeam = Memory.Read<int>(processHandle, entity + Netvars.m_iTeamNum);

                    if (otherTeam != team && !firing)
                        firing = true;
                }
            }
        }

        private void CheckFire()
        {
            while (true)
            {
                if (firing)
                {
                    Thread.Sleep(ThreadSafeRandom.Next(75, 115));
                    Clicker.TriggerLeftClick(ThreadSafeRandom.Next(15, 25));

                   firing = false;
                }

                Thread.Sleep(2);
            }
        }

        protected override void OnDisable()
        {
            Beeper.Beep(195, 215);

        }

        protected override void OnEnable()
        {
            Beeper.Beep(783, 215);
        }
    }
}
