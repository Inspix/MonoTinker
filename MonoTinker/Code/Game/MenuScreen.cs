using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Managers;

namespace MonoTinker.Code.Game
{
    public sealed class MenuScreen : Screen
    {
        private Texture2D bg;

        public MenuScreen(IServiceProvider service) : base(service, "Menu")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            bg = content.Load<Texture2D>("ghettoville");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen("Other");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg,Vector2.Zero,Color.White);
            spriteBatch.End();
        }

        
    }
}
