
using System.Runtime.Remoting.Messaging;

namespace MonoTinker.Code.Utils
{
    using Microsoft.Xna.Framework;

    public static class ColorHelper
    {
        public static Color[] SkinTones()
        {
            Color[] colors = new Color[16];

            colors[0] =  new Color(255, 255, 255);
            colors[1] =  new Color(255, 229, 200);
            colors[2] =  new Color(255, 218, 190);
            colors[3] =  new Color(255, 206, 180);
            colors[4] =  new Color(255, 195, 170);
            colors[5] =  new Color(240, 184, 160);
            colors[6] =  new Color(225, 172, 150);
            colors[7] =  new Color(210, 161, 140);
            colors[8] =  new Color(195, 149, 130);
            colors[9] =  new Color(180, 138, 120);
            colors[10] =  new Color(165, 126, 110);
            colors[11] = new Color(150, 114, 100);
            colors[12] = new Color(135, 114, 90);
            colors[13] = new Color(120, 92, 80);
            colors[14] = new Color(105, 80, 70);
            colors[15] = new Color(90, 69, 60);
            return colors;
        }

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

        public static Color Fade(Color clr, int amount)
        {
            if (amount >= 255)
            {
                clr.A = 255;
                clr.R = 255;
                clr.G = 255;
                clr.B = 255;

            }
            else if (amount <= 0)
            {
                clr.A = 0;
                clr.R = 0;
                clr.G = 0;
                clr.B = 0;
            }
            else
            {
                clr.A = (byte)amount;
                clr.R = (byte)amount;
                clr.G = (byte)amount;
                clr.B = (byte)amount;
            }
            return clr;
        }

        public static Color Saturation(Color clr, float amount)
        {
            amount = MathHelper.Clamp(amount, 0.25f, 1f);
            clr.R = (byte) (clr.R*amount);
            clr.G = (byte) (clr.G*amount);
            clr.B = (byte) (clr.B*amount);
            return clr;
        }
        
    }
}
