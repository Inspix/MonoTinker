using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Extensions;
using MonoTinker.Code.Components.Interfaces;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.Tiles
{
    public class CollisionTile : StaticTile,ICollidable
    {
        private Rectangle boundingBox;
        public bool Active;

        public CollisionTile(Texture2D texture, Rectangle source, Vector2 position) : base(texture, source, position)
        {
            boundingBox = new Rectangle(position.ToPoint(),source.Size);
            Active = true;
        }

        public bool Collision(Rectangle x)
        {
            return this.boundingBox.Intersects(x);
        }

        public Rectangle BoundingBox { get { return this.boundingBox; } }
        public bool Collided(ICollidable obj)
        {
            if (obj is Projectile)
            { 
                if (BoundingBox.Inflate(Vector2.One*10).Intersects(obj.BoundingBox))
                {
                    ((Projectile) obj).Active = false;
                    return true;
                }
            }

            
            return false;
        }
    }
}
