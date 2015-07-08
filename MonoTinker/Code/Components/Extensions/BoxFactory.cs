using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.GameObjects;
using MonoTinker.Code.Components.UI;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;
using TextBox = MonoTinker.Code.Components.UI.TextBox;

namespace MonoTinker.Code.Components.Extensions
{
    public enum TextAlignment
    {
        Left,Right,Center
    }

    public static class BoxFactory
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

        public static Sprite BoxSprite(SpriteBatch spriteBatch, Vector2 size)
        {
            SpriteAtlas elements = new SpriteAtlas();
            int width = 0, height = 0;
            GenerateBox(size.ToPoint(),ref elements,ref width,ref height);

            RenderTarget2D resultTexture = new RenderTarget2D(spriteBatch.GraphicsDevice,width,height);
            spriteBatch.GraphicsDevice.SetRenderTarget(resultTexture);
            spriteBatch.GraphicsDevice.Clear(Color.FromNonPremultiplied(0,0,0,0));
            spriteBatch.Begin();
            foreach (var sprite in elements)
            {
                sprite.Value.Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            return new Sprite(resultTexture);
        }


        public static Sprite BoxSpriteWithText(SpriteBatch spriteBatch, string[] text, TextAlignment alignment = TextAlignment.Left, float textscale = 1,bool spaceforHeader = false,bool spaceforFooter = false,bool spaceontheleft = false,bool spaceontheright = false)
        {
            SpriteFont font = AssetManager.Instance.Get<SpriteFont>("Standart");
            Vector2 fontSize = font.MeasureString("A") * textscale;
            Vector2 boxSize = new Vector2((text.Select(s => s.Length).Max()*fontSize.X)/32, (text.Length*(fontSize.Y + 5))/32);
            boxSize.Y += spaceforFooter ? +1 : 0;
            boxSize.Y += spaceforHeader ? +1 : 0;
            boxSize.X += spaceontheleft ? +1 : 0;
            boxSize.X += spaceontheright ? +1 : 0;
            Sprite box = BoxSprite(ScreenManager.Batch, boxSize+Vector2.One);
            Vector2 textpos = Vector2.Zero;
            switch (alignment)
            {
                case TextAlignment.Left:
                    textpos = new Vector2(10 + (spaceontheleft ? box.SpriteCenter.X / boxSize.X : 0), 20);
                break;
                case TextAlignment.Right:
                    textpos = new Vector2(box.SourceWidth - 20
                        + (spaceontheleft ? box.SpriteCenter.X / boxSize.X : 0)
                        - (spaceontheright ? box.SpriteCenter.X / boxSize.X : 0), 20);
                    break;
                case TextAlignment.Center:
                    textpos = new Vector2(box.SpriteCenter.X 
                        +(spaceontheleft ? box.SpriteCenter.X/boxSize.X : 0)
                        -(spaceontheright ? box.SpriteCenter.X / boxSize.X : 0), 20);
                    break;
            }
            textpos.Y += spaceforHeader ? fontSize.Y + 15 : 0;
            List<Text> labels = new List<Text>();
            foreach (var s in text)
            {
                Text toAdd = new Text(font,textpos,s);
                toAdd.Scale = Vector2.One * textscale;
                if (alignment == TextAlignment.Center)
                {
                    toAdd.PosX -= toAdd.Size.X/2;
                }else if (alignment == TextAlignment.Right)
                {
                    toAdd.PosX -= toAdd.Size.X;
                }
                textpos.Y += fontSize.Y + 15f;
                labels.Add(toAdd);
            }

            RenderTarget2D target = new RenderTarget2D(spriteBatch.GraphicsDevice,box.SourceWidth,box.SourceHeight);

            spriteBatch.GraphicsDevice.SetRenderTarget(target);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            box.Draw(spriteBatch);
            foreach (var label in labels)
            {
                label.Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            return new Sprite(target);

        }

        public static TextBox CharacterInfoBox(Vector2 position,CharacterClass clClass = CharacterClass.Warrior)
        {
            string text = null;
            switch (clClass)
            {
                case CharacterClass.Warrior:
                    text = Txt.CharacterInfo.Warrior;
                    break;
                case CharacterClass.Archer:
                    text = Txt.CharacterInfo.Archer;
                    break;
                case CharacterClass.Wizard:
                    text = Txt.CharacterInfo.Wizard;
                    break;
            }
            TextBox result = new TextBox(position,ScreenManager.Device, text,new Vector2(6,3));
            result.SetImage(new Sprite(TextureMaker.ClassSplash(ScreenManager.Device,clClass)),Origin.TopCenter);
            return result;
        }



        #endregion
    }
}
