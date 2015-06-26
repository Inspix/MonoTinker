
namespace MonoTinker.Code.Utils
{
    using Microsoft.Xna.Framework;

    public static class ColorHelper
    {
        public static Color SmoothTransition(Color c1, Color c2, byte step)
        {
            if (c1 == c2)
            {
                return c1;
            }
            c1.A = c1.A > c2.A
                ? c1.A - step < c2.A
                ? c2.A : (byte)(c1.A - step) : c1.A + step > c2.A
                ? c2.A : (byte)(c1.A + step);
            c1.R = c1.R > c2.R
                ? c1.R - step < c2.R
                ? c2.R : (byte)(c1.R - step) : c1.R + step > c2.R
                ? c2.R : (byte)(c1.R + step);
            c1.G = c1.G > c2.G
                ? c1.G - step < c2.G
                ? c2.G : (byte)(c1.G - step) : c1.G + step > c2.G
                ? c2.G : (byte)(c1.G + step);
            c1.B = c1.B > c2.B
                ? c1.B - step < c2.B
                ? c2.B : (byte)(c1.B - step) : c1.B + step > c2.B
                ? c2.B : (byte)(c1.B + step);

            return c1;
        }

        public static Color SmoothTransition(Color c1, Color c2, float step)
        {
            if (c1 == c2)
            {
                return c1;
            }

            byte step1 = (byte)(MathHelper.Clamp(step, 0.004f, 1f) * 254);
            return SmoothTransition(c1, c2, step1);
        }
        
    }
}
