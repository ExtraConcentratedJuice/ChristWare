using ChristWare.Core;
using ChristWare.Core.Offsets;
using ChristWare.Core.Offsets.Signatures;
using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public static class Signatures
    {
        public static void Initialize(IntPtr processHandle, ProcessModule client, ProcessModule engine)
        {
            int read = 0;
            byte[] clientBytes = new byte[client.ModuleMemorySize];
            byte[] engineBytes = new byte[engine.ModuleMemorySize];
            Memory.ReadProcessMemory((int)processHandle, (int)client.BaseAddress, clientBytes, client.ModuleMemorySize, ref read);
            Memory.ReadProcessMemory((int)processHandle, (int)engine.BaseAddress, engineBytes, engine.ModuleMemorySize, ref read);

            Console.WriteLine("Scanning for offsets...");

            var fields = typeof(Signatures).GetFields(BindingFlags.Public | BindingFlags.Static);
            var total = fields.Count();
            var count = 0;
            object lockObj = new object();
            Parallel.ForEach(fields, field =>
            {
                var attribute = field.GetCustomAttribute<PatternAttribute>();
                if (attribute == null)
                    throw new Exception("No pattern defined for " + field.Name);

                byte[] scanned;
                ProcessModule mod;

                if (attribute.Module == client.ModuleName)
                {
                    scanned = clientBytes;
                    mod = client;
                }
                else if (attribute.Module == engine.ModuleName)
                {
                    scanned = engineBytes;
                    mod = engine;
                }
                else
                    throw new Exception("Module not found: " + attribute.Module);

                if (!Scanner.TryFind(scanned, attribute, out var offset))
                    throw new Exception("Pattern not found for " + field.Name);

                for (int i = 0; i < attribute.Offsets.Count; i++)
                    offset = BitConverter.ToInt32(scanned, offset + attribute.Offsets[i]) - (int)mod.BaseAddress;

                offset += attribute.Extra;

                if (!attribute.Relative)
                    offset += (int)mod.BaseAddress;

                field.SetValue(null, offset);
                
                lock (lockObj)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"{(int)(Interlocked.Increment(ref count) / (float)total * 100)}%");
                }
            });
            Console.WriteLine();
        }

        [Pattern("engine.dll", "A1 ? ? ? ? 33 D2 6A 00 6A 00 33 C9 89 B0", 0, true, new int[] { 1 })]
        public static readonly Int32 dwClientState;
        [Pattern("engine.dll", "8B 80 ? ? ? ? 40 C3", 0, false, new int[] { 2 })]
        public static readonly Int32 dwClientState_GetLocalPlayer;
        [Pattern("engine.dll", "80 BF ? ? ? ? ? 0F 84 ? ? ? ? 32 DB", 0, false, new int[] { 2 })]
        public static readonly Int32 dwClientState_IsHLTV;
        [Pattern("engine.dll", "05 ? ? ? ? C3 CC CC CC CC CC CC CC A1", 0, false, new int[] { 1 })]
        public static readonly Int32 dwClientState_Map;
        [Pattern("engine.dll", "05 ? ? ? ? C3 CC CC CC CC CC CC CC 80 3D", 0, false, new int[] { 1 })]
        public static readonly Int32 dwClientState_MapDirectory;
        [Pattern("engine.dll", "A1 ? ? ? ? 8B 80 ? ? ? ? C3 CC CC CC CC 55 8B EC 8A 45 08", 0, false, new int[] { 7 })]
        public static readonly Int32 dwClientState_MaxPlayer;
        [Pattern("engine.dll", "8B 89 ? ? ? ? 85 C9 0F 84 ? ? ? ? 8B 01", 0, false, new int[] { 2 })]
        public static readonly Int32 dwClientState_PlayerInfo;
        [Pattern("engine.dll", "83 B8 ? ? ? ? ? 0F 94 C0 C3", 0, false, new int[] { 2 })]
        public static readonly Int32 dwClientState_State;
        [Pattern("engine.dll", "F3 0F 11 80 ? ? ? ? D9 46 04 D9 05", 0, false, new int[] { 4 })]
        public static readonly Int32 dwClientState_ViewAngles;
        [Pattern("engine.dll", "C7 87 ? ? ? ? ? ? ? ? FF 15 ? ? ? ? 83 C4 08", 0, false, new int[] { 2 })]
        public static readonly Int32 clientstate_delta_ticks;
        [Pattern("engine.dll", "8B 8F ? ? ? ? 8B 87 ? ? ? ? 41", 0, false, new int[] { 2 })]
        public static readonly Int32 clientstate_last_outgoing_command;
        [Pattern("engine.dll", "8B 87 ? ? ? ? 41", 0, false, new int[] { 2 })]
        public static readonly Int32 clientstate_choked_commands;
        [Pattern("engine.dll", "8B 8F ? ? ? ? 8B 01 8B 40 18", 0, false, new int[] { 2 })]
        public static readonly Int32 clientstate_net_channel;
        [Pattern("client_panorama.dll", "BB ? ? ? ? 83 FF 01 0F 8C ? ? ? ? 3B F8", 0, true, new int[] { 1 })]
        public static readonly Int32 dwEntityList;
        [Pattern("client_panorama.dll", "89 0D ? ? ? ? 8B 0D ? ? ? ? 8B F2 8B C1 83 CE 04", 0, true, new int[] { 2 })]
        public static readonly Int32 dwForceAttack;
        [Pattern("client_panorama.dll", "89 0D ? ? ? ? 8B 0D ? ? ? ? 8B F2 8B C1 83 CE 04", 12, true, new int[] { 2 })]
        public static readonly Int32 dwForceAttack2;
        [Pattern("client_panorama.dll", "55 8B EC 51 53 8A 5D 08", 0, true, new int[] { 287 })]
        public static readonly Int32 dwForceBackward;
        [Pattern("client_panorama.dll", "55 8B EC 51 53 8A 5D 08", 0, true, new int[] { 245 })]
        public static readonly Int32 dwForceForward;
        [Pattern("client_panorama.dll", "8B 0D ? ? ? ? 8B D6 8B C1 83 CA 02", 0, true, new int[] { 2 })]
        public static readonly Int32 dwForceJump;
        [Pattern("client_panorama.dll", "55 8B EC 51 53 8A 5D 08", 0, true, new int[] { 465 })]
        public static readonly Int32 dwForceLeft;
        [Pattern("client_panorama.dll", "55 8B EC 51 53 8A 5D 08", 0, true, new int[] { 512 })]
        public static readonly Int32 dwForceRight;
        [Pattern("engine.dll", "68 ? ? ? ? 8D 85 ? ? ? ? 50 68 ? ? ? ? 68", 0, true, new int[] { 1 })]
        public static readonly Int32 dwGameDir;
        [Pattern("client_panorama.dll", "A1 ? ? ? ? 85 C0 0F 84 ? ? ? ? 80 B8 ? ? ? ? ? 0F 84 ? ? ? ? 0F 10 05", 0, true, new int[] { 1 })]
        public static readonly Int32 dwGameRulesProxy;
        [Pattern("client_panorama.dll", "A1 ? ? ? ? C3 CC CC CC CC CC CC CC CC CC CC A1 ? ? ? ? B9", 0, true, new int[] { 1, 0 })]
        public static readonly Int32 dwGetAllClasses;
        [Pattern("engine.dll", "68 ? ? ? ? 68 ? ? ? ? FF 50 08 85 C0", 0, true, new int[] { 1 })]
        public static readonly Int32 dwGlobalVars;
        [Pattern("client_panorama.dll", "A1 ? ? ? ? A8 01 75 4B", 4, true, new int[] { 1 })]
        public static readonly Int32 dwGlowObjectManager;
        [Pattern("client_panorama.dll", "B9 ? ? ? ? F3 0F 11 04 24 FF 50 10", 0, true, new int[] { 1 })]
        public static readonly Int32 dwInput;
        [Pattern("client_panorama.dll", "8B 35 ? ? ? ? 57 85 F6 74 ? 8B 7D 08 8B 4E 04 8B C7 8A 11 3A 10", 0, true, new int[] { })]
        public static readonly Int32 dwInterfaceLinkList;
        [Pattern("client_panorama.dll", "8D 34 85 ? ? ? ? 89 15 ? ? ? ? 8B 41 08 8B 48 04 83 F9 FF", 4, true, new int[] { 3 })]
        public static readonly Int32 dwLocalPlayer;
        [Pattern("client_panorama.dll", "B9 ? ? ? ? FF 50 34 85 C0 75 10", 48, true, new int[] { 1 })]
        public static readonly Int32 dwMouseEnable;
        [Pattern("client_panorama.dll", "B9 ? ? ? ? FF 50 34 85 C0 75 10", 0, true, new int[] { 1 })]
        public static readonly Int32 dwMouseEnablePtr;
        [Pattern("client_panorama.dll", "8B 3D ? ? ? ? 85 FF 0F 84 ? ? ? ? 81 C7", 0, true, new int[] { 2 })]
        public static readonly Int32 dwPlayerResource;
        [Pattern("client_panorama.dll", "A1 ? ? ? ? 8B 0C B0 8B 01 FF 50 ? 46 3B 35 ? ? ? ? 7C EA 8B 0D", 0, true, new int[] { 1 })]
        public static readonly Int32 dwRadarBase;
        [Pattern("client_panorama.dll", "81 F9 ? ? ? ? 75 1D F3 0F 10 05 ? ? ? ? F3 0F 11 44 24 ? 8B 44 24 18 35 ? ? ? ? 89 44 24 0C EB 0B", 44, true, new int[] { 2 })]
        public static readonly Int32 dwSensitivity;
        [Pattern("client_panorama.dll", "81 F9 ? ? ? ? 75 1D F3 0F 10 05 ? ? ? ? F3 0F 11 44 24 ? 8B 44 24 18 35 ? ? ? ? 89 44 24 0C EB 0B", 0, true, new int[] { 2 })]
        public static readonly Int32 dwSensitivityPtr;
        [Pattern("engine.dll", "53 56 57 8B DA 8B F9 FF 15", 0, true, new int[] { })]
        public static readonly Int32 dwSetClanTag;
        [Pattern("client_panorama.dll", "0F 10 05 ? ? ? ? 8D 85 ? ? ? ? B9", 176, true, new int[] { 3 })]
        public static readonly Int32 dwViewMatrix;
        [Pattern("client_panorama.dll", "B9 ? ? ? ? 6A 00 FF 50 08 C3", 0, true, new int[] { 1 })]
        public static readonly Int32 dwWeaponTable;
        [Pattern("client_panorama.dll", "39 86 ? ? ? ? 74 06 89 86 ? ? ? ? 8B 86", 0, false, new int[] { 2 })]
        public static readonly Int32 dwWeaponTableIndex;
        [Pattern("client_panorama.dll", "81 F9 ? ? ? ? 75 1D F3 0F 10 05 ? ? ? ? F3 0F 11 44 24 ? 8B 44 24 1C 35 ? ? ? ? 89 44 24 18 EB 0B 8B 01 8B 40 30 FF D0 D9 5C 24 18 F3 0F 10 06", 0, true, new int[] { 2 })]
        public static readonly Int32 dwYawPtr;
        [Pattern("client_panorama.dll", "81 F9 ? ? ? ? 75 1A F3 0F 10 05 ? ? ? ? F3 0F 11 45 ? 8B 45 F4 35 ? ? ? ? 89 45 FC EB 0A 8B 01 8B 40 30 FF D0 D9 5D FC A1", 0, true, new int[] { 2 })]
        public static readonly Int32 dwZoomSensitivityRatioPtr;
        [Pattern("engine.dll", "B3 01 8B 01 8B 40 10 FF D0 84 C0 74 0F 80 BF ? ? ? ? ? 0F 84", 1, true, new int[] { })]
        public static readonly Int32 dwbSendPackets;
        [Pattern("client_panorama.dll", "8B B6 ? ? ? ? 85 F6 74 05 83 3E 00 75 02 33 F6 F3 0F 10 44 24", 0, false, new int[] { 2 })]
        public static readonly Int32 m_pStudioHdr;
        [Pattern("client_panorama.dll", "81 F9 ? ? ? ? 75 16 F3 0F 10 05 ? ? ? ? F3 0F 11 45 ? 81 75 ? ? ? ? ? EB 0A 8B 01 8B 40 30 FF D0 D9 5D 0C 8B 55 08", 0, true, new int[] { 2 })]
        public static readonly Int32 m_yawClassPtr;
        [Pattern("client_panorama.dll", "A1 ? ? ? ? 89 74 24 28", 0, true, new int[] { 1 })]
        public static readonly Int32 m_pitchClassPtr;
        [Pattern("client_panorama.dll", "8A 81 ? ? ? ? C3 32 C0", 8, false, new int[] { 2 })]
        public static readonly Int32 m_bDormant;
        [Pattern("engine.dll", "F3 0F 10 0D ? ? ? ? F3 0F 11 4C 24 ? 8B 44 24 20 35 ? ? ? ? 89 44 24 0C", 0, true, new int[] { 4 })]
        public static readonly Int32 model_ambient_min;
        [Pattern("client_panorama.dll", "55 8B EC 83 E4 F8 83 EC 64 53 56 57 8B F1 E8", 0, true, new int[] { })]
        public static readonly Int32 set_abs_angles;
        [Pattern("client_panorama.dll", "55 8B EC 83 E4 F8 51 53 56 57 8B F1 E8", 0, true, new int[] { })]
        public static readonly Int32 set_abs_origin;
        [Pattern("client_panorama.dll", "56 8B F1 85 F6 74 31", 0, true, new int[] { })]
        public static readonly Int32 is_c4_owner;
        [Pattern("client_panorama.dll", "74 07 8B CB E8 ? ? ? ? 83 C7 10", 0, true, new int[] { })]
        public static readonly Int32 force_update_spectator_glow;
        [Pattern("client_panorama.dll", "44 54 5F 54 45 57 6F 72 6C 64 44 65 63 61 6C", 0, false, new int[] { })]
        public static readonly Int32 firstClass;
    }
}
