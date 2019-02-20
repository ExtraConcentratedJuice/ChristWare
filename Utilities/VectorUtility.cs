using ChristWare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Utilities
{
    public static class VectorUtility
    {
        public static bool TryWorldToScreen(Matrix4x4 viewMatrix, int screenX, int screenY, Vector3 from, out Vector2 to)
        {
            to = default;

            to.X = viewMatrix.M11 * from.X + viewMatrix.M12 * from.Y + viewMatrix.M13 * from.Z + viewMatrix.M14;
            to.Y = viewMatrix.M21 * from.X + viewMatrix.M22 * from.Y + viewMatrix.M23 * from.Z + viewMatrix.M24;

            var w = viewMatrix.M41 * from.X + viewMatrix.M42 * from.Y + viewMatrix.M43 * from.Z + viewMatrix.M44;

            if (w < 0.01F)
                return false;

            var invw = 1.0F / w;

            to.X *= invw;
            to.Y *= invw;

            var x = screenX / 2F;
            var y = screenY / 2F;

            x += 0.5F * to.X * screenX + 0.5F;
            y -= 0.5F * to.Y * screenY + 0.5F;

            to.X = x;
            to.Y = y;

            return true;
        }

        public static Vector3 AngleToDirection(this Vector3 angle)
        {
            var x = angle.X * (Math.PI / 180F);
            var y = angle.Y * (Math.PI / 180F);

            var sinYaw = (float)Math.Sin(y);
            var cosYaw = (float)Math.Cos(y);

            var sinPitch = (float)Math.Sin(x);
            var cosPitch = (float)Math.Cos(x);

            return new Vector3(cosPitch * cosYaw, cosPitch * sinYaw, -sinPitch);
        }

        public static Vector3 ClampAngle(Vector3 angle)
        {
            Vector3 ret = new Vector3
            {
                Z = 0
            };

            if (angle.X > 89.0f && angle.X<= 180.0f)
                ret.X = 88.0f;

            if (angle.X > 180.0f)
                ret.X = angle.X - 360.0f;

            if (angle.X < -89.0f)
                ret.X = -88.0f;

            if (angle.Y > 180.0f)
                ret.Y = angle.Y - 360.0f;

            if (angle.Y < -180.0f)
                ret.Y = angle.Y + 360.0f;

            return ret;
        }
    }
}
