using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class BoxESP : Component//, IEntityHandler
    {
        public override string Name => "BoxESP";
        public override HotKey DefaultHotkey => new HotKey('/');

        public BoxESP(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration) 
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
        }

        /*
        public void HandleEntity(int entity)
        {
            if (!WindowUtility.TryGetActiveWindowDimensions(out var dimensions))
                return;

            var position = Memory.Read<Vector3>(processHandle, entity + Netvars.m_vecOrigin);
            var headPosition = PlayerUtility.ReadBone(processHandle, entity, 8);

            var viewMatrix = Memory.Read<Matrix4x4>(processHandle, (int)clientAddress + Signatures.dwViewMatrix);

            if (!VectorUtility.TryWorldToScreen(viewMatrix, (int)dimensions.X, (int)dimensions.Y, position, out var positionVec2) ||
                !VectorUtility.TryWorldToScreen(viewMatrix, (int)dimensions.X, (int)dimensions.Y, position, out var headPositionVec2))
                return;
            var height = (int)(positionVec2.Y - headPositionVec2.Y);
            var width = (int)(height / 2.1F);
            var x = (int)((positionVec2.X + headPositionVec2.X) / 2F - width / 2F);
            var y = (int)(headPositionVec2.Y - height / 10f);
            height = (int)(height * 1.1F);

            graphics.DrawRectangle(new Pen(Color.AliceBlue, 3), new Rectangle(x, y, width, height));
        }
        */
    }
}
