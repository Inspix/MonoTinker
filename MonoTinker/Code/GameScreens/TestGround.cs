

using MonoTinker.Code.Components.GameComponents;

namespace MonoTinker.Code.GameScreens
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Components;
    using Components.Elements;
    using Components.Extensions;
    using Managers;
    using Utils;
    using Components.UI;

    public sealed class TestGround : Screen
    {
        private SpriteAtlas atlas;
        private AnimationController controler;

        private StatusBar status;

        private TextBox box;
        private FancyTextBox fbox;

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
            status = new StatusBar(Vector2.One, ScreenManager.Device);
            status.DefaultAlpha = 1;
            status.Transitioning = true;
            status.FadeSpeed = 0.01f;
            inventory = new Inventory(Vector2.One*200, ScreenManager.Device,5);
            inventory.FadeSpeed = 0.01f;
            inventory2 = new Inventory(Vector2.One * 300, ScreenManager.Device, 4);
            controler = new AnimationController();
            atlas = new SpriteAtlas();
            Item x = Factory.CreateItem("Ashbringer", 50, 25, 50, 10, 10, ItemRarity.Legendary);
            Item y = Factory.CreateItem("Staff of Regrowth", 10, 5, 35, 50, 50, ItemRarity.Epic);
            fbox = new FancyTextBox(Vector2.UnitY* 300,ScreenManager.Device, @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",15,WriteEffect.CharacterByCharacter); 


            ItemTile item = new ItemTile(AssetManager.Instance.Get<Sprite>(Sn.Items.Ashbringer).DirectClone(),Vector2.One * 400,x);
            ItemTile item2 = new ItemTile(AssetManager.Instance.Get<Sprite>(Sn.Items.StaffOfRegrowth).DirectClone(), Vector2.One * 400, y);
            box = new TextBox(Vector2.One * 100,ScreenManager.Device, "Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool!", new Vector2(5,2),WriteEffect.CharacterByCharacter);
            
            controler = AssetManager.Instance.GetBaseWalkingController(An.Walk.BodyHuman);
            controler.IsVisible = true;
            foreach (var animationV2 in controler.States)
            {
                Console.WriteLine(animationV2.Key);
            }

            var slashUp = new AnimationV2(AssetManager.Instance.Get<Animation>(An.Slash.BodyHuman + "slashUp"));
            slashUp.AddLayer(AssetManager.Instance.Get<Animation>(An.Slash.WeaponLongSword + "slashUp"),"slashup");
            slashUp.ChangeLayerOffset(1,Vector2.One * -64);
            slashUp.Looping = false;
            /*var slashLeft = Factory.CreateAnimationWithLayers(atlas, slashLayers, 6, 6, "slashLeft", 10);
           
            slashLeft.Looping = false;
            var slashDown = Factory.CreateAnimationWithLayers(atlas, slashLayers, 12, 6, "slashDown", 10);
            
            slashDown.Looping = false;
            var slashRight = Factory.CreateAnimationWithLayers(atlas, slashLayers, 18, 6, "slashRight", 10);*/
            
            //slashRight.Looping = false;
            
            inventory.AddItemToSlot(item,1);
            inventory.AddItemToSlot(item2, 10);
            inventory2.AddItemToSlot(item,5);
            inventory2.AddItemToSlot(item2,3);
            controler.AddState("slashUp",slashUp);
            //controler.AddState("slashLeft", slashLeft);
            //controler.AddState("slashDown", slashDown);
            //controler.AddState("slashRight", slashRight);
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
            fbox.DrawElements();
            spriteBatch.Begin();
            controler.Draw(spriteBatch,position);
            status.Draw(spriteBatch);
            inventory2.Draw(spriteBatch);
            inventory.Draw(spriteBatch);
            box.Draw(spriteBatch);
            fbox.Draw(spriteBatch);
            GameManager.Draw(spriteBatch);
            spriteBatch.End();
            
        }

        public override void Update(GameTime gameTime)
        {
            box.Update(gameTime);
            fbox.Update(gameTime);
            inventory.Update(gameTime);
            inventory2.Update(gameTime);
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
                }
                if (state == "Down" || state == "idleDown")
                {
                    controler.ChangeState("slashDown");
                    attacking = true;
                }
                if (state == "Left" || state == "idleLeft")
                {
                    controler.ChangeState("slashLeft");
                    attacking = true;
                }
                if (state == "Right" || state == "idleRight")
                {
                    controler.ChangeState("slashRight");
                    attacking = true;
                }
            }

            if (Keys.Space.DownOnce())
            {
                status.IsVisible = !status.IsVisible;
            }
            status.Update(gameTime);
            if (Keys.Q.DownOnce())
            {
                int r = ScreenManager.Rng.Next(0, 255);
                int g = ScreenManager.Rng.Next(0, 255);
                int b = ScreenManager.Rng.Next(0, 255);
                foreach (var value in controler.States.Values)
                {
                    value.Layer(1).Tint = Color.FromNonPremultiplied(r, g, b, 255);
                }
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
