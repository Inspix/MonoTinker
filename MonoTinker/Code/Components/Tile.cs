using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    class Tile
    {
        private Sprite sprite;
        private Transform transform;

        public Tile(Sprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            transform = new Transform(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.Texture,transform.Position,sprite.Source,Color.White,0,Vector2.Zero,transform.Scale,SpriteEffects.None,0);
        }
    }
}
