namespace MonoTinker.Code.Components.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class TextBox : InterfaceElement
    {
        
        protected TextBox(Vector2 position, GraphicsDevice device)
            : base(position, device)
        {
        }
    }
}
