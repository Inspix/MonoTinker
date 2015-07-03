using MonoTinker.Code.Components.GameComponents;

namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;

    using Elements;
    using Managers;
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ToolTip : InterfaceElement
    {
        private SpriteFont font;
        private Vector2 charSize;
        private float spacing;

        public ToolTip(Vector2 position, GraphicsDevice device,Item item)
            : base(position, device)
        {
            this.InitToolTip(item);
        }
        
        private void InitToolTip(Item item)
        {
            this.font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            this.charSize = this.font.MeasureString("a");
            Vector2 sizeCalc = new Vector2(
               item.Name.Length * charSize.X,
               item.StatCount * charSize.Y);

            Point size = new Point(
                (int)Math.Floor(sizeCalc.X / AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddle).DefaultSource.Size.X),
                (int)Math.Floor(sizeCalc.Y / AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddle).DefaultSource.Size.Y));
            this.Top(size.X);
            for (int i = 0; i < size.Y; i++)
            {
                this.Middle(size.X, i);
            }
            this.Bottom(size.X);
            spacing = (Height+10)/(item.StatCount*charSize.Y) + charSize.Y;

            this.ToolTipGenerate(item);
            this.RenderTarget2D = new RenderTarget2D(this.Device, this.Width, this.Height);
        }

        private void ToolTipGenerate(Item item)
        {
            this.Labels.Clear();
            Text name = new Text(font, Vector2.One*10, item.Name);
            name.Clr = item.RarityColor();
            Text str = new Text(font, Vector2.One*10 + Vector2.UnitY*spacing*1, "Strenght: " + item.Status.Strenght);
            Text agi = new Text(font, Vector2.One*10 + Vector2.UnitY*spacing*2, "Agility: " + item.Status.Agility);
            Text vit = new Text(font, Vector2.One*10 + Vector2.UnitY*spacing*3, "Vitality: " + item.Status.Vitality);
            Text intel = new Text(font, Vector2.One*10 + Vector2.UnitY*spacing*4, "Intellect: " + item.Status.Intellect);
            Text wis = new Text(font, Vector2.One*10 + Vector2.UnitY*spacing*5, "Wisdom: " + item.Status.Wisdom);
            this.Labels.Add(name);
            this.Labels.Add(str);
            this.Labels.Add(agi);
            this.Labels.Add(vit);
            this.Labels.Add(intel);
            this.Labels.Add(wis);
        }

        public void DrawFromInventory(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            DrawToCurrentRenderTarget(spriteBatch);
            Draw(spriteBatch);
        }

        public void DrawAtPosition(SpriteBatch spriteBatch, Vector2 position)
        {
            if (!IsVisible) return;
            DrawElements();
            spriteBatch.Draw(this.RenderTarget2D,this.Position + position,this.RenderTarget2D.Bounds,Color.White * this.alpha,this.Rotation,Vector2.Zero,this.Scale,SpriteEffects.None, 0);
        }

        #region Generation
        private void Top(int size)
        {
            Sprite topLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopLeft).DirectClone();
            topLeft.Position = Vector2.Zero;
            Width += topLeft.SourceWidth;
            Height += topLeft.SourceHeight;
            this.Elements.Add("topLeft", topLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite topMiddle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopMiddle).DirectClone();
                topMiddle.Position = Vector2.UnitX * this.Width;
                Width += topMiddle.SourceWidth;
                this.Elements.Add("topMiddle" + i, topMiddle);
            }
            Sprite topRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopRight).DirectClone();
            topRight.Position = Vector2.UnitX * Width;
            Width += topRight.SourceWidth;
            this.Elements.Add("topRight", topRight);
        }

        private void Middle(int size, int mod)
        {
            Sprite middleLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddleLeft).DirectClone();
            middleLeft.Position = Vector2.UnitY * Height;
            Height += middleLeft.SourceHeight;
            this.Elements.Add("middleLeft" + mod, middleLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite middle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddle).DirectClone();
                middle.Position = middleLeft.Position + (Vector2.UnitX * middleLeft.SourceWidth * i);
                this.Elements.Add("middle" + i + "_" + mod, middle);
            }
            Sprite middleRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddleRight).DirectClone();
            middleRight.Position = middleLeft.Position + (Vector2.UnitX * (Width - middleRight.SourceWidth));
            this.Elements.Add("middleRight" + mod, middleRight);
        }

        private void Bottom(int size)
        {
            Sprite bottomLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomLeft).DirectClone();
            bottomLeft.Position = Vector2.UnitY * Height;
            Height += bottomLeft.SourceHeight;
            this.Elements.Add("bottomLeft", bottomLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite bottomMiddle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomMiddle).DirectClone();
                bottomMiddle.Position = bottomLeft.Position + (Vector2.UnitX * bottomMiddle.SourceWidth * i);
                this.Elements.Add("bottomMiddle" + i, bottomMiddle);
            }
            Sprite bottomRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomRight).DirectClone();
            bottomRight.Position = bottomLeft.Position + (Vector2.UnitX * (Width - bottomRight.SourceWidth));
            this.Elements.Add("bottomRight", bottomRight);
        }
        #endregion
    }
}
