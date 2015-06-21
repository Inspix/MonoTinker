using System;
using System.Threading;
using System.Globalization;
using Microsoft.Xna.Framework;
using MonoTinker.Code.Utils;

namespace MonoTinker
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            using (var game = new MonoTinker(1024,768))
                game.Run();

            /*Vector2 x = new Vector2(3,5);
            Vector2 y = new Vector2(-1,6);
            float magnitudeX = x.Magnitude();
            float magnitudeY = y.Magnitude();
            float dot = Mathf.Dot(x, y);

            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(magnitudeX);
            Console.WriteLine(Mathf.Angle(x,y));
            Console.WriteLine(MathHelper.Distance(x.X,x.Y));
            Console.WriteLine(magnitudeY);
            Console.WriteLine(dot);*/

        }
    }
#endif
}
