namespace MonoTinker.Code.Managers
{
    using System;

    using Components.Elements;
    using GameScreens;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public enum Screens
    {
        Splash,Menu,CharacterCreation,TestGround,Other
    }

    public class ScreenManager
    {
        public static Viewport View;
        public static GraphicsDevice Device;

        public static GameWindow Window;
        public static SpriteBatch Batch;
        public static ContentManager Content;
        public static bool ShouldExit;
        public static readonly Random Rng = new Random();

        private Texture2D fadeTexture;
        private float alpha;

        private static Screen currentScreen;
        private static Screen newScreen;
        private static int width, height;
        private static bool increase;
        private static IServiceProvider service;
        private static bool transitioning;
        private static Vector2 globalScale;

        public ScreenManager(ContentManager content,GameWindow window,GraphicsDevice gdevice)
        {
            Window = window;
            Batch = new SpriteBatch(gdevice);
            width = gdevice.Viewport.Width;
            height = gdevice.Viewport.Height;
            Device = gdevice;
            Content = content;
            service = content.ServiceProvider;
            globalScale = new Vector2(width/640f,height/480f);
            this.fadeTexture = content.Load<Texture2D>("fade");
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

        public static void ChangeScreen(Screens id)
        {
            switch (id)
            {
                case Screens.Menu:
                    newScreen = new MenuScreen(service);
                    break;
                case Screens.CharacterCreation:
                    newScreen = new CharacterCreationScreen(service);
                    break;
                case Screens.Splash:
                    newScreen = new SplashScreen(service);
                    break;
                case Screens.Other:
                    newScreen = new GameScreen(service);
                    break;
                case Screens.TestGround:
                    newScreen = new TestGround(service);
                    break;
            }
            transitioning = true;
            increase = true;
        }

        private void Transition(GameTime gameTime)
        {
            if (increase)
            {
                this.alpha += 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.alpha >= 1f)
                {
                    this.alpha = 1f;
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    increase = false;
                }
            }
            else
            {
                this.alpha -= 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (transitioning && this.alpha < 0)
                {
                    this.alpha = 0;
                    transitioning = false;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            if (transitioning)
            {
                this.Transition(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            currentScreen.Draw(spriteBatch);
            if (transitioning)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(this.fadeTexture,Vector2.Zero,null,null,Vector2.Zero,0f,new Vector2(width,height),Color.White * this.alpha);
                spriteBatch.End();

            }
        }


    }
}
