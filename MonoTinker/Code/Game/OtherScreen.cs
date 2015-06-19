using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Managers;

namespace MonoTinker.Code.Game
{
    public sealed class OtherScreen : Screen
    {
        private Texture2D bg;
        private Texture2D bg2;
        private Player player;
        public OtherScreen(IServiceProvider service) : base(service, "Other")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            bg = content.Load<Texture2D>("playerRun");
            bg2 = content.Load<Texture2D>("ghettoville1");
            player = new Player(new Animation(bg,new Vector2(130,150),1));
            player.Transform.Position = Vector2.One*50;
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
           
            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen("Menu");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg2,Vector2.Zero,Color.White);
            player.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
