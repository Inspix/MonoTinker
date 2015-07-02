namespace MonoTinker.Code.GameScreens
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Components.Elements;
    using Components.Extensions;
    using Components.UI;
    using Managers;
    using Utils;

    public sealed class CharacterCreationScreen : Screen
    {
        private MenuBox menuBox;
        private InputBox inputBox;
        private Color selectedHairColor = Color.White;
        private Color selectedSkinColor = Color.White;
        private float selectedHairSaturation;
        private float selectedSkinSaturation;
        private Sprite characterBox;
        private ColorPicker picker;
        private ColorPicker skinTonePicker;
        private Slider slider;
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
            characterBox = TextBoxFactory.GenerateBoxSprite(ScreenManager.Batch, new Vector2(3, 2));
            
            picker = new ColorPicker(new Vector2(332, 300), ScreenManager.Device,4,true);
            picker.IsVisible = false;
            picker.Transitioning = true;
            picker.PickCallback = OnColorPick;

            skinTonePicker = ColorPicker.SkinTonePicker(new Vector2(332, 300));
            skinTonePicker.IsVisible = false;
            skinTonePicker.Transitioning = true;
            skinTonePicker.PickCallback = OnSkinColorPick;

            slider = new Slider(new Vector2(550, 300),ScreenManager.Device,20);
            slider.IsVisible = false;
            slider.Transitioning = true;
            slider.OnValueChangeCallback = OnValueChangeCallback;

            character = Factory.CharacterAnimation();
            character.Transform.Position = new Vector2(400,200);
            characterBox.Position = character.Transform.Position + character.Layer(0).CurrentFrame.SpriteCenter - characterBox.SpriteCenter;

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
            menuBox.OnIndexChange = OnIndexChange;
            menuBox.MenuTransform.Position -= new Vector2(0, menuBox.Size.Y/2);
            menuBox[0] = () => this.enterName = true;
            menuBox[6] = () => ScreenManager.ShouldExit = true;
        }

        private void OnValueChangeCallback(float f)
        {
            switch (menuBox.SelectedIndex)
            {
                case 2:
                    if (character.ContainsLayerTag(An.Walk.Hair + "Down"))
                    {
                        Console.WriteLine(f);
                        selectedHairSaturation = f;
                        this.character.Layer(An.Walk.Hair + "Down").Tint = ColorHelper.Saturation(selectedHairColor,f);
                    }
                    break;
                case 3:
                    selectedSkinSaturation = f;
                    character.Layer(0).Tint = ColorHelper.Saturation(selectedSkinColor,f);
                    break;

            }
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
            slider.DrawElements();
            picker.DrawElements();
            skinTonePicker.DrawElements();

            spriteBatch.Begin();
            menuBox.Draw(spriteBatch);
            characterBox.Draw(spriteBatch);
            character.Draw(spriteBatch);
            name.Draw(spriteBatch);
            picker.Draw(spriteBatch);
            skinTonePicker.Draw(spriteBatch);
            slider.Draw(spriteBatch);
            if (enterName)
            {
                inputBox.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        private void OnColorPick(Color clr)
        {
            selectedHairColor = clr;
            if (character.ContainsLayerTag(An.Walk.Hair + "Down"))
            {
                this.character.Layer(An.Walk.Hair + "Down").Tint = selectedHairColor;
            }
        }

        private void OnSkinColorPick(Color clr)
        {
            selectedSkinColor = clr;
            character.Layer(0).Tint = selectedSkinColor;
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

            if (this.menuBox.SelectedIndex == 1 && (InputHandler.DirectionDownOnce("left") || InputHandler.DirectionDownOnce("right")))
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
            skinTonePicker.Update(gameTime);
            slider.Update(gameTime);
            character.Update(gameTime);
            name.Update(gameTime);
            menuBox.Update(gameTime);

        }

        private void OnIndexChange(int index)
        {
            slider.Reset();
            bool pickerCheck = menuBox.SelectedIndex == 2 && character.ContainsLayerTag(An.Walk.Hair + "Down");
            menuBox.Label(1).Contents = menuBox.SelectedIndex == 1 ? "<Hair>" : "Hair";
            picker.IsVisible = pickerCheck;
            skinTonePicker.IsVisible = menuBox.SelectedIndex == 3;
            slider.IsVisible = pickerCheck || menuBox.SelectedIndex == 3; 

        }
    }
}
