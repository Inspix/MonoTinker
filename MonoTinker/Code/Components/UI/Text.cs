using MonoTinker.Code.Components.Elements;

namespace MonoTinker.Code.Components.UI
{
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Text : Fadeable
    {
        private SpriteFont font;
        private Transform transform;

        public Color Clr;

        private string contents;

        public Text(SpriteFont font, Vector2 position, string contents, byte alpha = 255, bool isVisible = true)
        {
            this.font = font;
            this.isVisible = isVisible;
            this.transform = new Transform(position);
            this.contents = contents;
            this.Clr = Color.White;
            this.alpha = ColorHelper.AlphaChange(Color.White, alpha);
            this.DefaultAlpha = 255;
        }

        public Vector2 Size
        {
            get { return this.font.MeasureString(this.Contents); }
        }

        public string Contents
        {
            get
            {
                return this.contents;
            }
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.contents = "...";
                }
                else
                {
                    this.contents = value;
                }
            }
        }

        public Vector2 Position
        {
            get { return this.transform.Position; }
            set { this.transform.Position = value; }

        }

        public Transform Transform
        {
            get { return this.transform;}
            set { this.transform = value; }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.fadeIn || this.fadeOut)
            {
                this.Transition();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.font,this.Contents,this.Position,this.Clr * (this.Alpha/255f),this.transform.Rotation,Vector2.Zero, this.transform.Scale,SpriteEffects.None, 0);
        }
    }
}
