using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Components.Extensions;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Game
{
    public sealed class MenuScreen : Screen
    {

        private Sprite logo, logoGlow, play, options, bigGear, smallGear, belt,leds;

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
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            logo = new Sprite(content.Load<Texture2D>("text"))
            {
                Transform = {Position = ((ScreenManager.ScreenCenter) - Vector2.UnitY*100) + Vector2.UnitX*50}
            };
            logoGlow = new Sprite(content.Load<Texture2D>("textGlow"));
            logoGlow.Transform.Position = logo.Transform.Position - new Vector2(45,-10);
            logoGlow.Color = Color.OrangeRed;

            play = new Sprite(content.Load<Texture2D>("play"));
            play.Transform.Position = ScreenManager.ScreenCenter + Vector2.UnitY*35;
            play.Transform.Scale = Vector2.One * 0.4f;

            options = new Sprite(content.Load<Texture2D>("options"));
            options.Transform.Position = play.Transform.Position + Vector2.UnitY*100 - Vector2.UnitX*15;
            options.Transform.Scale = play.Transform.Scale;

            bigGear = new Sprite(content.Load<Texture2D>("bigGear"));
            bigGear.Transform.Position = Vector2.One*100;
            bigGear.OriginCustom = bigGear.Center + new Vector2(-1,3);

            leds = new Sprite(content.Load<Texture2D>("leds"));
            leds.Transform.Position = logo.Transform.Position + new Vector2(logo.Source.Right/2f, 0);
            leds.Transform.Scale = Vector2.One * 0.7f;

            smallGear = new Sprite(content.Load<Texture2D>("smallGear"));
            smallGear.Transform.Position = bigGear.Transform.Position + Vector2.UnitY*105 - Vector2.UnitX*20;

            belt = new Sprite(content.Load<Texture2D>("belt"));
            belt.Origin = Origin.Center;
            belt.Transform.Position = bigGear.Transform.Position + new Vector2(-10,44);
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(counter++);
            time += gameTime.ElapsedGameTime.TotalSeconds;
            rotation += reverse ? (float)gameTime.ElapsedGameTime.TotalSeconds : -(float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > TimeSpan.FromSeconds(rnd.NextDouble() + rnd.Next(1,6)).TotalSeconds)
            {
                if (counter >= 200)
                {
                    reverse = !reverse;
                }
                time = 0;
            }
            if (Keys.Tab.Down() && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen("Other");
            }

            if (Keys.Q.Down())
            {
                play.Transform.Scale(0.1f);
            }

            if (InputHandler.IsKeyDown(Keys.Up))
            {
                Index--;
            }
            if (InputHandler.IsKeyDown(Keys.Down))
            {
                Index++;
            }
            MenuIndexChange();
            Console.WriteLine(index);
            bigGear.Transform.Rotation = rotation;
            smallGear.Transform.Rotation = rotation;
        }

        public void MenuIndexChange()
        {
            play.Transform.Scale = Index == 1
                ? Vector2.One * (MathHelper.SmoothStep(play.Transform.Scale.X, 0.5f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(play.Transform.Scale.X, 0.4f, 0.1f));
            options.Transform.Scale = Index == 2
                ? Vector2.One * (MathHelper.SmoothStep(options.Transform.Scale.X, 0.5f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(options.Transform.Scale.X, 0.4f, 0.1f));
            play.Color = Index == 1 ? ColorHelper.SmoothTransition(play.Color, Color.OrangeRed, 0.02f) 
                                    : ColorHelper.SmoothTransition(play.Color, Color.White, 0.02f);
            options.Color = Index == 2 ? ColorHelper.SmoothTransition(options.Color, Color.OrangeRed, 0.02f)
                                    : ColorHelper.SmoothTransition(options.Color, Color.White, 0.02f);
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
            leds.Draw(spriteBatch);
            belt.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
