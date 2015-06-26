using System;
using System.Threading;
using System.Globalization;
using Microsoft.Xna.Framework;
using MonoTinker.Code.Utils;

namespace MonoTinker
{
    using System.Text;

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
           
        }
    }
#endif
}
