using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.GameObjects
{
    public abstract class Entity : ICollidable,IAdvancedDrawable,ITransformable
    {
        protected AnimationController animation;
        protected Sprite sprite;
        protected Vector2 position;
        protected Rectangle boundingBox;
        protected int? updatesOnScreen;
        protected bool isVisible;
        protected bool isAnimated;
        protected double timeToUpdate;
        protected double timeElapsed;

        protected Entity(Sprite sprite, Vector2 position, int? lifeonscreen = null, bool isvisible = true)
        {
            this.sprite = sprite;
            this.Position = position;
            this.updatesOnScreen = lifeonscreen;
            this.isVisible = isvisible;
            this.isAnimated = false;
            this.ScaleF = 1;
        }

        protected Entity(AnimationController animation, Vector2 position, int? lifeonscreen = null, bool isvisible = true)
        {
            this.animation = animation;
            this.Position = position;
            this.updatesOnScreen = lifeonscreen;
            this.isVisible = isvisible;
            this.isAnimated = true;
            this.ScaleF = 1;
        }

        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                if (!isAnimated)
                {
                    this.sprite.Position = value;
                }
                this.boundingBox.Location = value.ToPoint();
            }
        }

        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public float ScaleF { get; set; }

        public bool IsVisible { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if (!isVisible)
            {
                return;
            }
            if (updatesOnScreen != null)
            {
                timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed >= timeToUpdate)
                {
                    timeElapsed -= timeToUpdate;
                    updatesOnScreen--;
                }
            }
            if (isAnimated)
            {
                this.animation.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!isVisible)
            {
                return;
            }
            if (isAnimated)
            {
                animation.Draw(spriteBatch,this.Position,this.Rotation,null,this.Scale);
            }
            else
            {
                sprite.Draw(spriteBatch,this.Position,this.Rotation,this.Scale);
            }
        }

        public Action onLifeEnd { get; set; }

        public bool CollisionEnabled { get; set; }

        public Rectangle BoundingBox
        {
            get { return this.boundingBox; }
        }

        public bool Collided(ICollidable obj)
        {
            return CollisionEnabled && this.boundingBox.Intersects(obj.BoundingBox);
        }
    }
}
