using Microsoft.Xna.Framework;

namespace MonoTinker.Code.Components.Interfaces
{
    public interface ICollidable
    {
        Rectangle BoundingBox { get; }

        bool Collided(ICollidable obj);
    }
}
