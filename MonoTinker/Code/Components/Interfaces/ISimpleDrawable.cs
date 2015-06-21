using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components.Interfaces
{
    public interface ISimpleDrawable
    {
        void Draw(SpriteBatch spriteBatch);
        Vector2 Position { get; set; }
        Rectangle Source { get;}
        Color Clr { get; set; }
        Texture2D Texture { get; }
    }
}
