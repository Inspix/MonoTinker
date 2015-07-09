namespace MonoTinker.Code.Components.Tiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Components.Elements;
    using Components.Extensions;
    using Components.Interfaces;

    public class CollisionTile : StaticTile,ICollidable
    {
        private Rectangle boundingBox;
        public bool Active;

        public CollisionTile(Sprite sprite, Vector2 position) : base(sprite, position)
        {
            boundingBox = new Rectangle(position.ToPoint(),source.Size);
            Active = true;
        }

        public bool Collision(Rectangle x)
        {
            return this.boundingBox.Intersects(x);
        }

        public Rectangle BoundingBox { get { return this.boundingBox; } }
        public bool CollisionEnabled { get; set; }

        public bool Collided(ICollidable obj)
        {
            if (obj is Projectile)
            { 
                if (BoundingBox.InflateExt(Vector2.One*10).Intersects(obj.BoundingBox))
                {
                    ((Projectile) obj).Active = false;
                    return true;
                }
            }

            
            return false;
        }
    }
}
