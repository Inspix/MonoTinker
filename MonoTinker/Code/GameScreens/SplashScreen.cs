
namespace MonoTinker.Code.GameScreens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Components.Elements;
    using Managers;

    public sealed class SplashScreen : Screen
    {
        private SpriteFont font;
        private Texture2D logo;
        private Dictionary<string, Vector2> textCenters;
        private const string name = "Inspix";
        private const string presents = "Presents";
        private const string production = "an Indie production";
        private float increment;
        private double timer;

        private bool[] finished;
        private bool[] triggers;
        private float[] alphas;
        private int index;

        public SplashScreen(IServiceProvider service) : base(service, "SplashScreen")
        {
            this.textCenters = new Dictionary<string, Vector2>();
            this.increment = 1f;
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            font = content.Load<SpriteFont>("SplashFont");
            logo = content.Load<Texture2D>("logo");
            textCenters.Add(name, font.MeasureString(name)/2f);
            textCenters.Add(presents,font.MeasureString(presents)/2f);
            textCenters.Add(production, font.MeasureString(production)/2f);
            triggers = new bool[3];
            alphas = new float[4];
            finished = new bool[3];
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            if (!finished[0])
            {
                if (!triggers[index])
                {
                    if (index == 2)
                    {
                        increment = 0.25f;
                    }
                    alphas[index] += increment * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (alphas[index] > 1f)
                {
                    triggers[index] = true;
                    if (index + 1 < triggers.Length)
                    {
                        index++;
                    }
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (timer > TimeSpan.FromSeconds(3).TotalSeconds)
                {
                    timer = 0;
                    finished[0] = true;
                }
            }
            if (!finished[1] && finished[0])
            {
                for (int i = 0; i < alphas.Length-1; i++)
                {
                    alphas[i] -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (alphas[i] <= 0) triggers[i] = false;
                }
                finished[1] = triggers.All(s => s == false);
            }
            if (!finished[2] && finished[1])
            {
                alphas[3] += 1f*(float) gameTime.ElapsedGameTime.TotalSeconds;
                if (alphas[3] > 1f)
                {
                    alphas[3] = 1f;
                }
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer > TimeSpan.FromSeconds(4).TotalSeconds)
                {
                    finished[2] = true;
                    timer = 0;
                }
            }
            else if(finished[2])
            {
                alphas[3] -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (alphas[3]< 0f)
                {
                    ScreenManager.ChangeScreen(Screens.Menu);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if(!finished[1])
                spriteBatch.DrawString(font, name, ScreenManager.ScreenDimensions / 2f - Vector2.UnitY * 100, 
                    Color.AntiqueWhite * alphas[0],0, textCenters[name],Vector2.One, SpriteEffects.None, 0);
            if (triggers[0])
                spriteBatch.DrawString(font,presents, ScreenManager.ScreenDimensions / 2f,
                    Color.AntiqueWhite * alphas[1],0f, textCenters[presents], 0.35f,SpriteEffects.None,0);
            if (triggers[1])
                spriteBatch.DrawString(font,production, ScreenManager.ScreenDimensions / 2f + Vector2.UnitY*200,
                    Color.AntiqueWhite * alphas[2],0f, textCenters[production], 0.20f,SpriteEffects.None,0);
            if (finished[0] && finished[1])
            {
                spriteBatch.Draw(logo, ScreenManager.ScreenDimensions/2f, logo.Bounds, Color.Aquamarine*alphas[3], 0,
                    new Vector2(logo.Width/2f, logo.Height/2f), Vector2.One * 0.5f, SpriteEffects.None, 0);
            }
            spriteBatch.End();
        }
    }
}
