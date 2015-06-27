namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;

    using Managers;
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ToolTip : InterfaceElement
    {
        private string[] lines;

        private SpriteFont font;

        private Vector2 charSize;

        public ToolTip(Vector2 position, GraphicsDevice device,string[] lines)
            : base(position, device)
        {
            this.lines = lines;
            this.InitToolTip();
        }

        public Vector2 Position
        {
            get
            {
                return this.Transform.Position;
            }
            set
            {
                this.Transform.Position = value;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(this.Width,this.Height);
            }
        }

        private void InitToolTip()
        {
            this.font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            this.charSize = this.font.MeasureString("a");
            Vector2 sizeCalc = new Vector2(
                lines.Select(s => s.Length).Max() * this.charSize.X,
                lines.Length * this.charSize.Y);

            Point size = new Point(
                (int)Math.Floor(sizeCalc.X / AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddle).DefaultSource.Size.X),
                (int)Math.Floor(sizeCalc.Y / AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddle).DefaultSource.Size.Y));
            this.Top(size.X);
            for (int i = 0; i < size.Y; i++)
            {
                this.Middle(size.X, i);
            }
            this.Bottom(size.X);

            this.ToolTipGenerate();
            this.RenderTarget2D = new RenderTarget2D(this.Device, this.Width, this.Height);
        }

        private void ToolTipGenerate()
        {
            this.Labels.Clear();
            string output = String.Join("\n", this.lines);
            Text x = new Text(this.font, Vector2.One * 10, output);
            x.FadeSpeed = 1;
            this.Labels.Add(x);
        }

        #region Generation
        private void Top(int size)
        {
            Sprite topLeft = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameTopLeft).Clone() as Sprite;
            topLeft.Position = Vector2.Zero;
            Width += topLeft.SourceWidth;
            Height += topLeft.SourceHeight;
            this.Elements.Add("topLeft", topLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite topMiddle = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameTopMiddle).Clone() as Sprite;
                topMiddle.Position = Vector2.UnitX * this.Width;
                Width += topMiddle.SourceWidth;
                this.Elements.Add("topMiddle" + i, topMiddle);
            }
            Sprite topRight = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameTopRight).Clone() as Sprite;
            topRight.Position = Vector2.UnitX * Width;
            Width += topRight.SourceWidth;
            this.Elements.Add("topRight", topRight);
        }

        private void Middle(int size, int mod)
        {
            Sprite middleLeft = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddleLeft).Clone() as Sprite;
            middleLeft.Position = Vector2.UnitY * Height;
            Height += middleLeft.SourceHeight;
            this.Elements.Add("middleLeft" + mod, middleLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite middle = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddle).Clone() as Sprite;
                middle.Position = middleLeft.Position + (Vector2.UnitX * middleLeft.SourceWidth * i);
                this.Elements.Add("middle" + i + "_" + mod, middle);
            }
            Sprite middleRight = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddleRight).Clone() as Sprite;
            middleRight.Position = middleLeft.Position + (Vector2.UnitX * (Width - middleRight.SourceWidth));
            this.Elements.Add("middleRight" + mod, middleRight);
        }

        private void Bottom(int size)
        {
            Sprite bottomLeft = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameBottomLeft).Clone() as Sprite;
            bottomLeft.Position = Vector2.UnitY * Height;
            Height += bottomLeft.SourceHeight;
            this.Elements.Add("bottomLeft", bottomLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite bottomMiddle = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameBottomMiddle).Clone() as Sprite;
                bottomMiddle.Position = bottomLeft.Position + (Vector2.UnitX * bottomMiddle.SourceWidth * i);
                this.Elements.Add("bottomMiddle" + i, bottomMiddle);
            }
            Sprite bottomRight = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameBottomRight).Clone() as Sprite;
            bottomRight.Position = bottomLeft.Position + (Vector2.UnitX * (Width - bottomRight.SourceWidth));
            this.Elements.Add("bottomRight", bottomRight);
        }
        #endregion
    }
}
