namespace MonoTinker.Code.Components.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Text
    {
        private SpriteFont font;

        public Vector2 Position;

        private bool isVisible;
        private Color alpha;

        private int defaultAlpha;

        private bool fadeIn;

        private bool fadeOut;
        private bool transitioning;

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

        public bool Transitioning
        {
            set
            {
                this.transitioning = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (fadeIn || fadeOut)
                {
                    return;
                }
                if (transitioning)
                {
                    if (value)
                    {
                        this.fadeIn = true;
                        this.isVisible = true;
                    }
                    else
                    {
                        this.fadeOut = true;
                    } 
                }
                else
                {
                    this.isVisible = value;
                }
            }
        }

        private void Transition()
        {
            if (this.fadeIn)
            {
                this.Alpha += 5;
                if (this.Alpha >= this.defaultAlpha)
                {
                    this.Alpha = this.defaultAlpha;
                    this.fadeIn = false;
                }
            }
            if (this.fadeOut)
            {
                this.Alpha -= 5;
                if (this.Alpha <= 0)
                {
                    this.fadeOut = false;
                    this.isVisible = false;
                }
            }
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

        public int DefaultAlpha
        {
            get
            {
                return this.defaultAlpha;
            }
            set
            {
                if (value > 255)
                {
                    this.defaultAlpha = 255;

                }
                else if (value < 0)
                {
                    this.defaultAlpha = 0;
                }
                else
                {
                    this.defaultAlpha = value;
                }

            }
        }



        private int Alpha
        {
            get
            {
                return this.alpha.A;
            }
            set
            {
                if (value > 255)
                {
                    this.alpha.A = 255;
                    this.alpha.R = 255;
                    this.alpha.G = 255;
                    this.alpha.B = 255;

                }
                else if (value < 0)
                {
                    this.alpha.A = 0;
                    this.alpha.R = 0;
                    this.alpha.G = 0;
                    this.alpha.B = 0;
                }
                else
                {
                    this.alpha.A = (byte)value;
                    this.alpha.R = (byte)value;
                    this.alpha.G = (byte)value;
                    this.alpha.B = (byte)value;
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
            spriteBatch.DrawString(this.font,this.Contents,this.Position,this.Clr * (Alpha/100f));
        }
    }
}
