using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    public abstract class Screen
    {
        protected ContentManager content;

        protected Screen(IServiceProvider service, string path)
        {
            this.content = new ContentManager(service, "Content/" + path);
        }

        protected abstract void LoadContent();
        public abstract void UnloadContent();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
