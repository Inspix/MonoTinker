using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.UI
{
    using System.Collections.Generic;

    using Elements;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class InterfaceElement : Fadeable, IElementDrawable
    {
        protected Transform Transform;

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
            this.Transform = new Transform(position);
            this.Device = device;
            this.Batch = new SpriteBatch(this.Device);
        }

        public Vector2 Offset { get; set; }

        public Vector2 Size { get { return new Vector2(Width,Height);} }

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

        public virtual void DrawToCurrentRenderTarget(SpriteBatch spriteBatch)
        {

            foreach (var element in Elements)
            {
                element.Value.Draw(spriteBatch);
            }
            foreach (var label in Labels)
            {
                if (label.IsVisible)
                {
                    label.Draw(spriteBatch);
                }
            }
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
                 spriteBatch.Draw(this.RenderTarget2D, this.Transform.Position + this.Offset,this.RenderTarget2D.Bounds,Color.White * this.alpha,this.Transform.Rotation,Vector2.Zero,this.Transform.Scale,SpriteEffects.None, 0);
            }

        }
    }
}
