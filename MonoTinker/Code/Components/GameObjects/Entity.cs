using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.GameObjects
{
    public abstract class Entity : ICollidable,IAdvancedDrawable
    {
        protected AnimationController animation;
        protected Sprite sprite;
        protected Transform transform;
        protected Rectangle boundingBox;
        protected int? updatesOnScreen;
        protected bool isVisible;
        protected bool isAnimated;
        protected double timeToUpdate;
        protected double timeElapsed;

        protected Entity(Sprite sprite, Vector2 position, int? lifeonscreen = null, bool isvisible = true)
        {
            this.sprite = sprite;
            this.transform = new Transform(position);
            this.updatesOnScreen = lifeonscreen;
            this.isVisible = isvisible;
            this.isAnimated = false;
        }

        protected Entity(AnimationController animation, Vector2 position, int? lifeonscreen = null, bool isvisible = true)
        {
            this.animation = animation;
            this.transform = new Transform(position);
            this.updatesOnScreen = lifeonscreen;
            this.isVisible = isvisible;
            this.isAnimated = true;
        }

        public Vector2 Position
        {
            get { return this.transform.Position; }
            set
            {
                this.transform.Position = value;
                if (!isAnimated)
                {
                    this.sprite.Position = value;
                }
                this.boundingBox.Location = value.ToPoint();
            }
        }

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
                animation.Draw(spriteBatch,this.transform.Position,this.transform.Rotation,null,this.transform.Scale);
            }
            else
            {
                sprite.Draw(spriteBatch,this.transform.Position,this.transform.Rotation,this.transform.Scale);
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
