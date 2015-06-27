

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

    public sealed class CharacterCreationScreen : Screen
    {
        private SpriteAtlas atlas;
        private AnimationController controler;

        private StatusBar status;

        private TextBox box;

        private Inventory inventory;
        private Inventory inventory2;

        private ItemTile item;
        private bool attacking;
        private Vector2 position;

        public CharacterCreationScreen(IServiceProvider service) : base(service, "CharacterCreation")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            status = new StatusBar(Vector2.One, ScreenManager.Device);
            status.DefaultAlpha = 255;
            status.Transitioning = true;
            status.FadeSpeed = 10;
            inventory = new Inventory(Vector2.One*200, ScreenManager.Device,5);
            inventory.FadeSpeed = 10;
            inventory2 = new Inventory(Vector2.One * 300, ScreenManager.Device, 4);
            controler = new AnimationController();
            atlas = new SpriteAtlas();
            item = new ItemTile(AssetManager.Instance.Get<Sprite>(SpriteNames.GearSilver).DirectClone(),Vector2.One * 400,"Test Item of the Cool", "Awesome");


            box = new TextBox(Vector2.One * 100,ScreenManager.Device, "Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool! Once upon a time, there was a kid nameed Goshko..... He was very young and cool!", new Vector2(5,2));

            List<string[]> layers = new List<string[]>();
            string[] bodyAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Walking/BODY_male"), new Vector2(64, 64), "body");
            string[] hairAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Walking/HEAD_hair_blonde"), new Vector2(64, 64), "hair");
            string[] torsoAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Walking/TORSO_leather_armor_torso"),new Vector2(64,64),"torso");
            string[] legsAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Walking/LEGS_pants_greenish"),new Vector2(64, 64), "legs");
            layers.Add(bodyAnim);
            layers.Add(hairAnim);
            layers.Add(torsoAnim);
            layers.Add(legsAnim);
            var idleUp = Factory.CreateAnimationWithLayers(atlas, layers, 0, 1, 10);
            var idleDown = Factory.CreateAnimationWithLayers(atlas, layers, 18, 1, 10);
            var idleLeft = Factory.CreateAnimationWithLayers(atlas, layers, 9, 1, 10);
            var idleRight = Factory.CreateAnimationWithLayers(atlas, layers, 27, 1, 10);
            var walkUp = Factory.CreateAnimationWithLayers(atlas, layers, 1, 8, 10);
            var walkDown = Factory.CreateAnimationWithLayers(atlas, layers, 19, 8, 10);
            var walkLeft = Factory.CreateAnimationWithLayers(atlas, layers, 10, 8, 10);
            var walkRight = Factory.CreateAnimationWithLayers(atlas, layers, 28, 8, 10);

            List<string[]> slashLayers = new List<string[]>();
            string[] slashbodyAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Slash/BODY_male"), new Vector2(64, 64), "slashbody");
            string[] slashhairAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Slash/HEAD_hair_blonde"), new Vector2(64, 64), "slashhair");
            string[] slashtorsoAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Slash/TORSO_leather_armor_torso"), new Vector2(64, 64), "slashtorso");
            string[] slashlegsAnim = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Slash/LEGS_pants_greenish"), new Vector2(64, 64), "slashlegs");
            string[] slashRapier = atlas.PopulateFromSpriteSheet(content.Load<Texture2D>("Slash/WEAPON_rapier"),
                new Vector2(192, 192), "slashRapier");
            slashLayers.Add(slashbodyAnim);
            slashLayers.Add(slashhairAnim);
            slashLayers.Add(slashtorsoAnim);
            slashLayers.Add(slashlegsAnim);
            slashLayers.Add(slashRapier);
            var slashUp = Factory.CreateAnimationWithLayers(atlas, slashLayers, 0, 6,10);
            slashUp.ChangeLayerOffset(4,Vector2.One * -64);
            slashUp.Looping = false;
            var slashLeft = Factory.CreateAnimationWithLayers(atlas, slashLayers, 6, 6, 10);
            slashLeft.ChangeLayerOffset(4, Vector2.One * -64);
            slashLeft.Looping = false;
            var slashDown = Factory.CreateAnimationWithLayers(atlas, slashLayers, 12, 6, 10);
            slashDown.ChangeLayerOffset(4, Vector2.One * -64);
            slashDown.Looping = false;
            var slashRight = Factory.CreateAnimationWithLayers(atlas, slashLayers, 18, 6, 10);
            slashRight.ChangeLayerOffset(4, Vector2.One * -64);
            slashRight.Looping = false;
            
            controler.AddState("idleUp", idleUp);
            controler.AddState("idleLeft", idleLeft);
            controler.AddState("idleDown", idleDown);
            controler.AddState("idleRight", idleRight);
            controler.AddState("walkUp", walkUp);
            controler.AddState("walkLeft", walkLeft);
            controler.AddState("walkDown", walkDown);
            controler.AddState("walkRight", walkRight);
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
            spriteBatch.Begin();
            controler.Draw(spriteBatch,position);
            status.Draw(spriteBatch);
            inventory2.Draw(spriteBatch);
            inventory.Draw(spriteBatch);
            box.Draw(spriteBatch);
            item.Draw(spriteBatch);
            spriteBatch.End();
            
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(InputHandler.MouseDelta());
            item.Over(InputHandler.MousePos());
            item.Update(gameTime);
            box.Update(gameTime);
            inventory.Update(gameTime);
            inventory2.Update(gameTime);
            if (!attacking && Keys.A.Down())
            {
                controler.ChangeState("walkLeft");
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
                controler.ChangeState("walkRight");
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
                controler.ChangeState("walkUp");
            }
            else if (!attacking && Keys.W.DownLast())
            {
                controler.ChangeState("idleUp");
            }
            else if (!attacking && Keys.S.Down())
            {
                position.Y++;
                controler.ChangeState("walkDown");
            }
            else if (!attacking && Keys.S.DownLast())
            {
                controler.ChangeState("idleDown");
            }

            if (Keys.R.DownOnce())
            {
                string state = controler.CurrentState;
                if (state == "walkUp" || state == "idleUp")
                {
                    controler.ChangeState("slashUp");
                    attacking = true;
                }
                if (state == "walkDown" || state == "idleDown")
                {
                    controler.ChangeState("slashDown");
                    attacking = true;
                }
                if (state == "walkLeft" || state == "idleLeft")
                {
                    controler.ChangeState("slashLeft");
                    attacking = true;
                }
                if (state == "walkRight" || state == "idleRight")
                {
                    controler.ChangeState("slashRight");
                    attacking = true;
                }
            }

            if (Keys.Space.DownOnce())
            {
                int r = ScreenManager.Rng.Next(0, 255);
                int g = ScreenManager.Rng.Next(0, 255);
                int b = ScreenManager.Rng.Next(0, 255);
                foreach (var value in controler.States.Values)
                {
                    value.Layer(1).Tint = Color.FromNonPremultiplied(r, g, b, 255);
                    
                }

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
        }

    }
}
