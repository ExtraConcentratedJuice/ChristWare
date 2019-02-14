using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.IO;

namespace ChristWare
{
    public class ChristWare
    {
        private readonly IntPtr processHandle;
        private readonly IntPtr clientAddress;
        private readonly ChristConfiguration configuration;

        public ChristWare(string processName)
        {
            if (!ProcessUtility.TryGetProcessHandle("csgo", out processHandle))
                throw new ArgumentException("No CSGO process found.");

            Console.WriteLine("Acquired process handle @ " + processHandle);

            if (!ProcessUtility.TryGetProcessModule("csgo", "client_panorama.dll", out clientAddress))
                throw new ArgumentException("No CSGO client panorama module found.");

            configuration = JsonConvert.DeserializeObject<ChristConfiguration>(File.ReadAllText("christconfig.json"));

            Console.WriteLine("Client module @ " + clientAddress);
        }

        public void Run()
        {
            while (true)
            {
                var manager = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwGlowObjectManager);
                var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
                var teamId = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);

                for (int i = 1; i <= 32; i++)
                {
                    var entity = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + i * 0x10);

                    if (entity != 0)
                    {
                        var health = Memory.Read<int>(processHandle, entity + Netvars.m_iHealth);
                        var glow = Memory.Read<int>(processHandle, entity + Netvars.m_iGlowIndex);
                        var entityTeamId = Memory.Read<int>(processHandle, entity + Netvars.m_iTeamNum);

                        var r = entityTeamId != teamId ? configuration.EnemyR : configuration.FriendlyR;
                        var g = entityTeamId != teamId ? configuration.EnemyG : configuration.FriendlyG;
                        var b = entityTeamId != teamId ? configuration.EnemyB : configuration.FriendlyB;

                        Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x4, r / 255); // R
                        Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x8, g / 255); // G
                        Memory.Write<float>(processHandle, manager + glow * 0x38 + 0xC, b / 255); // B
                        Memory.Write<float>(processHandle, manager + glow * 0x38 + 0x10, 1F); // Alpha
                        Memory.Write<int>(processHandle, manager + glow * 0x38 + 0x24, 1); // Toggle
                    }
                }

                Thread.Sleep(5);
            }
        }
    }
}
