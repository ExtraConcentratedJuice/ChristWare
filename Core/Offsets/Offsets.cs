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
    public class ClientClass
    {
        IntPtr proc;
        private int baseAdr;

	    public ClientClass(IntPtr proc, int baseAdr)
        {
            this.baseAdr = baseAdr;
            this.proc = proc;
        }

        public int classId()
        {
            return Memory.Read<int>(proc, baseAdr + 0x14);
        }

        public String className()
        {
            return Memory.ReadString(proc, Memory.Read<int>(proc, baseAdr + 0x8), 64, System.Text.Encoding.Default);
        }

        public int next()
        {
            return Memory.Read<int>(proc, baseAdr + 0x10);
        }

        public int table()
        {
            return Memory.Read<int>(proc, baseAdr + 0xC);
        }

        public bool readable()
        {
            int red = 0;
            return Memory.ReadProcessMemory((int)proc, baseAdr, new byte[0x28], 0x28, ref red);
        }
    }

    public class RecvProp
    {
        private IntPtr proc;
        private int baseAdr;

	    public RecvProp(IntPtr proc, int baseAdr)
        {
            this.baseAdr = baseAdr;
            this.proc = proc;
        }

        private int procOffset;

        public RecvProp setOffset(int procOffset)
        {
            this.procOffset = procOffset;
            return this;
        }

        public int table()
        {
            return Memory.Read<int>(proc, baseAdr + 0x28);
        }

        public String name()
        {
            return Memory.ReadString(proc, Memory.Read<int>(proc, baseAdr), 64, System.Text.Encoding.Default);
        }

        public int offset()
        {
            return procOffset + Memory.Read<int>(proc, baseAdr + 0x2C);
        }

        public int type()
        {
            return Memory.Read<int>(proc, baseAdr + 0x4);
        }

        public int elements()
        {
            return Memory.Read<int>(proc, baseAdr + 0x34);
        }

        public int stringBufferCount()
        {
            return Memory.Read<int>(proc, baseAdr + 0xC);
        }

    }

    public class RecvTable
    {

        private int baseAdr;
        private IntPtr proc;

	public RecvTable(IntPtr proc, int baseAdr)
        {
            this.proc = proc;
            this.baseAdr = baseAdr;
        }

        public int propForId(int id)
        {
            return Memory.Read<int>(proc, baseAdr) + (id * 0x3C));
        }

        public String tableName()
        {
            return Memory.ReadString(proc, Memory.Read<int>(proc, baseAdr + 0xC), 32, System.Text.Encoding.Default);
        }

        public int propCount()
        {
            return Memory.Read<int>(proc, baseAdr + 0x4);
        }

        public bool readable()
        {
            int red = 0;
            return Memory.ReadProcessMemory((int)proc, baseAdr, new byte[0x10], 0x10, ref red);
        }


    }

    public static class Netvars
    {
        public static void Initialize(IntPtr processHandle, int clientAddress)
        {
            var tables = new Dictionary<(string, string), int>();
            var classTable = Memory.Read<int>(processHandle, 0x25FDFD05 + 0x2B);

            do
            {
                var tableAddr = Memory.Read<int>(processHandle, classTable + 0xC);

                if (tableAddr != 0)
                {
                    var strAddr = Memory.Read<int>(processHandle, tableAddr + 0xC);
                    var tableName = Memory.ReadString(processHandle, strAddr, 32, System.Text.Encoding.Default);
                    ReadTable(processHandle, tableAddr, 0, tableName, tables);
                }

                classTable = Memory.Read<int>(processHandle, classTable + 0x10);
            }
            while (classTable != 0);

            foreach (var field in typeof(Netvars).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var attribute = field.GetCustomAttribute<NetvarAttribute>();

                if (attribute == null)
                    continue;

                var fieldOffset = tables[(attribute.Table, attribute.Name)] + attribute.Offset;
                field.SetValue(null, fieldOffset);
                Console.WriteLine($"{field.Name} located @ {fieldOffset,'X'}");
            }
        }

        private static void ReadTable(IntPtr processHandle, int tableOffset, int propOffset, string tableName, Dictionary<(string, string), int> tables)
        {
            var propCount = Memory.Read<int>(processHandle, tableOffset + 0x4);

            for (var i = 0; i < propCount; i++)
            {
                var propBase = Memory.Read<int>(processHandle, tableOffset + i * 0x3C);
                var offset = Memory.Read<int>(processHandle, propBase + 0x2C) + propOffset;
                var name = Memory.ReadString(processHandle, Memory.Read<int>(processHandle, propBase), 64, System.Text.Encoding.Default);
                Console.WriteLine(name);
                Thread.Sleep(-1);

                if (char.IsDigit(name[0]))
                    continue;

                if (propOffset != 0)
                    tables[(tableName, name)] = offset;

                var child = Memory.Read<int>(processHandle, propBase + 0xC);

                if (child == 0)
                    continue;

                ReadTable(processHandle, child, offset, tableName, tables);
            }
        }

        [Netvar("DT_CSPlayer", "m_ArmorValue")]
        public static readonly Int32 m_ArmorValue;
        [Netvar("DT_BasePlayer", "m_Collision")]
        public static readonly Int32 m_Collision;
        [Netvar("DT_CSPlayer", "m_CollisionGroup")]
        public static readonly Int32 m_CollisionGroup;
        [Netvar("DT_BasePlayer", "m_Local")]
        public static readonly Int32 m_Local;
        [Netvar("DT_CSPlayer", "m_nRenderMode", 1)]
        public static readonly Int32 m_MoveType;
        [Netvar("DT_BaseAttributableItem", "m_OriginalOwnerXuidHigh")]
        public static readonly Int32 m_OriginalOwnerXuidHigh;
        [Netvar("DT_BaseAttributableItem", "m_OriginalOwnerXuidLow")]
        public static readonly Int32 m_OriginalOwnerXuidLow;
        [Netvar("DT_BasePlayer", "m_aimPunchAngle")]
        public static readonly Int32 m_aimPunchAngle;
        [Netvar("DT_BasePlayer", "m_aimPunchAngleVel")]
        public static readonly Int32 m_aimPunchAngleVel;
        [Netvar("DT_CSPlayer", "m_bGunGameImmunity")]
        public static readonly Int32 m_bGunGameImmunity;
        [Netvar("DT_CSPlayer", "m_bHasDefuser")]
        public static readonly Int32 m_bHasDefuser;
        [Netvar("DT_CSPlayer", "m_bHasHelmet")]
        public static readonly Int32 m_bHasHelmet;
        [Netvar("DT_BaseCombatWeapon", "m_flNextPrimaryAttack", 109)]
        public static readonly Int32 m_bInReload;
        [Netvar("DT_CSPlayer", "m_bIsDefusing")]
        public static readonly Int32 m_bIsDefusing;
        [Netvar("DT_CSPlayer", "m_bIsScoped")]
        public static readonly Int32 m_bIsScoped;
        [Netvar("DT_BaseEntity", "m_bSpotted")]
        public static readonly Int32 m_bSpotted;
        [Netvar("DT_BaseEntity", "m_bSpottedByMask")]
        public static readonly Int32 m_bSpottedByMask;
        [Netvar("DT_BaseAnimating", "m_nForceBone", 28)]
        public static readonly Int32 m_dwBoneMatrix;
        [Netvar("DT_WeaponCSBase", "m_fAccuracyPenalty")]
        public static readonly Int32 m_fAccuracyPenalty;
        [Netvar("DT_CSPlayer", "m_fFlags")]
        public static readonly Int32 m_fFlags;
        [Netvar("DT_BaseAttributableItem", "m_flFallbackWear")]
        public static readonly Int32 m_flFallbackWear;
        [Netvar("DT_CSPlayer", "m_flFlashDuration")]
        public static readonly Int32 m_flFlashDuration;
        [Netvar("DT_CSPlayer", "m_flFlashMaxAlpha")]
        public static readonly Int32 m_flFlashMaxAlpha;
        [Netvar("DT_BaseCombatWeapon", "m_flNextPrimaryAttack")]
        public static readonly Int32 m_flNextPrimaryAttack;
        [Netvar("DT_BasePlayer", "m_hActiveWeapon")]
        public static readonly Int32 m_hActiveWeapon;
        [Netvar("DT_BasePlayer", "m_hActiveWeapon", -256)]
        public static readonly Int32 m_hMyWeapons;
        [Netvar("DT_BasePlayer", "m_hObserverTarget")]
        public static readonly Int32 m_hObserverTarget;
        [Netvar("DT_PredictedViewModel", "m_hOwner")]
        public static readonly Int32 m_hOwner;
        [Netvar("DT_CSPlayer", "m_hOwnerEntity")]
        public static readonly Int32 m_hOwnerEntity;
        [Netvar("DT_BaseAttributableItem", "m_iAccountID")]
        public static readonly Int32 m_iAccountID;
        [Netvar("DT_BaseCombatWeapon", "m_iClip1")]
        public static readonly Int32 m_iClip1;
        [Netvar("DT_CSPlayerResource", "m_iCompetitiveRanking")]
        public static readonly Int32 m_iCompetitiveRanking;
        [Netvar("DT_CSPlayerResource", "m_iCompetitiveWins")]
        public static readonly Int32 m_iCompetitiveWins;
        [Netvar("DT_CSPlayer", "m_bHasDefuser", 92)]
        public static readonly Int32 m_iCrosshairId;
        [Netvar("DT_BaseAttributableItem", "m_iEntityQuality")]
        public static readonly Int32 m_iEntityQuality;
        [Netvar("DT_CSPlayer", "m_iFOVStart")]
        public static readonly Int32 m_iFOVStart;
        [Netvar("DT_CSPlayer", "m_iFOV")]
        public static readonly Int32 m_iFOV;
        [Netvar("DT_CSPlayer", "m_flFlashDuration", 24)]
        public static readonly Int32 m_iGlowIndex;
        [Netvar("DT_BasePlayer", "m_iHealth")]
        public static readonly Int32 m_iHealth;
        [Netvar("DT_BaseCombatWeapon", "m_iItemDefinitionIndex")]
        public static readonly Int32 m_iItemDefinitionIndex;
        [Netvar("DT_BaseAttributableItem", "m_iItemIDHigh")]
        public static readonly Int32 m_iItemIDHigh;
        [Netvar("DT_BasePlayer", "m_iObserverMode")]
        public static readonly Int32 m_iObserverMode;
        [Netvar("DT_CSPlayer", "m_iShotsFired")]
        public static readonly Int32 m_iShotsFired;
        [Netvar("DT_BaseCombatWeapon", "m_iState")]
        public static readonly Int32 m_iState;
        [Netvar("DT_BasePlayer", "m_iTeamNum")]
        public static readonly Int32 m_iTeamNum;
        [Netvar("DT_CSPlayer", "m_lifeState")]
        public static readonly Int32 m_lifeState;
        [Netvar("DT_BaseAttributableItem", "m_nFallbackPaintKit")]
        public static readonly Int32 m_nFallbackPaintKit;
        [Netvar("DT_BaseAttributableItem", "m_nFallbackSeed")]
        public static readonly Int32 m_nFallbackSeed;
        [Netvar("DT_BaseAttributableItem", "m_nFallbackStatTrak")]
        public static readonly Int32 m_nFallbackStatTrak;
        [Netvar("DT_BaseAnimating", "m_nForceBone")]
        public static readonly Int32 m_nForceBone;
        [Netvar("DT_BasePlayer", "m_nTickBase")]
        public static readonly Int32 m_nTickBase;
        [Netvar("DT_CSPlayer", "m_CollisionGroup", -48)]
        public static readonly Int32 m_rgflCoordinateFrame;
        [Netvar("DT_BaseAttributableItem", "m_szCustomName")]
        public static readonly Int32 m_szCustomName;
        [Netvar("DT_CSPlayer", "m_szLastPlaceName")]
        public static readonly Int32 m_szLastPlaceName;
        [Netvar("DT_BasePlayer", "m_vecOrigin")]
        public static readonly Int32 m_vecOrigin;
        [Netvar("DT_CSPlayer", "m_vecVelocity[0]")]
        public static readonly Int32 m_vecVelocity;
        [Netvar("DT_CSPlayer", "m_vecViewOffset[0]")]
        public static readonly Int32 m_vecViewOffset;
        [Netvar("DT_BasePlayer", "m_viewPunchAngle")]
        public static readonly Int32 m_viewPunchAngle;
        [Netvar("DT_CSPlayer", "deadflag", 4)]
        public static readonly Int32 m_thirdPersonViewAngles;
        [Netvar("DT_BaseEntity", "m_clrRender")]
        public static readonly Int32 m_clrRender;
        [Netvar("DT_PlantedC4", "m_flC4Blow")]
        public static readonly Int32 m_flC4Blow;
        [Netvar("DT_PlantedC4", "m_flTimerLength")]
        public static readonly Int32 m_flTimerLength;
        [Netvar("DT_PlantedC4", "m_flDefuseLength")]
        public static readonly Int32 m_flDefuseLength;
        [Netvar("DT_PlantedC4", "m_flDefuseCountDown")]
        public static readonly Int32 m_flDefuseCountDown;
        [Netvar("DT_CSGameRulesProxy", "cs_gamerules_data")]
        public static readonly Int32 cs_gamerules_data;
        [Netvar("DT_CSGameRulesProxy", "m_SurvivalRules")]
        public static readonly Int32 m_SurvivalRules;
        [Netvar("DT_CSGameRulesProxy", "m_SurvivalGameRuleDecisionTypes")]
        public static readonly Int32 m_SurvivalGameRuleDecisionTypes;
        [Netvar("DT_CSGameRulesProxy", "m_bIsValveDS")]
        public static readonly Int32 m_bIsValveDS;
        [Netvar("DT_CSGameRulesProxy", "m_bFreezePeriod")]
        public static readonly Int32 m_bFreezePeriod;
        [Netvar("DT_CSGameRulesProxy", "m_bBombPlanted")]
        public static readonly Int32 m_bBombPlanted;
        [Netvar("DT_CSGameRulesProxy", "m_bIsQueuedMatchmaking")]
        public static readonly Int32 m_bIsQueuedMatchmaking;
        [Netvar("DT_CSPlayer", "m_flSimulationTime")]
        public static readonly Int32 m_flSimulationTime;
        [Netvar("DT_CSPlayer", "m_flLowerBodyYawTarget")]
        public static readonly Int32 m_flLowerBodyYawTarget;
        [Netvar("DT_CSPlayer", "m_angEyeAngles[0]")]
        public static readonly Int32 m_angEyeAnglesX;
        [Netvar("DT_CSPlayer", "m_angEyeAngles[1]")]
        public static readonly Int32 m_angEyeAnglesY;
        [Netvar("DT_CSPlayer", "m_flNextAttack")]
        public static readonly Int32 m_flNextAttack;
        [Netvar("DT_CSPlayer", "m_nForceBone", 4)]
        public static readonly Int32 m_iMostRecentModelBoneCounter;
        [Netvar("DT_BaseAnimating", "m_nSequence", 104)]
        public static readonly Int32 m_flLastBoneSetupTime;
    }
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
