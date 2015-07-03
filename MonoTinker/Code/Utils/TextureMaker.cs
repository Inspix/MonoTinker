using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Utils
{
    public class TextureMaker
    {
        public static Texture2D Arrow(GraphicsDevice device,int size,Color? top = null,Color? tip = null, Color? bottom = null)
        {
            RenderTarget2D target = new RenderTarget2D(device, size, size);
            VertexPositionColor[] vertices = new VertexPositionColor[3];
            
            BasicEffect effect = new BasicEffect(device);
            effect.Projection = Matrix.CreateOrthographic(size, size, -1,1);
            effect.VertexColorEnabled = true;
            vertices[0].Position = new Vector3(-size / 2f, size / 2f, 0);
            vertices[0].Color = top ?? Color.Transparent;
            vertices[1].Position = new Vector3(size / 2f, 0,0);
            vertices[1].Color = tip ?? Color.White;
            vertices[2].Position = new Vector3(-size / 2f, -size / 2f, 0);
            vertices[2].Color = bottom ?? Color.Transparent;
            
            device.SetRenderTarget(target);
            device.Clear(Color.Transparent);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1, VertexPositionColor.VertexDeclaration);
            }
            device.SetRenderTarget(null);
            return target;
        }
    }
}
