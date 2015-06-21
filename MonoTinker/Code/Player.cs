using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Interfaces;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code
{
    public class Player : IMovable
    {
        private AnimationV2 animation;
        private Vector2 velocity;
        private float normalSpeed = 2;
        private bool flip;
        public bool grounded = true;
        private bool jumped;

        public Transform Transform;
        public BoxCollider Collider;

        public Player(AnimationV2 anim)
        {
            animation = anim;
            this.Transform = new Transform(Vector2.Zero);
            Collider = new BoxCollider(Transform.Position, animation.CurrentFrame.Size);
            Speed = normalSpeed;
        }

        public Texture2D Texture
        {
            get { return this.animation.CurrentFrame.Texture; }
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

        public Vector2 SpriteCenter { get { return this.animation.CurrentFrame.Center; } }

        public Vector2 SpriteSize { get { return this.animation.CurrentFrame.Size; } }

        public void Update(GameTime gameTime)
        {
            if (Keys.A.Down())
            {
                VelocityX = -1;

                if (Keys.LeftShift.Down())
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
            else if (Keys.D.Down())
            {
                VelocityX = 1;
                if (Keys.LeftShift.Down())
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
            }

            if (!jumped && Keys.Space.Down())
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
            if (Transform.PosY >= 480 - this.SpriteSize.Y)
            {
                jumped = false;
                Transform.PosY = 480 - this.SpriteSize.Y;
            }
            this.Collider.Position = Transform.Position;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.CurrentFrame.Texture,Transform.Position, animation.CurrentFrame.Source,Color.White,0,Vector2.Zero, Transform.Scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,0);
        }
    }
}
