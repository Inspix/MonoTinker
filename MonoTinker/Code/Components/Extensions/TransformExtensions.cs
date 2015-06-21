using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoTinker.Code.Components.Elements;

namespace MonoTinker.Code.Components.Extensions
{
    public static class TransformExtensions
    {
        public static void Scale(this Transform tr, float increment)
        {
            tr.Scale += Vector2.One*increment;
        }
    }
}
