namespace MonoTinker.Code.Components.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface ISimpleDrawable
    {
        void Draw(SpriteBatch spriteBatch);
        Vector2 Position { get; set; }
        Rectangle Source { get;}
        Color Clr { get; set; }
        Texture2D Texture { get; }
    }
}
