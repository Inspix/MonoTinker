using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Game;
using MonoTinker.Code.Components;

namespace MonoTinker.Code.Managers
{
    public class ScreenManager
    {
        private static Screen currentScreen;
        private static Screen newScreen;
        public static bool ShouldExit;
        private static int width, height;
        private float Alpha;
        private static bool increase;
        private Texture2D fadeTexture;
        private static IServiceProvider service;
        public static Viewport view;
        public static GraphicsDevice device;
        private static bool transitioning;
        private static Vector2 globalScale;

        public ScreenManager(ContentManager content,GraphicsDevice gdevice)
        {
            width = gdevice.Viewport.Width;
            height = gdevice.Viewport.Height;
            device = gdevice;
            service = content.ServiceProvider;
            globalScale = new Vector2(width/640f,height/480f);
            fadeTexture = content.Load<Texture2D>("fade");
            currentScreen = new MenuScreen(service);
        }

        public static Vector2 GlobalScale
        {
            get { return globalScale; }
        }

        public static bool Transitioning
        {
            get { return transitioning; }
        }

        public static Vector2 ScreenDimensions
        {
            get { return new Vector2(width,height);}
        }

        public static Vector2 ScreenCenter
        {
            get { return new Vector2(width/2f, height/2f); }
        }

        public static void ChangeScreen(string id)
        {
            switch (id)
            {
                case "Menu":
                    newScreen = new MenuScreen(service);
                    transitioning = true;
                    increase = true;
                    break;
                case "Other":
                    newScreen = new OtherScreen(service);
                    transitioning = true;
                    increase = true;
                    break;
                case "Splash":
                    newScreen = new SplashScreen(service);
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
