using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class Aimbot : Component, ITickHandler
    {
        public override string Name => "Aimbot";

        public override HotKey DefaultHotkey => new HotKey('0');
        private HotKey AimBotHoldKey { get; }

        public Aimbot(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
            AimBotHoldKey = new HotKey(configuration.Value.AimbotHoldKey);
        }

        public void OnTick()
        {
            GlobalState.AimBotControllingRecoil = false;

            if (configuration.Value.UseHoldKeyForAimbot && !KeyUtility.IsKeyDown(AimBotHoldKey.Value))
                return;

            if (!WindowUtility.TryGetActiveWindowDimensions(out var dimensions))
                return;

            var midx = dimensions.X / 2;
            var midy = dimensions.Y / 2;

            var viewMatrix = Memory.Read<Matrix4x4>(processHandle, (int)clientAddress + Signatures.dwViewMatrix);
            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var localPlayerPos = Memory.Read<Vector3>(processHandle, localPlayer + Netvars.m_vecOrigin);
            var localPlayerEye = localPlayerPos + Memory.Read<Vector3>(processHandle, localPlayer + Netvars.m_vecViewOffset);
            var teamId = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);

            var inFov = new List<Vector3>();

            for (int i = 1; i <= 32; i++)
            {
                var entity = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + i * 0x10);
                var enemyTeamId = Memory.Read<int>(processHandle, entity + Netvars.m_iTeamNum);

                if (enemyTeamId == teamId)
                    continue;

                var enemyHealth = Memory.Read<int>(processHandle, entity + Netvars.m_iHealth);

                if (enemyHealth <= 0)
                    continue;

                if (Memory.Read<bool>(processHandle, entity + Signatures.m_bDormant))
                    continue;

                var enemyPos = PlayerUtility.ReadBone(processHandle, entity, 8);

                if (!VectorUtility.TryWorldToScreen(viewMatrix, (int)dimensions.X, (int)dimensions.Y, enemyPos, out var screen))
                    continue;

                if (Math.Abs(midx - screen.X) < configuration.Value.AimbotFOV && Math.Abs(midy - screen.Y) < configuration.Value.AimbotFOV)
                    inFov.Add(enemyPos);
            }

            if (inFov.Count <= 0)
                return;

            var closest = inFov.OrderBy(x => Vector3.Distance(localPlayerEye, x)).First();

            var clientState = Memory.Read<int>(processHandle, (int)engineAddress + Signatures.dwClientState);
            var currentViewAngles = Memory.Read<Vector3>(processHandle, clientState + Signatures.dwClientState_ViewAngles);
            var calculatedAngles = VectorUtility.ClampAngle(VectorUtility.VectorAngles((closest - localPlayerEye).Normalize()).NormalizeAngle());

            if (configuration.Value.RecoilControlOnAimbot)
            {
                var weaponIndex = Memory.Read<int>(processHandle, localPlayer + Netvars.m_hActiveWeapon) & 0xFFF;
                var weaponPtr = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + (weaponIndex - 0x1) * 0x10);
                var weaponId = Memory.Read<int>(processHandle, weaponPtr + Netvars.m_iItemDefinitionIndex);

                if (!Weapons.IsSingleShot(weaponId))
                {
                    GlobalState.AimBotControllingRecoil = true;
                    var punchAngle = Memory.Read<Vector3>(processHandle, localPlayer + Netvars.m_aimPunchAngle);
                    calculatedAngles = VectorUtility.ClampAngle(calculatedAngles - punchAngle * 2F);
                }
            }
            
            if (configuration.Value.SmoothAim)
            {
                var delta = VectorUtility.ClampAngle((currentViewAngles - calculatedAngles).NormalizeAngle());
                calculatedAngles = VectorUtility.ClampAngle(currentViewAngles - delta / configuration.Value.AimSmoothingFactor);
            }

            Memory.Write<Vector3>(processHandle, clientState + Signatures.dwClientState_ViewAngles, calculatedAngles);
        }
    }
}
