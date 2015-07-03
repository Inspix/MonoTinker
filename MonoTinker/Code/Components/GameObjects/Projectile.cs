
namespace MonoTinker.Code.Components.Elements
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Components.Interfaces;

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
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = Vector2.One; 
            this.Speed = 500;
            this.life = 20;
            this.Active = true;
        }

        public Vector2 Velocity { get; set; }

        public float Speed { get; set; }

        public float RotationAngles
        {
            get { return this.Rotation; }
            set { this.Rotation = value; }
        }

        public float RotationRadians
        {
            get { return MathHelper.ToRadians(this.Rotation); }
            set { this.Rotation = MathHelper.ToDegrees(value); }
        }

        public void Move(GameTime gametime)
        {
            this.Position += Velocity*Speed* (float)gametime.ElapsedGameTime.TotalSeconds;
            timeElapsed += gametime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                life--;
                if (life <= 0)
                {
                    this.Active = false;
                }
            }
            this.boudndingBox.Location = this.Position.ToPoint();
        }

        public Rectangle BoundingBox
        {
            get { return this.boudndingBox; }
        }

        public bool CollisionEnabled { get; set; }

        public bool Collided(ICollidable obj)
        {
            return this.BoundingBox.Intersects(obj.BoundingBox);
        }
    }
}
