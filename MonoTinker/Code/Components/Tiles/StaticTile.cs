using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components.Tiles
{
    public class StaticTile : Tile
    {
        public new Vector2 Position
        {
            get { return base.position; }
        }

        public StaticTile(Texture2D texture, Rectangle source, Vector2 position) : base(texture, source, position)
        {
        }
    }
}
