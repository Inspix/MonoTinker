using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.UI;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.GameScreens
{
    public sealed class CharacterCreationScreen : Screen
    {
        private MenuBox menuBox;

        public CharacterCreationScreen(IServiceProvider service) : base(service, "CharacterCreation")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            menuBox = new MenuBox(new Vector2(50,ScreenManager.ScreenCenter.Y), ScreenManager.Device,new string[] {"Name","Hair","Hair color","Skin color"}, new Vector2(5,15));
            menuBox.MenuTransform.Position -= new Vector2(0, menuBox.Size.Y/2);
            menuBox[1] = () => ScreenManager.ShouldExit = true;
        }

        public override void UnloadContent()
        {
            this.content.Unload();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            menuBox.DrawElements();
            spriteBatch.Begin();
            menuBox.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.DirectionDownOnce("up"))
            {
                this.menuBox.SelectedIndex--;
            }
            if (InputHandler.DirectionDownOnce("down"))
            {
                this.menuBox.SelectedIndex++;
            }
            menuBox.Update(gameTime);
        }
    }
}
