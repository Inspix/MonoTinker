using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Extensions;
using MonoTinker.Code.Components.UI;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.GameScreens
{
    public sealed class CharacterCreationScreen : Screen
    {
        private MenuBox menuBox;
        private InputBox inputBox;
        private ColorPicker picker;
        private SpriteFont font;
        private AnimationV2 character;
        private Text name;
        private bool enterName;

        public CharacterCreationScreen(IServiceProvider service) : base(service, "CharacterCreation")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            picker = new ColorPicker(ScreenManager.ScreenCenter + Vector2.UnitX * 100,ScreenManager.Device,3);
            picker.PickCallback = OnColorPick;
            character = Factory.CharacterAnimation();
            character.Transform.Position = new Vector2(400,200);
            font = AssetManager.Instance.Get<SpriteFont>("Standart");
            name = new Text(font, character.Transform.Position - Vector2.UnitY*20, "");
            name.Transform.Scale = Vector2.One*0.5f;
            inputBox = new InputBox(new Vector2(ScreenManager.ScreenCenter.X/3, ScreenManager.ScreenCenter.Y/3),
                ScreenManager.Device, (int) (ScreenManager.ScreenDimensions.X - (ScreenManager.ScreenDimensions.X/3)),
                (int) (ScreenManager.ScreenDimensions.Y - (ScreenManager.ScreenDimensions.Y/3)));
            inputBox.Callback = (x) =>
            {
                this.name.Contents = x;
                this.name.Transform.Position = this.character.Transform.Position - Vector2.UnitY * 20 + (this.character.Layer(0).CurrentFrame.SpriteCenter * Vector2.UnitX)
                                            - (this.name.Size/4f*Vector2.UnitX);
                this.enterName = false;
            };

            menuBox = new MenuBox(new Vector2(50, ScreenManager.ScreenCenter.Y), ScreenManager.Device,
                new string[] {"Name", "Hair", "Hair color", "Skin color","Class","Bonus stat","Confirm"}, new Vector2(5, 15),Vector2.UnitY*50);
            menuBox.Label(6).Transform.Position += new Vector2(0, menuBox.Label(6).Size.Y + 25);
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
            picker.DrawElements();
            spriteBatch.Begin();
            menuBox.Draw(spriteBatch);
            character.Draw(spriteBatch);
            name.Draw(spriteBatch);
            if (enterName)
            {
                inputBox.Draw(spriteBatch);
            }
            picker.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void OnColorPick(Color clr)
        {
            this.character.Layer(An.Walk.Hair + "Down").Tint = clr;
        }

        public override void Update(GameTime gameTime)
        {
            if (enterName)
            {
                menuBox.Update(gameTime);
                character.Update(gameTime);
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
            if (menuBox.SelectedIndex == 1)
            {
                menuBox.Label(1).Contents = "<Hair>";
            }
            else
            {
                menuBox.Label(1).Contents = "Hair";
            }
            if (Keys.X.DownOnce())
            {
                if (character.ContainsLayerTag(An.Walk.Hair + "Down"))
                {
                    character.Reset();
                    character.RemoveLayer(An.Walk.Hair + "Down");
                }
                else
                {
                    character.Reset();
                    Factory.AddLayer(ref character, An.Walk.Hair + "Down");
                }
            }
            picker.Update(gameTime);
            character.Update(gameTime);
            name.Update(gameTime);
            menuBox.Update(gameTime);

        }
    }
}
