namespace MonoTinker.Code.Utils
{
    using System;
    using Microsoft.Xna.Framework;

    public static class Mathf
    {
        public static bool Approximately(this float f, float f2)
        {
            if (Math.Abs(f - f2) <= 0.001f)
            {
                return true;
            }
            return false;
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X*v2.X + v1.Y*v2.Y;
        }
        
        
        public static float Magnitude(this Vector2 v1)
        {
            return (float) Math.Sqrt(v1.X*v1.X + v1.Y*v1.Y);
        }

        public static float Angle(Vector2 veca, Vector2 vecb)
        {
            float dot = Dot(veca, vecb);
            if (dot.Approximately(0))
            {
                return 0;
            }
            float theta = dot/(Magnitude(veca)*Magnitude(vecb));
            float angle = (float)Math.Acos(theta);
            return MathHelper.ToDegrees(angle);
        }
    }
}
