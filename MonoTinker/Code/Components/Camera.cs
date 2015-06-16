using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    public class Camera
    {
        public Matrix Transform;
        private Viewport view;
        private Vector2 center;

        public Camera(Viewport view)
        {
            this.view = view;
        }

        public void Update(GameTime gameTime, Game1 game)
        {
            this.center = game.player.Transform.Position + game.player.SpriteCenter - new Vector2(320, 240);
            this.center.Y = MathHelper.Clamp(center.Y, -480, 0);

            Console.WriteLine(center);
            Transform = Matrix.CreateScale(new Vector3(1,1,0)) *
                        Matrix.CreateTranslation(new Vector3(-center.X,-center.Y,0));
        }
    }
}
