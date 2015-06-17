using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Managers;

namespace MonoTinker.Code.Components
{
    public sealed class OtherScreen : Screen
    {
        private Texture2D bg;
        public OtherScreen(IServiceProvider service, string path) : base(service, path)
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            bg = content.Load<Texture2D>("playerRun");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen("Menu");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg,Vector2.One,Color.White);
            spriteBatch.End();
        }
    }
}
