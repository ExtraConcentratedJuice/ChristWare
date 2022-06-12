using ChristWare.Core.Offsets;
using ChristWare.Core.Offsets.Netvars;
using ChristWare.Core.Offsets.Signatures;
using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public static class Netvars
    {
        // Thank you to my Java brothers
        // https://github.com/Jonatino/CSGO-Offset-Scanner/tree/master/src/main/java/com/github/jonatino/netvars

        public static void Initialize(IntPtr processHandle, ProcessModule clientModule)
        {
            Console.WriteLine("Scanning for netvars...");
            var netvars = new Dictionary<(string, string), int>();

            int read = 0;
            byte[] clientBytes = new byte[clientModule.ModuleMemorySize];
            Memory.ReadProcessMemory((int)processHandle, (int)clientModule.BaseAddress, clientBytes, clientModule.ModuleMemorySize, ref read);

            if (!Scanner.TryFind(clientBytes, BitConverter.GetBytes(Signatures.firstClass), new bool[4], out int ctOffset))
                throw new Exception("Unable to find class table offset");

            var clientClass = new ClientClass(processHandle, Memory.Read<int>(processHandle, ctOffset + (int)clientModule.BaseAddress + 0x2B));

            while (clientClass.Readable)
            {
                var table = new RecvTable(processHandle, clientClass.Table);
                var tableName = table.TableName;

                if (tableName.Length > 0 && table.PropCount > 0)
                    ReadTable(processHandle, table, 0, tableName, netvars);

                clientClass.BaseAddress = clientClass.Next;
            }

            foreach (var field in typeof(Netvars).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var attribute = field.GetCustomAttribute<NetvarAttribute>();

                if (attribute == null)
                    continue;

                var fieldOffset = netvars[(attribute.Table, attribute.Name)] + attribute.Offset;
                field.SetValue(null, fieldOffset);
            }
        }

        private static void ReadTable(IntPtr processHandle, RecvTable table, int offset, String className, Dictionary<(string, string), int> netvars)
        {
            for (int i = 0; i < table.PropCount; i++)
            {
                var prop = new RecvProp(processHandle, table.GetPropById(i))
                {
                    ProcOffset = offset
                };

                var propName = prop.Name;
                var propOffset = prop.Offset;

                if (Char.IsDigit(propName[0]))
                    continue;

                if (propOffset != 0)
                    netvars[(className, propName)] = propOffset;

                var child = prop.Table;

                if (child == 0)
                    continue;

                ReadTable(processHandle, new RecvTable(processHandle, child), propOffset, className, netvars);
            }
        }

        [Netvar("DT_CSPlayer", "m_iAccount")]
        public static readonly Int32 m_iAccount;
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
        // Netvar scanner won't locate this netvar for whatever reason
        //[Netvar("DT_CSGameRulesProxy", "cs_gamerules_data")]
        //public static readonly Int32 cs_gamerules_data;
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
}
