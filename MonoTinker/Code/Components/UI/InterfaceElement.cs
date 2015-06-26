namespace MonoTinker.Code.Components.UI
{
    using System.Collections.Generic;

    using Elements;

    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class InterfaceElement : Fadeable
    {
        protected Transform Transform;

        protected SpriteAtlas Elements;

        protected List<Text> Labels;

        protected bool OverrideDrawElements;

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
        
        public virtual void DrawElements()
        {
            this.Device.SetRenderTarget(this.RenderTarget2D);
            this.Device.Clear(Color.FromNonPremultiplied(0,0,0,0));
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

            if (this.OverrideDrawElements) return;

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
            this.DrawElements();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                //this.DrawElements();
                 spriteBatch.Draw(this.RenderTarget2D, this.Transform.Position, this.alpha);
            }

        }
    }
}
