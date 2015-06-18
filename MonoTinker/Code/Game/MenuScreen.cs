using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Managers;

namespace MonoTinker.Code.Game
{
    public sealed class MenuScreen : Screen
    {
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, Sprite> sprites; 
        private Random rnd;
        private double time;
        private float rotation;
        private bool reverse;
        private byte counter;

        public MenuScreen(IServiceProvider service) : base(service, "Menu")
        {
            textures = new Dictionary<string, Texture2D>();
            sprites = new Dictionary<string, Sprite>();

            rnd = new Random(DateTime.Now.Minute);
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            //textures.Add("Text",content.Load<Texture2D>("text"));
            sprites.Add("Text", new Sprite(content.Load<Texture2D>("text")));
            sprites.Add("TextGlow",new Sprite(content.Load<Texture2D>("textGlow")));
            sprites.Add("BigGear",new Sprite(content.Load<Texture2D>("bigGear")));
            textures.Add("Leds", content.Load<Texture2D>("leds"));
            textures.Add("SmallGear", content.Load<Texture2D>("smallGear"));
            textures.Add("Belt", content.Load<Texture2D>("belt"));
            Sprite play = new Sprite(content.Load<Texture2D>("play"));
            play.Scale = Vector2.One*0.5f;
            sprites.Add("Play", play);

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
            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen("Other");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                sprites["Play"].Scale += Vector2.One*0.1f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            /*spriteBatch.Draw(textures["Text"],((ScreenManager.ScreenDimensions/2f) - Vector2.UnitY*100) + Vector2.UnitX*50,
                textures["Text"].Bounds,Color.White,0,
                new Vector2(textures["Text"].Width/2f, textures["Text"].Height / 2f),
                Vector2.One, SpriteEffects.None, 0 );*/
            spriteBatch.Draw(sprites["Text"].Texture,
                ((ScreenManager.ScreenCenter) - Vector2.UnitY*100) + Vector2.UnitX * 50,
                sprites["Text"].Source, Color.White, 0,
                sprites["Text"].Center,
                sprites["Text"].Scale, SpriteEffects.None, 0);
            if ((time >= TimeSpan.FromSeconds(rnd.Next(1, 2) + rnd.NextDouble()).TotalSeconds) &&
                (time <= TimeSpan.FromSeconds(rnd.Next(2, 2) + rnd.NextDouble()).TotalSeconds) ||
                (time >= TimeSpan.FromSeconds(rnd.Next(2, 3) + rnd.NextDouble()).TotalSeconds) &&
                (time <= TimeSpan.FromSeconds(rnd.Next(3, 4) + rnd.NextDouble()).TotalSeconds))
           {
                /*spriteBatch.Draw(textures["TextGlow"], ((ScreenManager.ScreenDimensions / 2f) - Vector2.UnitY * 100) + Vector2.UnitX * 50,
                    textures["TextGlow"].Bounds, Color.White*(float) rnd.NextDouble(), 0,
                    new Vector2((textures["TextGlow"].Width + 95f)/2f, (textures["TextGlow"].Height - 20)/2f),
                    Vector2.One, SpriteEffects.None, 0);*/
               spriteBatch.Draw(sprites["TextGlow"].Texture,
                   (ScreenManager.ScreenCenter - Vector2.UnitY*100) + Vector2.UnitX*50,
                   sprites["TextGlow"].Source, Color.White*(float) rnd.NextDouble(), 0,
                   sprites["TextGlow"].Center + new Vector2(45, -10),
                   sprites["TextGlow"].Scale, SpriteEffects.None, 0);
           }

            spriteBatch.Draw(sprites["BigGear"].Texture,Vector2.One * 50,sprites["BigGear"].Source,Color.White,rotation,sprites["BigGear"].Center, sprites["BigGear"].Scale, SpriteEffects.None,0 );
            spriteBatch.Draw(sprites["Play"].Texture,ScreenManager.ScreenCenter + Vector2.UnitY * 35, sprites["Play"].Source,Color.White,0, sprites["Play"].Center, sprites["Play"].Scale,SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
