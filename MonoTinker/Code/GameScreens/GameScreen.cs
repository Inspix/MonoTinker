

namespace MonoTinker.Code.GameScreens
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Components;
    using Components.Elements;
    using Components.Tiles;
    using Managers;
    using Utils;

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
        private PlayerOld player;
        private TileMap map;
        private Color color;
        private List<Projectile> projectiles;   

        public GameScreen(IServiceProvider service,int level = 0) : base(service, "Game")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            color = Color.White;
            color.A = 150;
            light = content.Load<Texture2D>("lighting");
            lightEffect = content.Load<Effect>("LightingFX");
            TileAtlas = new SpriteAtlas();
            TileAtlas.PopulateFromSpriteSheet(content.Load<Texture2D>("hyptosis_tile-art-batch-3"), new Vector2(32, 32), "TileSetOne");
            TileAtlas.PopulateFromSpriteSheet(content.Load<Texture2D>("hyptosis_til-art-batch-2"), new Vector2(32, 32), "TileSetTwo");
            TileAtlas.PopulateFromSpriteSheet(content.Load<Texture2D>("light"), new Vector2(32, 32), "Light");
            TileAtlas.Add("LaserProjectile",new Sprite(content.Load<Texture2D>("laser")));
            TileAtlas.Add("ArrowProjectile", new Sprite(content.Load<Texture2D>("arrow")));
            projectiles = new List<Projectile>();
           
            crosshair = content.Load<Texture2D>("crosshair");
            map = new TileMap();
            map.LoadFromTiledFile(ref TileAtlas, content.RootDirectory + "/tt.txt");
            camera = new Camera(ScreenManager.View);
            playerAtlas = new SpriteAtlas();
            lightMask = new RenderTarget2D(ScreenManager.Device,map.Widht,map.Height);
            main = new RenderTarget2D(ScreenManager.Device, map.Widht, map.Height);

            string[] names = playerAtlas.PopulateFromSpriteSheet(content.Load<Texture2D>("playerRun"),
                new Vector2(130, 150), "dude", 1);
            player = new PlayerOld(new Animation(names,playerAtlas));
            player.Scale = Vector2.One * 0.4f; 
            
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            map.Update(gameTime);
            player.Update(gameTime);
            camera.Update(gameTime,player.Position);
            if (InputHandler.MouseDown("left"))
            {
                Projectile p = new Projectile(TileAtlas["LaserProjectile"], player.Position, 0);
                p.Velocity =
                    Vector2.Normalize(player.Position - player.Position +
                                     Mouse.GetState().Position.ToVector2() - ScreenManager.ScreenCenter);
                p.RotationAngles = (float) Math.Atan2(p.Velocity.Y, p.Velocity.X) + 29.8f;
                projectiles.Add(p);

            }
            if (InputHandler.MouseDown("right"))
            {
                Projectile p = new Projectile(TileAtlas["ArrowProjectile"], player.Position, 0);
                p.Velocity =
                    Vector2.Normalize(player.Position - player.Position +
                                     Mouse.GetState().Position.ToVector2() - ScreenManager.ScreenCenter);
                p.RotationAngles = (float)Math.Atan2(p.Velocity.Y, p.Velocity.X) + 29.8f;
                p.Effect = SpriteEffects.FlipVertically;
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
            foreach (var collisionTile in map.CollisionTiles)
            {
                foreach (var projectile in projectiles)
                {
                    collisionTile.Collided(projectile);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager.Device.SetRenderTarget(lightMask);
            ScreenManager.Device.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            spriteBatch.Draw(light,Vector2.Zero,light.Bounds,color,0,light.Bounds.Center.ToVector2(),Vector2.One*50,SpriteEffects.None,0);
            spriteBatch.Draw(light,player.Position,light.Bounds,Color.White,0,Vector2.Zero, Vector2.One*0.5f,SpriteEffects.None, 0 );
            foreach (var lightTile in map.LightTiles)
            {
                lightTile.Draw(spriteBatch);
            }
            spriteBatch.End();

            ScreenManager.Device.SetRenderTarget(main);
            ScreenManager.Device.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (var staticTile in map.StaticTiles)
            { 
                staticTile.Draw(spriteBatch);
            }
            foreach (var collisionTile in map.CollisionTiles)
            {
                if (collisionTile.Active)
                {
                    collisionTile.Draw(spriteBatch);
                }
            }
            foreach (var projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            spriteBatch.End();

            ScreenManager.Device.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,null,null,null,null,camera.Transform);
            lightEffect.Parameters["LightMask"].SetValue(lightMask);
            lightEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(main,Vector2.Zero,Color.White);

            spriteBatch.Draw(crosshair, player.Position + Mouse.GetState().Position.ToVector2() - ScreenManager.ScreenCenter, crosshair.Bounds,Color.White,0,crosshair.Bounds.Center.ToVector2(),Vector2.One,SpriteEffects.None, 0);
            
            spriteBatch.End();
        }

        
    }
}
