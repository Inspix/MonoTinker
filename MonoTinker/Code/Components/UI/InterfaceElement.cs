using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.UI
{
    using System.Collections.Generic;

    using Elements;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class InterfaceElement : Fadeable, IElementDrawable,ITransformable
    {
        protected SpriteAtlas Elements;
        protected List<Text> Labels;
        protected bool OverrideDrawElements;
        protected bool OverrideDrawLabels;
        protected int Width;
        protected int Height;
        protected RenderTarget2D RenderTarget2D;
        protected GraphicsDevice Device;
        protected SpriteBatch Batch;
        
        protected InterfaceElement(Vector2 position,GraphicsDevice device)
        {
            this.Elements = new SpriteAtlas();
            this.Labels = new List<Text>();
            this.Position = position;
            this.Device = device;
            this.Batch = new SpriteBatch(this.Device);
            this.ScaleF = 1;
        }

        public Vector2 Offset { get; set; }

        public Vector2 Size { get { return new Vector2(Width,Height) * Scale;} }

        public Vector2 Position { get; set; }

        public float PosX
        {
            get { return this.Position.X; }
            set
            {
                this.Position = new Vector2(value, Position.Y);
            }
        }

        public float PosY
        {
            get { return this.Position.Y; }
            set
            {
                this.Position = new Vector2(Position.X, value);
            }
        }

        public Vector2 Scale { get; set; }

        public float ScaleF
        {
            get { return (this.Scale.X + this.Scale.Y)/2f; }
            set { this.Scale = Vector2.One*value; }
        }

        public float Rotation { get; set; }

        public virtual void DrawElements()
        {

            this.Device.SetRenderTarget(this.RenderTarget2D);
            this.Device.Clear(Color.FromNonPremultiplied(0,0,0,0));
            this.Batch.Begin();
            foreach (var element in Elements)
            {
                element.Value.Draw(this.Batch);
            }

            if (!OverrideDrawLabels)
            {
                foreach (var label in Labels)
                {
                    if (label.IsVisible)
                    {
                        label.Draw(Batch);
                    }
                } 
            }

            if (this.OverrideDrawElements) return;

            this.Batch.End();
            this.Device.SetRenderTarget(null);
        }

        public virtual void DrawToRenderTarget(SpriteBatch spriteBatch, RenderTarget2D target)
        {
            DrawElements();
            Device.SetRenderTarget(target);
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
                 spriteBatch.Draw(this.RenderTarget2D, this.Position + this.Offset,this.RenderTarget2D.Bounds,Color.White * this.alpha,this.Rotation,Vector2.Zero,this.Scale,SpriteEffects.None, 0);
            }

        }
    }
}
