

using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.Elements
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Utils;

    public class AnimationV2 : IAdvancedDrawable
    {
        private List<Animation> layers;
        private List<string> layerTags;
        private Transform transform;
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
            this.layerTags = new List<string>();
            this.layers.Add(new Animation(frames,source,fps));
            this.layerTags.Add("base");
            this.atlas = source;
            this.FramesPerSecond = fps;
            this.Looping = true;
        }

        public AnimationV2(Animation baseAnim)
        {
            this.layers = new List<Animation>();
            this.layerTags = new List<string>();
            this.layers.Add(baseAnim);
            this.layerTags.Add("base");
            this.FramesPerSecond = baseAnim.FramesPerSecond;
            this.Looping = baseAnim.Looping;
        }

        /// <summary>
        /// Add layer to the animation (Use the same atlas as the base animation)
        /// </summary>
        /// <param name="animation">animation</param>
        /// <param name="offset">offset of the entire animation</param>
        public void AddLayer(Animation animation, string tag = "",Vector2 offset = default(Vector2))
        {
            if (layers == null)
            {
                this.layers = new List<Animation>();
            }
            animation.Offset = offset;
            layers.Add(animation);
            layerTags.Add(tag);
        }

        public void ChangeLayerOffset(int index, Vector2 offset)
        {
            this.layers[index].Offset = offset;
        }

        public Animation Layer(int index)
        {
            return this.layers[index];
        }

        public Animation Layer(string tag)
        {
            int index = layerTags.IndexOf(tag);
            return Layer(index);
        }

        public int LayerCount
        {
            get { return this.layers.Count; }
        }

        public void RemoveLayer(int index)
        {
            try
            {
                layerTags.RemoveAt(index);
                layers.RemoveAt(index);
            }
            catch (Exception)
            {
                Debug.Error("Theres no existing layer at this index");
            }
        }

        public void RemoveLayer(string tag)
        {
            int index = layerTags.IndexOf(tag);
            if (index == -1)
            {
                Debug.Error("Theres no existing layer with tag: {0}",tag);
                return;
            }
            RemoveLayer(index);
        }

        public bool ContainsLayerTag(string tag)
        {
            return this.layerTags.Contains(tag);
        }

        public Transform Transform
        {
            get { return this.transform ?? (this.transform = new Transform()); }
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

        public void Draw(SpriteBatch spriteBatch)
        {
           this.Draw(spriteBatch,this.transform.Position,this.Transform.Rotation,Vector2.Zero,this.Transform.Scale);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            this.Draw(spriteBatch,position,0);
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
