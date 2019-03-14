using ChristWare.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class TriggerBot : Component, ITickHandler
    {
        public override string Name => "TriggerBot";
        public override HotKey DefaultHotkey => new HotKey('i');
        private readonly HotKey triggerBotHoldKey;

        private volatile bool firing;

        public TriggerBot(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
            triggerBotHoldKey = new HotKey(configuration.Value.TriggerBotHoldKey);
            new Thread(CheckFire).Start();
        }

        public void OnTick()
        {
            if (!KeyUtility.IsKeyDown(triggerBotHoldKey.Value) || firing)
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

                    if (otherTeam != team)
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
                    Thread.Sleep(ThreadSafeRandom.Next(40, 80));
                    Clicker.TriggerLeftClick(ThreadSafeRandom.Next(10, 15));

                   firing = false;
                }

                Thread.Sleep(2);
            }
        }
    }
}
