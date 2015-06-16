using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;

namespace MonoTinker.Code
{
    class Player
    {
        public Transform Transform;
        private Animation animation;
        private bool flip;
        public BoxCollider Collider;

        public Player(Animation anim)
        {
            animation = anim;
            this.Transform = new Transform(Vector2.Zero);
            Collider = new BoxCollider(Transform.Position, animation.Size);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                if (ks.IsKeyDown(Keys.LeftShift))
                {
                    Move(new Vector2(-2f, 0));
                    animation.FramesPerSecond = 45;
                }
                else
                {
                    Move(new Vector2(-1f,0));
                    animation.FramesPerSecond = 30;
                }
                flip = true;
                animation.Update(gameTime);
            }
            if (ks.IsKeyDown(Keys.D))
            {
                if (ks.IsKeyDown(Keys.LeftShift))
                {
                    Move(new Vector2(2f, 0));
                    animation.FramesPerSecond = 45;
                }
                else
                {
                    Move(new Vector2(1f, 0));
                    animation.FramesPerSecond = 30;
                }
                flip = false;
                animation.Update(gameTime);
            }
            if (ks.IsKeyDown(Keys.W))
            {
                Move(new Vector2(0, -1));
            }
            if (ks.IsKeyDown(Keys.S))
            {
                Move(new Vector2(0, 1));
            }
        }

        public void Move(Vector2 move)
        {
            this.Transform.Position += move;
            this.Collider.Position += move;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.Texture,Transform.Position, animation.Source,Color.White,0,Vector2.Zero, Vector2.One, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,0);
        }
    }
}
