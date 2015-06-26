namespace MonoTinker.Code.Components.Extensions
{
    using Microsoft.Xna.Framework;

    using Components.Elements;

    public static class TransformExtensions
    {
        public static void Scale(this Transform tr, float increment)
        {
            tr.Scale += Vector2.One*increment;
        }
    }
}
