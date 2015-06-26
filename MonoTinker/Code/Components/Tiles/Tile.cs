
namespace MonoTinker.Code.Components.Tiles
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Components.Interfaces;

    public abstract class Tile : ISimpleDrawable
    {
        protected Vector2 position;
        protected Color color;
        protected Rectangle source;
        protected Texture2D texture2D;
        protected bool IsActive;

        protected Tile(Texture2D texture, Rectangle source, Vector2 position)
        {
            this.texture2D = texture;
            this.source = source;
            this.position = position;
            this.color = Color.White;
            this.IsActive = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D,position,source,color);
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Rectangle Source
        {
            get { return this.source; }
        }

        public Color Clr
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public Texture2D Texture
        {
            get { return this.texture2D; } 
        }
    }
}
