using MonoTinker.Code.Components.GameObjects;

namespace MonoTinker.Code.GameScreens
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Components;
    using Components.Elements;
    using Components.Extensions;
    using Components.GameComponents;

    using Managers;
    using Utils;
    using Components.UI;

    public sealed class TestGround : Screen
    {
        private AnimationController controler;
        private ChoiceBox choicebox;
        private StatusBar status;

        private TextBox box;
        private FancyTextBox fbox;
        private Texture2D splash;
        private Inventory inventory;
        private Inventory inventory2;
        
        private bool attacking;
        private Vector2 position;

        public TestGround(IServiceProvider service) : base(service, "CharacterCreation")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            choicebox = new ChoiceBox(new Vector2(500,220),ScreenManager.Device, ChoiceBoxType.CharacterInfo );
            
            splash = TextureMaker.ClassSplash(ScreenManager.Device, CharacterClass.Archer);
            status = new StatusBar(Vector2.One, ScreenManager.Device);
            status.DefaultAlpha = 1;
            status.Transitioning = true;
            status.FadeSpeed = 0.01f;
            inventory = new Inventory(Vector2.One*200, ScreenManager.Device,5);
            inventory.FadeSpeed = 0.01f;
            inventory2 = new Inventory(Vector2.One * 300, ScreenManager.Device, 4);
            controler = new AnimationController();
            Item x = Factory.CreateItem("Ashbringer", 50, 25, 50, 10, 10, ItemRarity.Legendary);
            Item y = Factory.CreateItem("Staff of Regrowth", 10, 5, 35, 50, 50, ItemRarity.Epic);
            fbox = new FancyTextBox(Vector2.UnitY* 300,ScreenManager.Device, @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",15,WriteEffect.CharacterByCharacter); 


            ItemTile item = new ItemTile(AssetManager.Instance.Get<Sprite>(Sn.Items.Ashbringer).DirectClone(),Vector2.One * 400,x,ScreenManager.Device);
            ItemTile item2 = new ItemTile(AssetManager.Instance.Get<Sprite>(Sn.Items.StaffOfRegrowth).DirectClone(), Vector2.One * 400, y, ScreenManager.Device);
            box = new TextBox(Vector2.One * 100,ScreenManager.Device, "Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool!", new Vector2(5,2),WriteEffect.CharacterByCharacter);
            /*choicebox.AddItem(item.DirectClone(true),() => Console.WriteLine("blalba"));
            choicebox.AddItem(item2.DirectClone(true), () => Console.WriteLine("blalba2"));*/
            choicebox.AddItem(BoxFactory.CharacterInfoBox(Vector2.Zero),() => Console.WriteLine("Blablaba"));
            
            choicebox.AddItem(BoxFactory.CharacterInfoBox(Vector2.Zero,CharacterClass.Archer));
            choicebox.AddItem(BoxFactory.CharacterInfoBox(Vector2.Zero,CharacterClass.Wizard));
            controler = AssetManager.Instance.GetBaseWalkingController(An.Walk.BodyHuman);
            controler.IsVisible = true;
            foreach (var animationV2 in controler.States)
            {
                Console.WriteLine(animationV2.Key);
            }

            var slashUp = new AnimationV2(AssetManager.Instance.Get<Animation>(An.Thrust.BodyHuman + "Up"));
            slashUp.Looping = false;
            slashUp.AddLayer(AssetManager.Instance.Get<Animation>(An.Thrust.WeaponLongSpear + "Up"),"slashup");
            var slashLeft = new AnimationV2(AssetManager.Instance.Get<Animation>(An.Thrust.BodyHuman + "Left"));
            slashLeft.Looping = false;
            slashLeft.AddLayer(AssetManager.Instance.Get<Animation>(An.Thrust.WeaponLongSpear + "Left"), "slashleft");
            var slashRight = new AnimationV2(AssetManager.Instance.Get<Animation>(An.Thrust.BodyHuman + "Right"));
            slashRight.Looping = false;
            slashRight.AddLayer(AssetManager.Instance.Get<Animation>(An.Thrust.WeaponLongSpear + "Right"), "slashright");
            var slashDown = new AnimationV2(AssetManager.Instance.Get<Animation>(An.Thrust.BodyHuman + "Down"));
            slashDown.Looping = false;
            slashDown.AddLayer(AssetManager.Instance.Get<Animation>(An.Thrust.WeaponLongSpear + "Down"), "slashdown");

            inventory.AddItemToSlot(item,1);
            inventory.AddItemToSlot(item2, 10);
            inventory2.AddItemToSlot(item,5);
            inventory2.AddItemToSlot(item2,3);
            controler.AddState("slashUp",slashUp);
            controler.AddState("slashLeft", slashLeft);
            controler.AddState("slashDown", slashDown);
            controler.AddState("slashRight", slashRight);
            controler.OnStateAnimationFinish += this.StateAnimationFinish;

        }

        private void StateAnimationFinish(string stateName)
        {
             switch (stateName)
            {
                case "slashUp":
                    controler.ChangeState("idleUp");
                    attacking = false;
                    break;
                case "slashDown":
                    controler.ChangeState("idleDown");
                    attacking = false;
                    break;
                case "slashLeft":
                    controler.ChangeState("idleLeft");
                    attacking = false;
                    break;
                case "slashRight":
                    controler.ChangeState("idleRight");
                    attacking = false;
                    break;
            }
        }

        public override void UnloadContent()
        {
            content.Unload();
            content.Dispose();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            box.DrawElements();
            inventory2.DrawElements();
            inventory.DrawElements();
            status.DrawElements();
            choicebox.DrawElements();
            fbox.DrawElements();
            spriteBatch.Begin();
            controler.Draw(spriteBatch,position);
            status.Draw(spriteBatch);
            inventory2.Draw(spriteBatch);
            inventory.Draw(spriteBatch);
            box.Draw(spriteBatch);
            fbox.Draw(spriteBatch);
            GameManager.Draw(spriteBatch);
            spriteBatch.Draw(splash,ScreenManager.ScreenCenter,splash.Bounds,Color.White,0,Vector2.Zero, Vector2.One*1.5f,SpriteEffects.None, 0 );
            choicebox.Draw(spriteBatch);
            spriteBatch.End();
            
        }

        public override void Update(GameTime gameTime)
        {
            box.Update(gameTime);
            fbox.Update(gameTime);
            inventory.Update(gameTime);
            inventory2.Update(gameTime);
            choicebox.Update(gameTime);
            if (!attacking && Keys.A.Down())
            {
                controler.ChangeState("Left");
                if (Keys.LeftShift.Down())
                {
                    controler.Fps = 20;
                    position.X -= 2f;
                }
                else
                {
                    controler.Fps = 10;
                    position.X -= 1f;
                }
            }
            else if(!attacking && Keys.A.DownLast())
            {
                controler.ChangeState("idleLeft");
            }
            else if (!attacking && Keys.D.Down())
            {
                controler.ChangeState("Right");
                if (Keys.LeftShift.Down())
                {
                    controler.Fps = 20;
                    position.X += 2f;
                }
                else
                {
                    controler.Fps = 10;
                    position.X += 1f;
                }
            }
            else if (!attacking && Keys.D.DownLast())
            {
                controler.ChangeState("idleRight");
            }
            else if (!attacking && Keys.W.Down())
            {
                position.Y--;
                controler.ChangeState("Up");
            }
            else if (!attacking && Keys.W.DownLast())
            {
                controler.ChangeState("idleUp");
            }
            else if (!attacking && Keys.S.Down())
            {
                position.Y++;
                controler.ChangeState("Down");
            }
            else if (!attacking && Keys.S.DownLast())
            {
                controler.ChangeState("idleDown");
            }

            if (Keys.R.DownOnce())
            {
                string state = controler.CurrentState;
                if (state == "Up" || state == "idleUp")
                {
                    controler.ChangeState("slashUp");
                    attacking = true;
                    return;
                }
                if (state == "Down" || state == "idleDown")
                {
                    controler.ChangeState("slashDown");
                    attacking = true;
                    return;
                }
                if (state == "Left" || state == "idleLeft")
                {
                    controler.ChangeState("slashLeft");
                    attacking = true;
                    return;
                }
                if (state == "Right" || state == "idleRight")
                {
                    controler.ChangeState("slashRight");
                    attacking = true;
                    return;
                }
            }

            if (Keys.Space.DownOnce())
            {
                 status.IsVisible = !status.IsVisible;
            }
            status.Update(gameTime);
            if (Keys.Q.DownOnce())
            {
                box.SetImage(new Sprite(TextureMaker.ClassSplash(ScreenManager.Device, CharacterClass.Archer)),Origin.BottomCenter);
                box.CycleableText = true;
            }


            if (Keys.X.DownOnce())
            {
                foreach (var value in controler.States.Values)
                {
                    value.RemoveLayer(0);
                }
            }
            controler.Update(gameTime);
            GameManager.Update(gameTime);

        }

    }
}
