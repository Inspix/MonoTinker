using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;

namespace MonoTinker.Code.Components.Interfaces
{
    public interface ITransformable
    {
        Vector2 Position { get; set; }
        Vector2 Scale { get; set; }
        float Rotation { get; set; }
        float ScaleF { get; set; }
    }
}
