using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class AntiFlash : Component, ITickHandler
    {
        public override string Name => "AntiFlash";
        public override HotKey DefaultHotkey => new HotKey(']');

        public AntiFlash(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
        }

        public void OnTick()
        {
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            Memory.Write<float>(processHandle, localPlayer + Netvars.m_flFlashMaxAlpha, 255F / 2F);
        }
    }
}
