using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code;
using MonoTinker.Code.Components;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MonoTinker : Game
    {
        public readonly int Widht;
        public readonly int Height;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static bool ShouldExit;
        private ScreenManager sm;
        public Vector2 CameraLookAt;

        public MonoTinker(int width,int height)
        {
            Widht = width;
            Height = height;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.Position = new Point(GraphicsDevice.DisplayMode.Width / 2 - Widht / 2,GraphicsDevice.DisplayMode.Height / 2 - Height / 2);
            Window.Title = "MonoTinker";
            IsMouseVisible = true;
            sm = new ScreenManager(Content,Window,GraphicsDevice);
            ScreenManager.View = graphics.GraphicsDevice.Viewport;
            AssetManager.Instance.LoadContent(Content);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (ScreenManager.ShouldExit)
            {
                Exit();
            }
            InputHandler.Update(Window);
            sm.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            sm.Draw(spriteBatch);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
