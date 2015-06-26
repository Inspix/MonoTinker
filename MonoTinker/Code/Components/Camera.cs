
namespace MonoTinker.Code.Components
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Camera
    {
        public Matrix Transform;
        private Viewport view;
        private Vector2 center;
        private Vector2 minLimit;
        private Vector2 maxLimit;
        public bool Limit;
        

        public void Limits(Vector2 minlimit, Vector2 maxlimit)
        {
            this.minLimit = minlimit;
            this.maxLimit = maxlimit;
        }

        public Camera(Viewport view)
        {
            this.view = view;
        }

        public void Update(GameTime gameTime, Vector2 position, Vector2 offset = default(Vector2))
        {
            this.center = position + offset - new Vector2(view.Width/2f, view.Height/2f);

            if (Limit)
            {
                this.center.Y = MathHelper.Clamp(center.Y, minLimit.Y, maxLimit.Y);
                this.center.X = MathHelper.Clamp(center.X, minLimit.X, maxLimit.X);
            }

            Transform = Matrix.CreateScale(new Vector3(1,1,0)) *
                        Matrix.CreateTranslation(new Vector3(-center.X,-center.Y,0));
        }
    }
}
