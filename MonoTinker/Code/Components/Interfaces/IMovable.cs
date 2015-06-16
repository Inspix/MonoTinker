using Microsoft.Xna.Framework;

namespace MonoTinker.Code.Components.Interfaces
{
    public interface IMovable
    {
        Vector2 Velocity { get; set; }
        float Speed { get; set; }

        void Move(GameTime gametime);
    }
}
