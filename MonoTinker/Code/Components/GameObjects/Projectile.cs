using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.Elements
{
    public class Projectile : Sprite,IMovable,ICollidable
    {
        private Rectangle boudndingBox;
        private int life;
        private double timeToUpdate = TimeSpan.FromSeconds(1).TotalSeconds;
        private double timeElapsed;
        public SpriteEffects flip;

        public bool Active;

        public Projectile(Sprite sprite, Vector2 position, float rotation) : base(sprite.Texture,sprite.Source)
        {
            this.Transform = new Transform(position);
            this.Transform.Rotation = rotation;
            this.Transform.Scale = Vector2.One; 
            this.Speed = 500;
            this.life = 20;
            this.Active = true;
        }

        public Vector2 Velocity { get; set; }

        public float Speed { get; set; }

        public float RotationAngles
        {
            get { return this.Transform.Rotation; }
            set { this.Transform.Rotation = value; }
        }

        public float RotationRadians
        {
            get { return MathHelper.ToRadians(this.Transform.Rotation); }
            set { this.Transform.Rotation = MathHelper.ToDegrees(value); }
        }

        public void Move(GameTime gametime)
        {
            this.Transform.Position += Velocity*Speed* (float)gametime.ElapsedGameTime.TotalSeconds;
            timeElapsed += gametime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                life--;
                if (life <= 0)
                {
                    this.Active = false;
                }
            }
            this.boudndingBox.Location = this.Transform.Position.ToPoint();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, Transform.Position, this.Source, Color.White, this.Transform.Rotation,
                this.Center, this.Transform.Scale, this.effect, 0);
        }

        public Rectangle BoundingBox
        {
            get { return this.boudndingBox; }
        }
        
        public bool Collided(ICollidable obj)
        {
            return this.BoundingBox.Intersects(obj.BoundingBox);
        }
    }
}
