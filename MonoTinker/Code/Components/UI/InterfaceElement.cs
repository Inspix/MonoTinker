namespace MonoTinker.Code.Components.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;

    using Components.Elements;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class InterfaceElement
    {
        private Color alpha;

        private int defaultAlpha;

        private bool fadeIn;

        private bool fadeOut;

        private bool isVisible;

        protected Transform Transform;

        protected SpriteAtlas Elements;

        protected List<Text> Labels;

        protected int Width;

        protected int Height;

        protected RenderTarget2D RenderTarget2D;
        protected GraphicsDevice Device;

        protected SpriteBatch Batch;

        protected InterfaceElement(Vector2 position,GraphicsDevice device)
        {
            this.Elements = new SpriteAtlas();
            this.Labels = new List<Text>();
            this.Transform = new Transform(position);
            this.IsVisible = true;
            this.Device = device;
            this.Batch = new SpriteBatch(this.Device);
            this.alpha = Color.White;
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
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
                if (this.Alpha == 0)
                {
                    this.fadeOut = false;
                    this.isVisible = false;
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
                    this.defaultAlpha = 255;
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
        
        protected virtual void DrawElements()
        {
            this.Device.SetRenderTarget(this.RenderTarget2D);
            this.Device.Clear(default(Color));
            this.Batch.Begin();
            foreach (var element in Elements)
            {
                element.Value.Draw(this.Batch);
            }

            foreach (var label in Labels)
            {
                if (label.IsVisible)
                {
                    label.Draw(Batch);
                }
            }
            this.Batch.End();
            this.Device.SetRenderTarget(null);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (fadeIn || fadeOut)
            {
                this.Transition();
            }
            foreach (var label in Labels)
            {
                label.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                this.DrawElements();
                 spriteBatch.Draw(this.RenderTarget2D, this.Transform.Position, this.alpha);
            }

        }
    }
}
