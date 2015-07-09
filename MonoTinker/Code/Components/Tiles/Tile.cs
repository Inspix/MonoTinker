namespace MonoTinker.Code.Components.Tiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    
    using Elements;

    public abstract class Tile : Sprite
    {
        protected Tile(Sprite sprite, Vector2 position) : base(sprite)
        {
            this.Position = position;
            this.IsVisible = true;
        }
    }
}
