using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class ThirdPerson : Component
    {
        public override string Name => "ThirdPerson";
        public override HotKey DefaultHotkey => new HotKey('-');

        public ThirdPerson(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
        }

        protected override void OnEnable()
        {
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            Memory.Write<int>(processHandle, localPlayer + Netvars.m_iObserverMode, 5);
        }

        protected override void OnDisable()
        {
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            Memory.Write<int>(processHandle, localPlayer + Netvars.m_iObserverMode, 0);
        }
    }
}
