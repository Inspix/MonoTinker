using Microsoft.Xna.Framework;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.Extensions
{
    public static class TextBoxFactory
    {
        #region Generation
        public static void Top(int size, ref SpriteAtlas elements, ref int width, ref int height)
        {
            Sprite topLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopLeft).DirectClone();
            topLeft.Position = Vector2.Zero;
            width += topLeft.SourceWidth;
            height += topLeft.SourceHeight;
            elements.Add("topLeft", topLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite topMiddle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopMiddle).DirectClone();
                topMiddle.Position = Vector2.UnitX * width;
                width += topMiddle.SourceWidth;
                elements.Add("topMiddle" + i, topMiddle);
            }
            Sprite topRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopRight).DirectClone();
            topRight.Position = Vector2.UnitX * width;
            width += topRight.SourceWidth;
            elements.Add("topRight", topRight);
        }

        public static void Middle(int size, ref SpriteAtlas elements, ref int width, ref int height, int mod)
        {
            Sprite middleLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddleLeft).DirectClone();
            middleLeft.Position = Vector2.UnitY * height;
            height += middleLeft.SourceHeight;
            elements.Add("middleLeft" + mod, middleLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite middle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddle).DirectClone();
                middle.Position = middleLeft.Position + (Vector2.UnitX * middleLeft.SourceWidth * i);
                elements.Add("middle" + i + "_" + mod, middle);
            }
            Sprite middleRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddleRight).DirectClone();
            middleRight.Position = middleLeft.Position + (Vector2.UnitX * (width - middleRight.SourceWidth));
            elements.Add("middleRight" + mod, middleRight);
        }

        public static void Bottom(int size, ref SpriteAtlas elements, ref int width, ref int height)
        {
            Sprite bottomLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomLeft).DirectClone();
            bottomLeft.Position = Vector2.UnitY * height;
            height += bottomLeft.SourceHeight;
            elements.Add("bottomLeft", bottomLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite bottomMiddle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomMiddle).DirectClone();
                bottomMiddle.Position = bottomLeft.Position + (Vector2.UnitX * bottomMiddle.SourceWidth * i);
                elements.Add("bottomMiddle" + i, bottomMiddle);
            }
            Sprite bottomRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomRight).DirectClone();
            bottomRight.Position = bottomLeft.Position + (Vector2.UnitX * (width - bottomRight.SourceWidth));
            elements.Add("bottomRight", bottomRight);
        }

        public static void GenerateBox(Point size, ref SpriteAtlas elements, ref int width, ref int height)
        {
            Top(size.X,ref elements,ref width,ref height);
            for (int i = 0; i < size.Y; i++)
            {
                Middle(size.X, ref elements, ref width, ref height, i);
            }
            Bottom(size.X,ref elements,ref width,ref height);
        }

        #endregion
    }
}
