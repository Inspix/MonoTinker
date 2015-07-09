

namespace MonoTinker.Code.GameScreens
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Components.Elements.DebugGraphics;
    using Components.GameComponents;
    using Components.GameObjects;
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
        private CharacterClass classSelected;
        private bool classPicked;
        private float selectedHairSaturation;
        private float selectedSkinSaturation;
        private Sprite characterBox;
        private StatPicker statPicker;
        private ColorPicker picker;
        private ColorPicker skinTonePicker;
        private ChoiceBox classPicker;
        private Sprite checkbox;
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
            font = AssetManager.Instance.Get<SpriteFont>("Standart");
            characterBox = BoxFactory.BoxSprite(ScreenManager.Batch, new Vector2(4, 2));
            character = Factory.HumanDownWalk();
            character.Position = new Vector2(400, 150);
            characterBox.Position = character.Position + character.Layer(0).CurrentFrame.SpriteCenter - characterBox.SpriteCenter;
            checkbox = AssetManager.Instance.Get<Sprite>("BasicCheckbox").DirectClone(true);
            checkbox.Transitioning = true;
            classPicker = new ChoiceBox(new Vector2(290, 275), ScreenManager.Device, ChoiceBoxType.CharacterInfo);
            statPicker = new StatPicker(new Vector2(325, classPicker.PosY + classPicker.Size.Y), ScreenManager.Device, Stats.Ten);
            picker = new ColorPicker(new Vector2(332, 300), ScreenManager.Device, 4, true);
            skinTonePicker = ColorPicker.SkinTonePicker(new Vector2(332, 300));
            slider = new Slider(new Vector2(550, 300), ScreenManager.Device, 20);
            name = new Text(font, character.Position - Vector2.UnitY * 20, "");
            inputBox = new InputBox(new Vector2(ScreenManager.ScreenCenter.X / 3, ScreenManager.ScreenCenter.Y / 3),
                ScreenManager.Device, (int)(ScreenManager.ScreenDimensions.X - (ScreenManager.ScreenDimensions.X / 3)),
                (int)(ScreenManager.ScreenDimensions.Y - (ScreenManager.ScreenDimensions.Y / 3)));
            menuBox = new MenuBox(new Vector2(50, ScreenManager.ScreenCenter.Y), ScreenManager.Device,
                new string[] { "Name", "Hair", "Hair color", "Skin color", "Class", "Bonus stat", "Confirm" }, new Vector2(5, 15), Vector2.UnitY * 50);

            picker.IsVisible = false;
            picker.FadeSpeed = 0.03f;
            picker.Transitioning = true;
            picker.PickCallback = OnColorPick;

            skinTonePicker.IsVisible = false;
            skinTonePicker.FadeSpeed = 0.03f;
            skinTonePicker.Transitioning = true;
            skinTonePicker.PickCallback = OnSkinColorPick;

            statPicker.IsVisible = false;
            statPicker.Transitioning = true;
            statPicker.FadeSpeed = 0.03f;

            classPicker.IsVisible = false;
            classPicker.FadeSpeed = 0.1f;
            classPicker.Transitioning = true;
            classPicker.AddItem(BoxFactory.CharacterInfoBox(Vector2.Zero), () =>
            {
                classPicked = true;
                classSelected =CharacterClass.Warrior;
                statPicker.ChangeStartingStats(Prefabs.BaseStats(CharacterClass.Warrior));
                OnIndexChange(0);
            });
            classPicker.AddItem(BoxFactory.CharacterInfoBox(Vector2.Zero, CharacterClass.Archer), () =>
            {
                classPicked = true;
                classSelected = CharacterClass.Archer;
                statPicker.ChangeStartingStats(Prefabs.BaseStats(CharacterClass.Archer));
                OnIndexChange(0);
            });
            classPicker.AddItem(BoxFactory.CharacterInfoBox(Vector2.Zero, CharacterClass.Wizard), () => 
            {
                classPicked = true;
                classSelected = CharacterClass.Wizard;
                statPicker.ChangeStartingStats(Prefabs.BaseStats(CharacterClass.Wizard));
                OnIndexChange(0);
            });
            classPicker.OnIndexChange = () => OnIndexChange(0);
            classPicker.LeftButtonCallback += () => checkbox.IsVisible = false;
            classPicker.RightButtonCallback += () => checkbox.IsVisible = false;
            checkbox.Position = classPicker.Position + classPicker.Size*new Vector2(1,0) + new Vector2(-64,16);
            slider.IsVisible = false;
            slider.FadeSpeed = 0.03f;
            slider.Transitioning = true;
            slider.OnValueChangeCallback = OnValueChangeCallback;

            name.Scale = Vector2.One*0.5f;
            
            inputBox.Callback = (x) =>
            {
                this.name.Contents = x;
                this.name.Position = this.character.Position - Vector2.UnitY * 20 + (this.character.Layer(0).CurrentFrame.SpriteCenter * Vector2.UnitX)
                                            - (this.name.Size/2f*Vector2.UnitX);
                this.enterName = false;
            };

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
            classPicker.DrawElements();
            spriteBatch.Begin();

            menuBox.Draw(spriteBatch);
            characterBox.Draw(spriteBatch);
            character.Draw(spriteBatch);
            classPicker.Draw(spriteBatch);
            name.Draw(spriteBatch);
            picker.Draw(spriteBatch);
            skinTonePicker.Draw(spriteBatch);
            slider.Draw(spriteBatch);
            if (enterName)
            {
                inputBox.Draw(spriteBatch);
            }
            statPicker.Draw(spriteBatch);
            checkbox.Draw(spriteBatch);
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
            classPicker.Update(gameTime);
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
            statPicker.IsVisible = classPicked && menuBox.SelectedIndex == 5;
            classPicker.IsVisible = menuBox.SelectedIndex == 4 || menuBox.SelectedIndex == 5;
            checkbox.IsVisible = (menuBox.SelectedIndex == 4 || menuBox.SelectedIndex == 5) && classPicked && classPicker.CurrentItem == (int) classSelected;
            slider.IsVisible = pickerCheck; 

        }
    }
}
