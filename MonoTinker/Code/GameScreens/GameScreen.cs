using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Tiles;
using MonoTinker.Code.Managers;

namespace MonoTinker.Code.GameScreens
{
    public sealed class GameScreen : Screen
    {
        private SpriteAtlas TileAtlas;
        private SpriteAtlas playerAtlas;
        private Texture2D light;
        private Effect lightEffect;
        private RenderTarget2D lightMask;
        private RenderTarget2D main;
        private Camera camera;
        private Player player;
        private TileMap map;
        private Color color;
        public GameScreen(IServiceProvider service,int level = 0) : base(service, "Game")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            color = Color.White;
            color.A = 100;
            light = content.Load<Texture2D>("lighting");
            lightEffect = content.Load<Effect>("LightingFX");
            TileAtlas = new SpriteAtlas();
            TileAtlas.PopulateFromSpritesheet(content.Load<Texture2D>("hyptosis_tile-art-batch-3"), new Vector2(32, 32), "TileSetOne");
            TileAtlas.PopulateFromSpritesheet(content.Load<Texture2D>("hyptosis_til-art-batch-2"), new Vector2(32, 32), "TileSetTwo");
            TileAtlas.PopulateFromSpritesheet(content.Load<Texture2D>("light"), new Vector2(32, 32), "Light");
            map = new TileMap();
            map.LoadFromTiledFile(ref TileAtlas, content.RootDirectory + "/map.txt");
            camera = new Camera(ScreenManager.view);
            playerAtlas = new SpriteAtlas();
            lightMask = new RenderTarget2D(ScreenManager.device,map.Widht,map.Height);
            main = new RenderTarget2D(ScreenManager.device, map.Widht, map.Height);

            string[] names = playerAtlas.PopulateFromSpritesheet(content.Load<Texture2D>("playerRun"),
                new Vector2(130, 150), "dude", 1);
            player = new Player(new AnimationV2(names,playerAtlas));
            player.Transform.Scale = Vector2.One * 0.4f;
            
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            map.Update(gameTime);
            player.Update(gameTime);
            camera.Update(gameTime,player.Transform.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager.device.SetRenderTarget(lightMask);
            ScreenManager.device.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            spriteBatch.Draw(light,Vector2.Zero,light.Bounds,color,0,light.Bounds.Center.ToVector2(),Vector2.One*50,SpriteEffects.None,0);
            foreach (var lightTile in map.LightTiles)
            {
                lightTile.Draw(spriteBatch);
            }
            spriteBatch.End();

            ScreenManager.device.SetRenderTarget(main);
            ScreenManager.device.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (var staticTile in map.StaticTiles)
            {
                staticTile.Draw(spriteBatch);
            }
            foreach (var collisionTile in map.CollisionTiles)
            {
                collisionTile.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            spriteBatch.End();

            ScreenManager.device.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,null,null,null,null,camera.Transform);
            lightEffect.Parameters["LightMask"].SetValue(lightMask);
            lightEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(main,Vector2.Zero,Color.White);
            spriteBatch.End();
        }

        
    }
}
