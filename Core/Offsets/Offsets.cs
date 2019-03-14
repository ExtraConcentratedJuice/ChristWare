using ChristWare.Core;
using ChristWare.Core.Offsets;
using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

/*
 * public static void InitializeSignatures(byte[] client, byte[] engine)
        {
            var fields = typeof(Signatures).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (!(field.GetCustomAttributes(typeof(PatternAttribute), false).FirstOrDefault() is PatternAttribute pattern))
                    throw new Exception("No pattern defined for " + field.Name);

                if (!ScannerUtility.TryFind(client, pattern, out var offset))
                    throw new Exception("Pattern not found for " + field.Name);

                field.SetValue(null, offset);
            }
        }
        */
// 2019-02-14 04:27:21.134356100 UTC

namespace ChristWare
{
    public static class Signatures
    {
        public const Int32 clientstate_choked_commands = 0x4D28;
        public const Int32 clientstate_delta_ticks = 0x174;
        public const Int32 clientstate_last_outgoing_command = 0x4D24;
        public const Int32 clientstate_net_channel = 0x9C;
        public const Int32 convar_name_hash_table = 0x2F0F8;
        public const Int32 dwClientState = 0x58BCFC;
        public const Int32 dwClientState_GetLocalPlayer = 0x180;
        public const Int32 dwClientState_IsHLTV = 0x4D40;
        public const Int32 dwClientState_Map = 0x28C;
        public const Int32 dwClientState_MapDirectory = 0x188;
        public const Int32 dwClientState_MaxPlayer = 0x388;
        public const Int32 dwClientState_PlayerInfo = 0x52B8;
        public const Int32 dwClientState_State = 0x108;
        public const Int32 dwClientState_ViewAngles = 0x4D88;
        public const Int32 dwEntityList = 0x4CE34FC;
        public const Int32 dwForceAttack = 0x3114BC4;
        public const Int32 dwForceAttack2 = 0x3114BD0;
        public const Int32 dwForceBackward = 0x3114C18;
        public const Int32 dwForceForward = 0x3114BF4;
        public const Int32 dwForceJump = 0x5186998;
        public const Int32 dwForceLeft = 0x3114C0C;
        public const Int32 dwForceRight = 0x3114C30;
        public const Int32 dwGameDir = 0x631F70;
        public const Int32 dwGameRulesProxy = 0x51F8CD4;
        public const Int32 dwGetAllClasses = 0xCF6BC4;
        public const Int32 dwGlobalVars = 0x58BA00;
        public const Int32 dwGlowObjectManager = 0x5223740;
        public const Int32 dwInput = 0x512E510;
        public const Int32 dwInterfaceLinkList = 0x8AFF44;
        public const Int32 dwLocalPlayer = 0xCD2764;
        public const Int32 dwMouseEnable = 0xCD82B0;
        public const Int32 dwMouseEnablePtr = 0xCD8280;
        public const Int32 dwPlayerResource = 0x3112F5C;
        public const Int32 dwRadarBase = 0x511823C;
        public const Int32 dwSensitivity = 0xCD814C;
        public const Int32 dwSensitivityPtr = 0xCD8120;
        public const Int32 dwSetClanTag = 0x896A0;
        public const Int32 dwViewMatrix = 0x4CD4F14;
        public const Int32 dwWeaponTable = 0x512EFD8;
        public const Int32 dwWeaponTableIndex = 0x323C;
        public const Int32 dwYawPtr = 0xCD7F10;
        public const Int32 dwZoomSensitivityRatioPtr = 0xCDD150;
        public const Int32 dwbSendPackets = 0xD243A;
        public const Int32 dwppDirect3DDevice9 = 0xA6020;
        public const Int32 force_update_spectator_glow = 0x38DE82;
        public const Int32 interface_engine_cvar = 0x3E9EC;
        public const Int32 is_c4_owner = 0x399F60;
        public const Int32 m_bDormant = 0xED;
        public const Int32 m_pStudioHdr = 0x294C;
        public const Int32 m_pitchClassPtr = 0x51184F0;
        public const Int32 m_yawClassPtr = 0xCD7F10;
        public const Int32 model_ambient_min = 0x58ED1C;
        public const Int32 set_abs_angles = 0x1C7C70;
        public const Int32 set_abs_origin = 0x1C7AB0;
    }
}
