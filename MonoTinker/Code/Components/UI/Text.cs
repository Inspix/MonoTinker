namespace MonoTinker.Code.Components.UI
{
    using global::MonoTinker.Code.Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Text : Fadeable
    {
        private SpriteFont font;

        public Vector2 Position;

        public Color Clr;

        private string contents;

        public Text(SpriteFont font, Vector2 position, string contents)
        {
            this.font = font;
            this.IsVisible = true;
            this.Position = position;
            this.contents = contents;
            this.Clr = Color.White;
            this.alpha = Color.White;
            this.DefaultAlpha = 255;
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

        public virtual void Update(GameTime gameTime)
        {
            if (this.fadeIn || this.fadeOut)
            {
                this.Transition();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.font,this.Contents,this.Position,this.Clr * (this.Alpha/100f));
        }
    }
}
