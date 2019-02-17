using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class BunnyHop : Component, ITickHandler
    {
        public override string Name => "BunnyHop";
        public override char Hotkey => 'n';

        private volatile bool jumping;

        private const int FL_ONGROUND = (1 << 0);

        public BunnyHop(IntPtr processHandle, IntPtr clientAddress, ChristConfiguration configuration)
            : base(processHandle, clientAddress, configuration)
        {
            new Thread(CheckJump).Start();
        }

        public void OnTick()
        {
            // Middle mouse button = 0x06
            if (jumping || !KeyUtility.IsKeyDown(0x04))
                return;

            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var flags = Memory.Read<int>(processHandle, localPlayer+ Netvars.m_fFlags);

            if ((flags & FL_ONGROUND) == 1)
                jumping = true;
        }

        private void CheckJump()
        {
            var jumpAdr = (int)clientAddress + Signatures.dwForceJump;

            while (true)
            {
                if (jumping)
                {
                    jumping = false;

                    Memory.Write<int>(processHandle, jumpAdr, 5);
                    Thread.Sleep(ThreadSafeRandom.Next(12, 20));
                    Memory.Write<int>(processHandle, jumpAdr, 4);
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
