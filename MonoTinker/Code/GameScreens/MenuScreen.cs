

namespace MonoTinker.Code.GameScreens
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Components;
    using Components.Elements;
    using Managers;
    using Components.Extensions;
    using Utils;

    public sealed class MenuScreen : Screen
    {

        private Sprite logo, logoGlow, play, options, exit, bigGear, smallGear, belt,leds;
        private SpriteAtlas atlas;
        private Random rnd;
        private double time;
        private float rotation;
        private bool reverse;
        private byte counter;

        private int index;

        private int Index
        {
            get { return index; }
            set
            {
                if (value < 1 || value > 3)
                {
                    return;
                }
                this.index = value;
            }
        }

        public MenuScreen(IServiceProvider service) : base(service, "Menu")
        {
            rnd = new Random(DateTime.Now.Minute);
            atlas = new SpriteAtlas();
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            atlas.PopulateFromSpriteSheet(content,"menu");

            foreach (var spriteAtla in atlas)
            {
                Console.WriteLine(spriteAtla.Key);
            }

            logo = atlas["text"];
            logo.Transform.Position = ((ScreenManager.ScreenCenter) - Vector2.UnitY * 100) + Vector2.UnitX*50;

            logoGlow = atlas["textGlow"];
            logoGlow.Transform.Position = logo.Transform.Position - new Vector2(45, -10);
            logoGlow.Clr = Color.OrangeRed;

            play = atlas["play"];
            play.Transform.Position = ScreenManager.ScreenCenter + Vector2.UnitY * 35;
            play.Transform.Scale = Vector2.One*0.4f;

            options = atlas["options"];
            options.Transform.Position = play.Transform.Position + Vector2.UnitY * 100 - Vector2.UnitX * 15;
            options.Transform.Scale = Vector2.One * 0.4f;

            exit = atlas["exit"];
            exit.Transform.Position = options.Transform.Position + Vector2.UnitY * 70 + Vector2.UnitX * 15;
            exit.Transform.Scale = Vector2.One * 0.5f;

            bigGear = atlas["bigGear"];
            bigGear.Transform.Position = Vector2.One * 100;
            bigGear.OriginCustom = bigGear.Center + new Vector2(-1,2);

            smallGear = atlas["smallGear"];
            smallGear.Transform.Position = bigGear.Transform.Position + Vector2.UnitY * 105 - Vector2.UnitX * 20;

            leds = atlas["leds"];
            leds.Transform.Position = logo.Transform.Position + Vector2.UnitX * logo.Size.X/2f;
            leds.Transform.Scale = Vector2.One * 0.7f;

            belt = atlas["belt"];
            belt.Origin = Origin.Center;
            belt.Transform.Position = bigGear.Transform.Position + new Vector2(-10,44);
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            Input(gameTime);
            time += gameTime.ElapsedGameTime.TotalSeconds;
            rotation += reverse ? (float)gameTime.ElapsedGameTime.TotalSeconds : -(float)gameTime.ElapsedGameTime.TotalSeconds;
            counter++;
            if (time > TimeSpan.FromSeconds(rnd.NextDouble() + rnd.Next(1,6)).TotalSeconds)
            {
                if (counter >= 200)
                {
                    reverse = !reverse;
                }
                time = 0;
            }
            MenuIndexCheck();
            bigGear.Transform.Rotation = rotation;
            smallGear.Transform.Rotation = rotation;
        }

        public void Input(GameTime gameTime)
        {
            if (Keys.Enter.DownOnce())
            {
                switch (index)
                {
                    case 1:
                        ScreenManager.ChangeScreen("Other");
                        break;
                    case 2:
                        ScreenManager.ChangeScreen("CharacterCreation");
                        break;
                    case 3:
                        this.UnloadContent();
                        ScreenManager.ShouldExit = true;
                        break;
                }
            }
            if (Keys.Tab.Down() && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen("Other");
            }
            if (Keys.Q.Down())
            {
                play.Transform.Scale(0.1f);
            }
            if (InputHandler.DirectionDownOnce("up"))
            {
                Index--;
            }
            if (InputHandler.DirectionDownOnce("down"))
            {
                Index++;
            }
        }


        public void MenuIndexCheck()
        {
            play.Transform.Scale = Index == 1
                ? Vector2.One * (MathHelper.SmoothStep(play.Transform.Scale.X, 0.5f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(play.Transform.Scale.X, 0.4f, 0.1f));
            options.Transform.Scale = Index == 2
                ? Vector2.One * (MathHelper.SmoothStep(options.Transform.Scale.X, 0.5f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(options.Transform.Scale.X, 0.4f, 0.1f));
            exit.Transform.Scale = Index == 3
                ? Vector2.One * (MathHelper.SmoothStep(exit.Transform.Scale.X, 0.6f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(exit.Transform.Scale.X, 0.5f, 0.1f));

            play.Clr = Index == 1 ? ColorHelper.SmoothTransition(play.Clr, Color.OrangeRed, 0.02f) 
                                    : ColorHelper.SmoothTransition(play.Clr, Color.White, 0.02f);
            options.Clr = Index == 2 ? ColorHelper.SmoothTransition(options.Clr, Color.OrangeRed, 0.02f)
                                    : ColorHelper.SmoothTransition(options.Clr, Color.White, 0.02f);
            exit.Clr = Index == 3 ? ColorHelper.SmoothTransition(exit.Clr, Color.OrangeRed, 0.02f)
                                    : ColorHelper.SmoothTransition(exit.Clr, Color.White, 0.02f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            logo.Draw(spriteBatch);
            if ((time >= TimeSpan.FromSeconds(rnd.Next(1, 2) + rnd.NextDouble()).TotalSeconds) &&
                (time <= TimeSpan.FromSeconds(rnd.Next(2, 2) + rnd.NextDouble()).TotalSeconds) ||
                (time >= TimeSpan.FromSeconds(rnd.Next(2, 3) + rnd.NextDouble()).TotalSeconds) &&
                (time <= TimeSpan.FromSeconds(rnd.Next(3, 4) + rnd.NextDouble()).TotalSeconds))
            {
                logoGlow.Draw(spriteBatch);
            }

            bigGear.Draw(spriteBatch);
            smallGear.Draw(spriteBatch);
            play.Draw(spriteBatch);
            options.Draw(spriteBatch);
            exit.Draw(spriteBatch);
            leds.Draw(spriteBatch);
            belt.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
