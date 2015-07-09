namespace MonoTinker.Code.GameScreens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Components;
    using Components.Elements;
    using Components.Interfaces;
    using Components.Tiles;

    using global::MonoTinker.Code.Components.UI;

    using Managers;
    using Utils;

    public sealed class OtherScreen : Screen
    {
        private Texture2D bg;
        private Texture2D bg2;
        private Sprite light;
        private SpriteAtlas textures;
        private RenderTarget2D lightMask;
        private RenderTarget2D mainTarget;
        private GraphicsDevice Device;

        private StatusBar hud;
        private Camera camera;
        private PlayerOld player;
        private List<ISimpleDrawable> lights;
        private Random rnd = new Random();
        private bool grayScale;
        private bool invertColors;
        private Effect fx;
        private Effect fx2;
        private Effect fx3;
        public OtherScreen(IServiceProvider service) : base(service, "Other")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            hud=new StatusBar(Vector2.One*20, ScreenManager.Device);
            hud.IsVisible = true;
            textures= new SpriteAtlas();
            Device = ScreenManager.Device;
            lights = new List<ISimpleDrawable>();
            var pp = Device.PresentationParameters;
            lightMask = new RenderTarget2D(Device, pp.BackBufferWidth+500,pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(Device, pp.BackBufferWidth+500,pp.BackBufferHeight);
            camera = new Camera(ScreenManager.View);
            camera.Limit = true;
            camera.Limits(new Vector2(0,0), new Vector2(pp.BackBufferWidth-600, pp.BackBufferHeight));
            fx = content.Load<Effect>("../Shaders/SpriteGrayScale");
            fx2 = content.Load<Effect>("../Shaders/SpriteInvert");
            fx3 = content.Load<Effect>("../Shaders/Lighting");
            bg = content.Load<Texture2D>("playerRun");
            bg2 = content.Load<Texture2D>("ghettoville1");
            light = new Sprite(content.Load<Texture2D>("lighting"));
            lights.Add(new LightTile(light, Vector2.One*100,LightSimpleEffect.Shimmering));
            lights.Add(new LightTile(light, Vector2.One * 200, LightSimpleEffect.Fading));
            lights.Add(new LightTile(light, Vector2.One * 400, LightSimpleEffect.Fading));

            string[] names = textures.PopulateFromSpriteSheet(bg, new Vector2(130, 150), "dude", 1);
            player = new PlayerOld(new Animation(names,textures));
            player.Position = Vector2.One*50;
            player.Scale = Vector2.One*0.5f;

        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            hud.Update(gameTime);
            foreach (var l in lights.OfType<LightTile>())
            {
                l.Update(gameTime);
            }
            player.Update(gameTime);
            camera.Update(gameTime,player.Position);
            InputUpdate(gameTime);
        }

        public void InputUpdate(GameTime gameTime)
        {

            if (Keys.Tab.DownOnce() && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen(Screens.Menu);
            }
            if (Keys.Z.DownOnce())
            {
                grayScale = !grayScale;
            }
            if (Keys.X.DownOnce())
            {
                invertColors = !invertColors;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //hud.DrawElements();
            Device.SetRenderTarget(lightMask);
            Device.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            spriteBatch.Draw(light.Texture,player.Position,null,null,light.Source.Center.ToVector2() - Vector2.One * 20,0f,Vector2.One + Vector2.One *(float)rnd.NextDouble() * 0.5f);
            foreach (var simpleDrawable in lights)
            {
                simpleDrawable.Draw(spriteBatch);
            }
            spriteBatch.End();

            Device.SetRenderTarget(mainTarget);
            Device.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate);

            spriteBatch.Draw(bg2, Vector2.Zero, Color.White);

            if (grayScale) fx.CurrentTechnique.Passes[0].Apply();
            if (invertColors) fx2.CurrentTechnique.Passes[0].Apply();
            player.Draw(spriteBatch);

            spriteBatch.End();

            Device.SetRenderTarget(null);
            
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend);

            fx3.Parameters["LightMask"].SetValue(lightMask);
            fx3.CurrentTechnique.Passes[0].Apply();
            

            spriteBatch.Draw(mainTarget,Vector2.Zero,Color.White);

            spriteBatch.End();
            spriteBatch.Begin();
            hud.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
