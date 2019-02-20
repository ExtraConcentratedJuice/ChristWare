using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class RecoilControl : Component, ITickHandler
    {
        public override string Name => "RecoilControl";
        public override HotKey DefaultHotkey => new HotKey('[');

        public RecoilControl(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ChristConfiguration configuration)
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
        }

        private Vector3 oldAngle = Vector3.Zero;

        public void OnTick()
        {
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var shots = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iShotsFired);
            var clientState = Memory.Read<int>(processHandle, (int)engineAddress + Signatures.dwClientState);

            if (shots == 0)
                return;

            if (shots > 1)
            {
                var viewAngle = Memory.Read<Vector3>(processHandle, clientState + Signatures.dwClientState_ViewAngles);
                var punchAngle = Memory.Read<Vector3>(processHandle, localPlayer + Netvars.m_aimPunchAngle);

                viewAngle += oldAngle;


                var newAngle = viewAngle - (punchAngle * 2);

                Memory.Write<Vector3>(processHandle, clientState + Signatures.dwClientState_ViewAngles, newAngle);

                oldAngle = punchAngle * 2;
            }
            else
            {
                oldAngle = Vector3.Zero;
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
