using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components.Tiles
{
    public class CollisionTile : StaticTile
    {
        private Rectangle boundingBox;

        public CollisionTile(Texture2D texture, Rectangle source, Vector2 position) : base(texture, source, position)
        {
            boundingBox = new Rectangle(position.ToPoint(),source.Size);
        }

        public bool Collision(Rectangle x)
        {
            return this.boundingBox.Intersects(x);
        }
    }
}
