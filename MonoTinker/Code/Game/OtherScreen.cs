using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Game
{
    public sealed class OtherScreen : Screen
    {
        private Texture2D bg;
        private Texture2D bg2;
        private Texture2D light;
        private RenderTarget2D lightMask;
        private RenderTarget2D mainTarget;
        private GraphicsDevice Device;
        private Camera camera;
        private Player player;
        private bool firstFramePassed;
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
            Device = ScreenManager.device;
            var pp = Device.PresentationParameters;
            lightMask = new RenderTarget2D(Device, pp.BackBufferWidth+500,pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(Device, pp.BackBufferWidth+500,pp.BackBufferHeight);
            camera = new Camera(ScreenManager.view);
            camera.Limit = true;
            camera.Limits(new Vector2(0,0), new Vector2(pp.BackBufferWidth-600, pp.BackBufferHeight));
            fx = content.Load<Effect>("../Shaders/SpriteGrayScale");
            fx2 = content.Load<Effect>("../Shaders/SpriteInvert");
            fx3 = content.Load<Effect>("../Shaders/Lighting");
            bg = content.Load<Texture2D>("playerRun");
            bg2 = content.Load<Texture2D>("ghettoville1");
            light = content.Load<Texture2D>("lighting");
            player = new Player(new Animation(bg,new Vector2(130,150),1));
            player.Transform.Position = Vector2.One*50;
            player.Transform.Scale = Vector2.One*0.5f;

        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            camera.Update(gameTime,player.Transform.Position);
            InputUpdate(gameTime);
        }

        public void InputUpdate(GameTime gameTime)
        {
            if (Keys.Tab.DownOnce() && !ScreenManager.Transitioning)
            {
                ScreenManager.ChangeScreen("Menu");
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
            Console.WriteLine(Device.Indices);
            Device.SetRenderTarget(lightMask);
            Device.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            spriteBatch.Draw(light,player.Transform.Position,null,null,player.SpriteCenter * (Vector2.One + player.Transform.Scale));
            spriteBatch.Draw(light, new Rectangle(0,0,Device.Viewport.Width,Device.Viewport.Height), Color.White * 0.5f);
            spriteBatch.Draw(light, new Vector2(450, 200), Color.White);
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
            Device.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            fx3.Parameters["LightMask"].SetValue(lightMask);
            fx3.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainTarget,Vector2.Zero,Color.White);
            spriteBatch.End();
        }
    }
}
