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
        private static readonly float radianConversion = (float)(180F / Math.PI);

        public static Vector3 NormalizeAngle(this Vector3 angle)
        {
            while (angle.X > 89.0F)
                angle.X -= 180F;

            while (angle.X < -89.0F)
                angle.X += 180F;

            while (angle.Y > 180F)
                angle.Y -= 360F;

            while (angle.Y < -180F)
                angle.Y += 360F;

            return angle;
        }

        public static Vector3 Normalize(this Vector3 vector)
        {
            var radius = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
            var iradius = 1F / (radius + float.Epsilon);

            return new Vector3(vector.X * iradius, vector.Y * iradius, vector.Z * iradius);
        }

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
            if (angle.Y > 180.0F)
                angle.Y = 180.0F;
            else if (angle.Y < -180.0F)
                angle.Y = -180.0F;

            if (angle.X > 89.0F)
                angle.X = 89.0F;
            else if (angle.X < -89.0F)
                angle.X = -89.0F;

            angle.Z = 0;

            return angle;
        }

        public static Vector3 VectorAngles(Vector3 forwardVector)
        {
            float pitch;
            float yaw;

            if (forwardVector.X == 0 && forwardVector.Y == 0)
            {
                yaw = 0;
                pitch = forwardVector.Z > 0 ? 270 : 90;
            }
            else
            {
                yaw = (float)(Math.Atan2(forwardVector.Y, forwardVector.X) * 180 / Math.PI);

                if (yaw < 0)
                    yaw += 360;

                var hyp = Math.Sqrt(forwardVector.X * forwardVector.X + forwardVector.Y * forwardVector.Y);
                pitch = (float)(Math.Atan2(-forwardVector.Z, hyp) * 180 / Math.PI);

                if (pitch < 0)
                    pitch += 360;
            }

            return new Vector3(pitch, yaw, 0);
        }
    }
}
