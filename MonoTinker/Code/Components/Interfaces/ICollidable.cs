
namespace MonoTinker.Code.Components.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface ICollidable
    {
        Rectangle BoundingBox { get; }
        bool CollisionEnabled { get; }
        bool Collided(ICollidable obj);
    }
}
