using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components;

namespace MonoTinker.Code.Managers
{
    public class ScreenManager
    {
        private static Screen currentScreen;
        private static Screen newScreen;
        private int width, height;
        private float Alpha;
        private static bool increase;
        private Texture2D fadeTexture;
        private SpriteBatch batch;
        private static IServiceProvider service;
        private static bool transitioning;

        public ScreenManager(ContentManager content,GraphicsDevice gdevice)
        {
            batch = new SpriteBatch(gdevice);
            width = gdevice.DisplayMode.Width;
            height = gdevice.DisplayMode.Height;
            service = content.ServiceProvider;
            fadeTexture = content.Load<Texture2D>("fade");
            currentScreen = new MenuScreen(service, "Menu");
        }

        public static bool Transitioning
        {
            get { return transitioning; }
        }

        public static void ChangeScreen(string id)
        {
            switch (id)
            {
                case "Menu":
                    newScreen = new MenuScreen(service,"Menu");
                    transitioning = true;
                    increase = true;
                    break;
                case "Other":
                    newScreen = new OtherScreen(service,"Other");
                    transitioning = true;
                    increase = true;
                    break;
            }
        }

        private void Transition(GameTime gameTime)
        {
            if (increase)
            {
                this.Alpha += 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Alpha >= 1f)
                {
                    this.Alpha = 1f;
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    increase = false;
                }
            }
            else
            {
                this.Alpha -= 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (transitioning && Alpha < 0)
                {
                    this.Alpha = 0;
                    transitioning = false;
                }
            }
        }


        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            if (transitioning)
            {
                Transition(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            currentScreen.Draw(spriteBatch);
            if (transitioning)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(fadeTexture,Vector2.Zero,null,null,Vector2.Zero,0f,new Vector2(width,height),Color.White * Alpha);
                spriteBatch.End();

            }
        }


    }
}
