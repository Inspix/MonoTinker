

namespace MonoTinker.Code.Components.Elements
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Utils;

    public class AnimationV2
    {
        private List<Animation> layers;
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
                if (layers != null)
                {
                    foreach (var animationV2 in layers)
                    {
                        animationV2.FramesPerSecond = value;
                    }
                }
                this.timeToUpdate = (1f/value);
            }
        }

        public AnimationV2(string[] frames, SpriteAtlas source, int fps = 30)
        {
            this.layers = new List<Animation>();
            this.layers.Add(new Animation(frames,source,fps));
            this.atlas = source;
            this.FramesPerSecond = fps;
            this.Tint = Color.White;
            this.Looping = true;
        }

        /// <summary>
        /// Add layer to the animation (Use the same atlas as the base animation)
        /// </summary>
        /// <param name="animation">animation</param>
        /// <param name="offset">offset of the entire animation</param>
        public void AddLayer(Animation animation, Vector2 offset = default(Vector2))
        {
            if (layers == null)
            {
                this.layers = new List<Animation>();
            }
            animation.Offset = offset;
            layers.Add(animation);
        }

        public void ChangeLayerOffset(int index, Vector2 offset)
        {
            this.layers[index].Offset = offset;
        }

        public Animation Layer(int index)
        {
            return this.layers[index];
        }

        public int LayerCount
        {
            get { return this.layers.Count; }
        }

        public void RemoveLayer(int index)
        {
            try
            {
                layers.RemoveAt(index);
            }
            catch (Exception)
            {
                Debug.Error("Theres no existing layer at this index");
            }
        }
/*
        public Sprite this[int index]
        {
            get { return this.atlas[sprites[index]]; }
        }*/
/*
        public Sprite CurrentFrame
        {
            get { return this[currentFrame]; }
        }*/

        public Color Tint
        {
            get { return this.tint; }
            set
            {
                if (value != Color.White)
                {
                    this.tint = value*0.75f;
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
                if (layers[0].Count != 1)
                {
                    this.currentFrame++;
                    if (layers != null)
                    {
                        foreach (var animationV2 in layers)
                        {
                            animationV2.Update();
                        }
                    }
                }

                if (currentFrame >= this.layers[0].Count)
                {
                    if (!Looping)
                    {
                        OnAnimationFinishInvoke(this,new EventArgs());
                    }
                    currentFrame = 0;
                }
            }
        }

        public void Update()
        {
            this.currentFrame++;
            if (currentFrame >= this.layers[0].Count)
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
            Draw(spriteBatch,position,0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2? origin = null,
            Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Draw(spriteBatch, position, rotation, origin ?? Vector2.Zero,
                    scale ?? Vector2.One, effect);
            }

        }

        public void Reset()
        {
            if (layers != null)
            {
                foreach (var animationV2 in layers)
                {
                    animationV2.Reset(); 
                }
            }
        }
        
    }
}
