
namespace MonoTinker.Code.Components.Elements
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Animation
    {
        private string[] sprites;
        public Vector2 Offset;
        public bool Looping;
        public event EventHandler OnAnimationFinish;
        private SpriteAtlas atlas;
        private Color tint;
        private double timeElapsed;
        private double timeToUpdate;
        private int currentFrame;

        public int FramesPerSecond
        {
            set
            {
                this.timeToUpdate = (1f / value);
            }
        }

        public Animation(string[] frames, SpriteAtlas source, int fps = 30)
        {
            this.sprites = frames;
            this.atlas = source;
            this.FramesPerSecond = fps;
            this.Tint = Color.White;
            this.Looping = true;
        }

        public Sprite this[int index]
        {
            get { return this.atlas[sprites[index]]; }
        }

        public Sprite CurrentFrame
        {
            get { return this[currentFrame]; }
        }

        public int Count
        {
            get { return this.sprites.Length; }
        }

        public Color Tint
        {
            get { return this.tint; }
            set
            {
                if (value != Color.White)
                {
                    this.tint = value * 0.75f;
                }
                else
                {
                    this.tint = value;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                Console.WriteLine(sprites[currentFrame]);
                if (sprites.Length != 1)
                {
                    this.currentFrame++;
                }
                if (currentFrame >= this.sprites.Length)
                {
                    if (!Looping)
                    {
                        OnAnimationFinishInvoke(this, new EventArgs());
                    }
                    currentFrame = 0;
                }
            }
        }

        public void Update()
        {
            this.currentFrame++;
            if (currentFrame >= this.sprites.Length)
            {
                if (!Looping)
                {
                    OnAnimationFinishInvoke(this, new EventArgs());
                }
                currentFrame = 0;
            }
        }

        private void OnAnimationFinishInvoke(object sender, EventArgs e)
        {
            if (OnAnimationFinish != null)
            {
                OnAnimationFinish(sender, e);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position,0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
        {
            spriteBatch.Draw(CurrentFrame.Texture, position + Offset, CurrentFrame.Source, Tint, rotation, origin ?? Vector2.Zero, scale ?? Vector2.One, effect, 0);
        }

        public void Reset()
        {
            this.currentFrame = 0;
            this.timeElapsed = 0;
        }

    }
}
