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
        private InputBox inputBox;
        private bool enterName;
        private string name;

        public CharacterCreationScreen(IServiceProvider service) : base(service, "CharacterCreation")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            inputBox = new InputBox(new Vector2(ScreenManager.ScreenCenter.X/3,ScreenManager.ScreenCenter.Y/3),ScreenManager.Device, (int)(ScreenManager.ScreenDimensions.X - (ScreenManager.ScreenDimensions.X/3)), (int)(ScreenManager.ScreenDimensions.Y - (ScreenManager.ScreenDimensions.Y/ 3)));
            inputBox.Callback = (x) =>
            {
                this.name = x;
                this.enterName = false;
            };
            
            menuBox = new MenuBox(new Vector2(50,ScreenManager.ScreenCenter.Y), ScreenManager.Device,new string[] {"Name","Hair","Hair color","Skin color"}, new Vector2(5,15));
            menuBox.MenuTransform.Position -= new Vector2(0, menuBox.Size.Y/2);
            menuBox[0] = () => this.enterName = true;
            menuBox[1] = () => ScreenManager.ShouldExit = true;
        }

        public override void UnloadContent()
        {
            this.content.Unload();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (enterName)
            {
                inputBox.DrawElements();
            }
            menuBox.DrawElements();
            spriteBatch.Begin();
            menuBox.Draw(spriteBatch);
            if (enterName)
            {
                inputBox.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (enterName)
            {
                menuBox.Update(gameTime);
                inputBox.Update(gameTime);

                return;
            }
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
