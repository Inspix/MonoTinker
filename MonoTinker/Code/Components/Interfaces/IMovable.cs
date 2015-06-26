
namespace MonoTinker.Code.Components.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IMovable
    {
        Vector2 Velocity { get; set; }
        float Speed { get; set; }

        void Move(GameTime gametime);
    }
}
