using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code
{
    public class Player : IMovable
    {
        private Animation animation;
        private Vector2 velocity;
        private float normalSpeed = 2;
        private bool flip;
        public bool grounded = true;
        private bool jumped;

        public Transform Transform;
        public BoxCollider Collider;

        public Player(Animation anim)
        {
            animation = anim;
            this.Transform = new Transform(Vector2.Zero);
            Collider = new BoxCollider(Transform.Position, animation.Size);
            Speed = normalSpeed;
        }

        public Vector2 Velocity
        {
            get { return this.velocity; } 
            set { this.velocity = value; }
        }

        public float VelocityX
        {
            get { return this.velocity.X; }
            set { this.velocity.X = value; }
        }

        public float VelocityY
        {
            get { return this.velocity.Y; }
            set { this.velocity.Y = value; }
        }

        public float Speed { get; set; }

        public Vector2 SpriteCenter { get { return this.animation.Center; } }

        public Vector2 SpriteSize { get { return this.animation.Size; } }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                VelocityX = -1;

                if (ks.IsKeyDown(Keys.LeftShift))
                {
                    Speed = normalSpeed * 2;
                    animation.FramesPerSecond = 45;
                }
                else
                {
                    Speed = normalSpeed * 1;
                    animation.FramesPerSecond = 30;
                }
                flip = true;
                animation.Update(gameTime);
            }
            else if (ks.IsKeyDown(Keys.D))
            {
                VelocityX = 1;
                if (ks.IsKeyDown(Keys.LeftShift))
                {
                    Speed = normalSpeed * 2;
                    animation.FramesPerSecond = 45;
                }
                else
                {
                    Speed = normalSpeed * 1;
                    animation.FramesPerSecond = 30;
                }
                flip = false;
                animation.Update(gameTime);
            }
            else
            {
                VelocityX = 0;
                animation.Reset();
            }

            if (!jumped && ks.IsKeyDown(Keys.Space))
            {
                jumped = true;
                Transform.PosY -= 10f;
                VelocityY = -5f;
            }
            if (!jumped)
            {
                VelocityY = 0;
            }
            else
            {
                VelocityY += 0.15f*2;
            }

            Move(gameTime);
        }

       
        public void Move(GameTime gametime)
        {
            this.Transform.Position += Velocity * Speed;
            Console.WriteLine(Transform.Position);
            if (Transform.PosY >= 480 - this.SpriteSize.Y)
            {
                jumped = false;
                Transform.PosY = 480 - this.SpriteSize.Y;
            }
            this.Collider.Position = Transform.Position;

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.Texture,Transform.Position, animation.Source,Color.White,0,Vector2.Zero, Vector2.One, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,0);
        }

        
    }
}
