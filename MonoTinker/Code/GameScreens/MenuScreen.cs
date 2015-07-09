
namespace MonoTinker.Code.GameScreens
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;


    using Components;
    using Components.Elements;
    using Components.Elements.DebugGraphics;
    using Components.Extensions;
    using Components.Tiles;
    
    using Managers;
    using Utils;

    public sealed class MenuScreen : Screen
    {

        private Sprite logo, logoGlow, play, options, exit, bigGear, smallGear, belt, leds;
        private SpriteAtlas atlas;
        private RenderTarget2D lightmask;
        private RenderTarget2D maintarget;
        private Effect effect;
        private TileMap map;
        private Matrix camera;
        private Random rnd;
        private Screens choice;
        private Vector3 campos;
        private Vector3 camscale;
        private double time;
        private float rotation;
        private bool reverse;
        private bool reverseCamera;
        private int index;
        private byte counter;

        public MenuScreen(IServiceProvider service) : base(service, "Menu")
        {
            rnd = new Random(DateTime.Now.Minute);
            atlas = new SpriteAtlas();
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            lightmask = new RenderTarget2D(ScreenManager.Device,(int)ScreenManager.ScreenDimensions.X,(int)ScreenManager.ScreenDimensions.Y);
            maintarget = new RenderTarget2D(ScreenManager.Device, (int)ScreenManager.ScreenDimensions.X, (int)ScreenManager.ScreenDimensions.Y);
            effect = AssetManager.Instance.Get<Effect>("Lightmask");
            camera = Matrix.CreateScale(1, 1, 0);
            campos = new Vector3(0,-18,0);
            map = new TileMap();
            map.LoadFromTiledJsonFile(ref atlas,ScreenManager.Content,"/Game/test.json");
            map.LightTiles[1].ScaleF = 0.4f;
            map.LightTiles[1].Clr = Color.White.Alpha(100);
            map.LightTiles[1].Effect = LightSimpleEffect.Shimmering;
            map.LightTiles[1].Position -= Vector2.One* 5;
            atlas.PopulateFromSpriteSheet(content,"menu");
            camscale = Vector3.One;
            logo = atlas["text"];
            logo.Position = ((ScreenManager.ScreenCenter) - Vector2.UnitY * 100) + Vector2.UnitX*50;

            logoGlow = atlas["textGlow"];
            logoGlow.Position = logo.Position - new Vector2(45, -10);
            logoGlow.Clr = Color.OrangeRed;

            play = atlas["play"];
            play.Position = ScreenManager.ScreenCenter + Vector2.UnitY * 35;
            play.Scale = Vector2.One*0.4f;

            options = atlas["options"];
            options.Position = play.Position + Vector2.UnitY * 100 - Vector2.UnitX * 15;
            options.Scale = Vector2.One * 0.4f;

            exit = atlas["exit"];
            exit.Position = options.Position + Vector2.UnitY * 70 + Vector2.UnitX * 15;
            exit.Scale = Vector2.One * 0.5f;

            bigGear = atlas["bigGear"];
            bigGear.Position = Vector2.One * 100;
            bigGear.OriginCustom = bigGear.SpriteCenter + new Vector2(-1,2);

            smallGear = atlas["smallGear"];
            smallGear.Position = bigGear.Position + Vector2.UnitY * 105 - Vector2.UnitX * 20;

            leds = atlas["leds"];
            leds.Position = logo.Position + Vector2.UnitX * logo.Size.X/2f;
            leds.Scale = Vector2.One * 0.7f;

            belt = atlas["belt"];
            belt.Origin = Origin.Center;
            belt.Position = bigGear.Position + new Vector2(-10,44);
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public bool Transition { get; set; }

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
           
            foreach (var lightTile in map.LightTiles)
            {
                lightTile.Update(gameTime);
            }
            MenuIndexCheck();
            bigGear.Rotation = rotation;
            smallGear.Rotation = rotation;
            CameraMovement();

        }

        public void Input(GameTime gameTime)
        {
            if (Keys.Enter.DownOnce())
            {
                switch (index)
                {
                    case 1:
                        Transition = true;
                        choice = Screens.CharacterCreation;
                        break;
                    case 2:
                        ScreenManager.ChangeScreen(Screens.TestGround);
                        break;
                    case 3:
                        this.UnloadContent();
                        ScreenManager.ShouldExit = true;
                        break;
                }
            }
            if (Keys.Tab.Down() && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen(Screens.Other);
            }
            if (Keys.Q.Down())
            {
                play.ScaleF += 0.1f;
            }
            campos += new Vector3(Keys.NumPad1.Down() ? 10 : 0,Keys.NumPad2.Down()?10:0,0);
            if (InputHandler.DirectionDownOnce("up"))
            {
                Index--;
            }
            if (InputHandler.DirectionDownOnce("down"))
            {
                Index++;
            }
        }

        private void CameraMovement()
        {
            if (ScreenManager.Transitioning)
            {
                camscale = new Vector3(MathHelper.Lerp(camscale.X, 10, 0.01f), MathHelper.Lerp(camscale.Y, 10, 0.01f), 0);
                camera.Scale = camscale;
                campos = new Vector3(MathHelper.SmoothStep(campos.X, 1880, .1f), MathHelper.SmoothStep(campos.Y, 640, .1f), 0);

                camera.Translation = -campos * camscale;

                return;
            }
            if (Transition)
            {
                camscale = new Vector3(MathHelper.Lerp(camscale.X,5,0.01f), MathHelper.Lerp(camscale.Y, 5, 0.01f),0);
                camera.Scale = camscale;
                campos = new Vector3(MathHelper.Lerp(campos.X,1800,.02f), MathHelper.Lerp(campos.Y, 580, .02f),0);
                if(campos.X * camscale.X >= 1779*camscale.X && campos.Y * camscale.X > 579*camscale.Y) ScreenManager.ChangeScreen(choice);
            }
            else
            {
                if (reverseCamera)
                {
                    campos.X--;

                }
                else
                {
                    campos.X++;
                }
                if (campos.X * camscale.X >= map.Widht* camscale.X - 1050 *  camscale.X || campos.X* camscale.X <= 0)
                {
                    reverseCamera = !reverseCamera;
                }
            }
            Console.WriteLine(campos);
            camera.Translation = -campos * camscale;

        }


        public void MenuIndexCheck()
        {
            play.Scale = Index == 1
                ? Vector2.One * (MathHelper.SmoothStep(play.Scale.X, 0.5f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(play.Scale.X, 0.4f, 0.1f));
            options.Scale = Index == 2
                ? Vector2.One * (MathHelper.SmoothStep(options.Scale.X, 0.5f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(options.Scale.X, 0.4f, 0.1f));
            exit.Scale = Index == 3
                ? Vector2.One * (MathHelper.SmoothStep(exit.Scale.X, 0.6f, 0.1f))
                : Vector2.One * (MathHelper.SmoothStep(exit.Scale.X, 0.5f, 0.1f));

            play.Clr = Index == 1 ? ColorHelper.SmoothTransition(play.Clr, Color.OrangeRed, 0.02f) 
                                    : ColorHelper.SmoothTransition(play.Clr, Color.White, 0.02f);
            options.Clr = Index == 2 ? ColorHelper.SmoothTransition(options.Clr, Color.OrangeRed, 0.02f)
                                    : ColorHelper.SmoothTransition(options.Clr, Color.White, 0.02f);
            exit.Clr = Index == 3 ? ColorHelper.SmoothTransition(exit.Clr, Color.OrangeRed, 0.02f)
                                    : ColorHelper.SmoothTransition(exit.Clr, Color.White, 0.02f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.SetRenderTarget(lightmask);
            spriteBatch.GraphicsDevice.Clear(Color.FromNonPremultiplied(50,50,50,100));
             
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive, null, null, null, null, camera);
            foreach (var lightTile in map.LightTiles)
            {
                lightTile.Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);


            spriteBatch.GraphicsDevice.SetRenderTarget(maintarget);
            spriteBatch.GraphicsDevice.Clear(Color.FromNonPremultiplied(100,100,100,255));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,null,null,null,null,camera);
            foreach (var staticTile in map.StaticTiles)
            {
                staticTile.Draw(spriteBatch);
            }
            DebugShapes.DrawLine(spriteBatch,new Vector2(1941, 693), new Vector2(1943, 693),2f,Color.White * 0.2f);
            DebugShapes.DrawLine(spriteBatch, new Vector2(1945, 693), new Vector2(1947, 693), 2f, Color.White * 0.2f);
            DebugShapes.DrawLine(spriteBatch, new Vector2(1941, 697), new Vector2(1947, 697), 1f, Color.White * 0.2f);

            spriteBatch.End();
            
            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend);
            effect.Parameters["LightMask"].SetValue(lightmask);
            effect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(maintarget,Vector2.Zero,Color.White);
            spriteBatch.End();
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
