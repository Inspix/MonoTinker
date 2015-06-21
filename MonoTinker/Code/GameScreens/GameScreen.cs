using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Tiles;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.GameScreens
{
    public sealed class GameScreen : Screen
    {
        private SpriteAtlas TileAtlas;
        private SpriteAtlas playerAtlas;
        private Texture2D light;
        private RenderTarget2D lightMask;
        private RenderTarget2D main;
        private Effect lightEffect;
        private Camera camera;
        private Texture2D crosshair;
        private Player player;
        private TileMap map;
        private Color color;
        private AnimationV2 projectileAnimationV2;
        private List<Projectile> projectiles;   

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
            TileAtlas.PopulateFromSpriteSheet(content,"projectiles");
            projectiles = new List<Projectile>();
            string[] projectileAnimation = new[]
            {
                "slice01_01", "slice02_02", "slice03_03",
                "slice04_04", "slice05_05", "slice06_06",
            };
            crosshair = content.Load<Texture2D>("crosshair");
            projectileAnimationV2 = new AnimationV2(projectileAnimation,TileAtlas);
            map = new TileMap();
            map.LoadFromTiledFile(ref TileAtlas, content.RootDirectory + "/tt.txt");
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
            if (InputHandler.MouseDown("left"))
            {
                Projectile p = new Projectile(TileAtlas["slice01_01"], player.Transform.Position,0);
                p.Velocity =
                    Vector2.Normalize(player.Transform.Position - player.Transform.Position +
                                      Mouse.GetState().Position.ToVector2() - ScreenManager.ScreenCenter);
                p.RotationAngles = (float) Math.Atan2(p.Velocity.Y, p.Velocity.X) + 29.8f;
                projectiles.Add(p);

            }
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Move(gameTime);
                if (!projectiles[i].Active)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager.device.SetRenderTarget(lightMask);
            ScreenManager.device.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            spriteBatch.Draw(light,Vector2.Zero,light.Bounds,color,0,light.Bounds.Center.ToVector2(),Vector2.One*50,SpriteEffects.None,0);
            spriteBatch.Draw(light,player.Transform.Position,light.Bounds,Color.White,0,Vector2.Zero, Vector2.One*0.5f,SpriteEffects.None, 0 );
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
            foreach (var projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            spriteBatch.End();

            ScreenManager.device.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,null,null,null,null,camera.Transform);
            lightEffect.Parameters["LightMask"].SetValue(lightMask);
            lightEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(main,Vector2.Zero,Color.White);

            spriteBatch.Draw(crosshair, player.Transform.Position + Mouse.GetState().Position.ToVector2() - ScreenManager.ScreenCenter, crosshair.Bounds,Color.White,0,crosshair.Bounds.Center.ToVector2(),Vector2.One,SpriteEffects.None, 0);
            
            spriteBatch.End();
        }

        
    }
}
