using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace MonoTinker.Code.Utils
{
    public static class RectangleHelper
    {
        public static Vector2 SizeVec2(this Rectangle rect)
        {
            return new Vector2(rect.Size.X,rect.Size.Y);
        }
    }
}
