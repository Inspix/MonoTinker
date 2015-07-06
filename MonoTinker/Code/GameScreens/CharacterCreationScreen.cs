using MonoTinker.Code.Components.Elements.DebugGraphics;
using MonoTinker.Code.Components.GameComponents;

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
        private Sprite arrow;
        private float selectedHairSaturation;
        private float selectedSkinSaturation;
        private Sprite characterBox;
        private StatPicker statPicker;
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
            characterBox = BoxFactory.BoxSprite(ScreenManager.Batch, new Vector2(4, 2));
            arrow = AssetManager.Instance.Get<Sprite>("BasicArrow").DirectClone();
            arrow.Position = Vector2.One*150;


            picker = new ColorPicker(new Vector2(332, 300), ScreenManager.Device,4,true);
            picker.IsVisible = false;
            picker.FadeSpeed = 0.03f;
            picker.Transitioning = true;
            picker.PickCallback = OnColorPick;
            
            skinTonePicker = ColorPicker.SkinTonePicker(new Vector2(332, 300));
            skinTonePicker.IsVisible = false;
            skinTonePicker.FadeSpeed = 0.03f;
            skinTonePicker.Transitioning = true;
            skinTonePicker.PickCallback = OnSkinColorPick;

            statPicker = new StatPicker(new Vector2(325, 300), ScreenManager.Device, Stats.Ten);
            statPicker.IsVisible = false;
            statPicker.Transitioning = true;
            statPicker.FadeSpeed = 0.03f;


            slider = new Slider(new Vector2(550, 300),ScreenManager.Device,20);
            slider.IsVisible = false;
            slider.FadeSpeed = 0.03f;
            slider.Transitioning = true;
            slider.OnValueChangeCallback = OnValueChangeCallback;

            character = Factory.HumanDownWalk();
            character.Position = new Vector2(400,150);
            characterBox.Position = character.Position + character.Layer(0).CurrentFrame.SpriteCenter - characterBox.SpriteCenter; ;

            font = AssetManager.Instance.Get<SpriteFont>("Standart");

            name = new Text(font, character.Position - Vector2.UnitY*20, "");
            name.Scale = Vector2.One*0.5f;

            inputBox = new InputBox(new Vector2(ScreenManager.ScreenCenter.X/3, ScreenManager.ScreenCenter.Y/3),
                ScreenManager.Device, (int) (ScreenManager.ScreenDimensions.X - (ScreenManager.ScreenDimensions.X/3)),
                (int) (ScreenManager.ScreenDimensions.Y - (ScreenManager.ScreenDimensions.Y/3)));
            inputBox.Callback = (x) =>
            {
                this.name.Contents = x;
                this.name.Position = this.character.Position - Vector2.UnitY * 20 + (this.character.Layer(0).CurrentFrame.SpriteCenter * Vector2.UnitX)
                                            - (this.name.Size/2f*Vector2.UnitX);
                this.enterName = false;
            };

            menuBox = new MenuBox(new Vector2(50, ScreenManager.ScreenCenter.Y), ScreenManager.Device,
                new string[] {"Name", "Hair", "Hair color", "Skin color","Class","Bonus stat","Confirm"}, new Vector2(5, 15),Vector2.UnitY*50);
            menuBox.Label(6).Position += new Vector2(0, menuBox.Label(6).Size.Y + 25);
            menuBox.OnIndexChange = this.OnIndexChange;
            menuBox.Position -= new Vector2(0, menuBox.Size.Y/2);
            menuBox[0] = () => this.enterName = true;
            menuBox[1] = ToggleHair;
            menuBox[6] = () => ScreenManager.ShouldExit = true;
        }

        private void OnValueChangeCallback(float f)
        {
            switch (menuBox.SelectedIndex)
            {
                case 2:
                    if (character.ContainsLayerTag(An.Walk.HeadHairWhite + "Down"))
                    {
                        Console.WriteLine(f);
                        selectedHairSaturation = f;
                        this.character.Layer(An.Walk.HeadHairWhite + "Down").Tint = ColorHelper.Saturation(selectedHairColor,f);
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
            statPicker.DrawElements();
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
            arrow.Draw(spriteBatch);
            statPicker.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void OnColorPick(Color clr)
        {
            selectedHairColor = clr;
            if (character.ContainsLayerTag(An.Walk.HeadHairWhite + "Down"))
            {
                this.character.Layer(An.Walk.HeadHairWhite + "Down").Tint = selectedHairColor;
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
                ToggleHair();
            }
            picker.Update(gameTime);
            skinTonePicker.Update(gameTime);
            slider.Update(gameTime);
            character.Update(gameTime);
            name.Update(gameTime);
            menuBox.Update(gameTime);
            menuBox.MouseUpdate(gameTime);
            statPicker.Update(gameTime);

        }

        private void ToggleHair()
        {
            if (character.ContainsLayerTag(An.Walk.HeadHairWhite + "Down"))
            {
                character.Reset();
                character.RemoveLayer(An.Walk.HeadHairWhite + "Down");
            }
            else
            {
                character.Reset();
                Factory.AddLayer(ref character, An.Walk.HeadHairWhite + "Down",selectedHairColor);
            }
        }

        private void OnIndexChange(int index)
        {
            slider.Reset();
            bool pickerCheck = menuBox.SelectedIndex == 2 && character.ContainsLayerTag(An.Walk.HeadHairWhite + "Down");
            menuBox.Label(1).Contents = menuBox.SelectedIndex == 1 ? "<Hair>" : "Hair";
            picker.IsVisible = pickerCheck;
            skinTonePicker.IsVisible = menuBox.SelectedIndex == 3;
            statPicker.IsVisible = menuBox.SelectedIndex == 5;
            slider.IsVisible = pickerCheck; 

        }
    }
}
