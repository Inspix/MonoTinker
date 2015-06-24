using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.GameScreens;

namespace MonoTinker.Code.Managers
{
    public class ScreenManager
    {
        public static Viewport view;
        public static GraphicsDevice device;
        public static bool ShouldExit;
        public static readonly Random rng = new Random();

        private Texture2D fadeTexture;
        private float Alpha;

        private static Screen currentScreen;
        private static Screen newScreen;
        private static int width, height;
        private static bool increase;
        private static IServiceProvider service;
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
                    break;
                case "Other":
                    newScreen = new OtherScreen(service);
                    break;
                case "Splash":
                    newScreen = new SplashScreen(service);
                    break;
                case "Game":
                    newScreen = new GameScreen(service);
                    break;
                case "CharacterCreation":
                    newScreen = new CharacterCreationScreen(service);
                    break;
            }
            transitioning = true;
            increase = true;
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
