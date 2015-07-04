
namespace MonoTinker.Code.Components.Elements
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Interfaces;
    using Utils;

    public class AnimationV2 : IAdvancedDrawable, ITransformable
    {
        #region Private Fields
        /// <summary>
        /// Conatins the Animation layers
        /// </summary>
        private List<Animation> layers;

        /// <summary>
        /// Contains the layer tags
        /// </summary>
        private List<string> layerTags;
        

        /// <summary>
        /// Tint fro the whole animation
        /// </summary>
        private Color tint;

        /// <summary>
        /// Time counter
        /// </summary>
        private double timeElapsed;

        /// <summary>
        /// Time in seconds before the update(used to determine fps)
        /// </summary>
        private double timeToUpdate;

        /// <summary>
        /// Current frame index
        /// </summary>
        private int currentFrame;
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="AnimationV2"/> class.
        /// </summary>
        /// <param name="frames">Base animation frame names</param>
        /// <param name="source">Source atlas</param>
        /// <param name="fps">Frames per second</param>
        public AnimationV2(string[] frames, SpriteAtlas source, int fps = 30)
        {
            this.layers = new List<Animation>();
            this.layerTags = new List<string>();
            this.layers.Add(new Animation(frames, source, fps));
            this.layerTags.Add("base");
            this.FramesPerSecond = fps;
            this.Looping = true;
            this.ScaleF = 1;
            this.IsVisible = true;
        }

        /// <summary>
        /// /Initializes a new instance of the <see cref="AnimationV2"/> class.
        /// </summary>
        /// <param name="baseAnim">Base animation</param>
        public AnimationV2(Animation baseAnim)
        {
            this.layers = new List<Animation>();
            this.layerTags = new List<string>();
            this.layers.Add(baseAnim);
            this.layerTags.Add("base");
            this.FramesPerSecond = baseAnim.FramesPerSecond;
            this.Looping = baseAnim.Looping;
            this.ScaleF = 1;
            this.IsVisible = true;
        }
        #endregion


        /// <summary>
        /// Fired on animation finish(when not looping)
        /// </summary>
        public event EventHandler OnAnimationFinish;

        #region Properties
        /// <summary>
        /// Sets the frames per second
        /// </summary>
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
                this.timeToUpdate = (1f / value);
            }
        }

        /// <summary>
        /// Gets or sets if the animation should loop
        /// </summary>
        public bool Looping { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public float ScaleF
        {
            get
            {
                return (this.Scale.X + this.Scale.Y)/2;
            }
            set { this.Scale = Vector2.One*value; }
        }

        /// <summary>
        /// Gets the Layers count
        /// </summary>
        public int LayerCount
        {
            get { return this.layers.Count; }
        }

        /// <summary>
        /// Tint for the whole animation(changes each layer tint)
        /// </summary>
        public Color Tint
        {
            get { return this.tint; }
            set
            {
                foreach (var animation in layers)
                {
                    animation.Tint = value;
                }
                this.tint = value;

            }
        }

        public bool IsVisible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Add layer to the animation (Use the same atlas as the base animation)
        /// </summary>
        /// <param name="animation">animation</param>
        /// <param name="offset">offset of the entire animation layer</param>
        public void AddLayer(Animation animation, string tag)
        {
            if (layers == null)
            {
                this.layers = new List<Animation>();
            }
            layers.Add(animation);
            layerTags.Add(tag);
        }

        /// <summary>
        /// Changes the offset of a given layer
        /// </summary>
        /// <param name="index">Layer index</param>
        /// <param name="offset">Offset</param>
        public void ChangeLayerOffset(int index, Vector2 offset)
        {
            this.layers[index].Offset = offset;
        }

        /// <summary>
        /// Gets the Animation at given layer index
        /// </summary>
        /// <param name="index">Layer index</param>
        /// <returns><see cref="Animation"/></returns>
        public Animation Layer(int index)
        {
            return this.layers[index];
        }

        /// <summary>
        /// Gets the Animation layer with given tag
        /// </summary>
        /// <param name="tag">Layer tag</param>
        /// <returns><see cref="Animation"/></returns>
        public Animation Layer(string tag)
        {
            int index = layerTags.IndexOf(tag);
            return Layer(index);
        }

        /// <summary>
        /// Removes a layer at given index
        /// </summary>
        /// <param name="index">Layer index</param>
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

        /// <summary>
        /// Removes a layer with given tag
        /// </summary>
        /// <param name="tag">Layer tag</param>
        public void RemoveLayer(string tag)
        {
            int index = layerTags.IndexOf(tag);
            if (index == -1)
            {
                Debug.Error("Theres no existing layer with tag: {0}", tag);
                return;
            }
            RemoveLayer(index);
        }

        /// <summary>
        /// Checks if theres a layer with given tag
        /// </summary>
        /// <param name="tag">Layer tag</param>
        /// <returns><see cref="bool"/></returns>
        public bool ContainsLayerTag(string tag)
        {
            return this.layerTags.Contains(tag);
        }

        /// <summary>
        /// Update the animation with gametime snapshot(used to calculate when to change frames)
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!IsVisible)
            {
                return;
            }
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
                        OnAnimationFinishInvoke(this, new EventArgs());
                    }
                    currentFrame = 0;
                }
            }
        }

        /// <summary>
        /// Change frame
        /// </summary>
        public void Update()
        {
            if (!IsVisible)
            {
                return;
            }
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

        /// <summary>
        /// Draw the current frame with given <see cref="SpriteBatch"/>
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                this.Draw(spriteBatch, this.Position, this.Rotation, Vector2.Zero, this.Scale);
            }
        }

        /// <summary>
        /// Draws the animation at given position(override the animation transform)
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        /// <param name="position">Position to draw at</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (IsVisible)
            {
                this.Draw(spriteBatch, position, 0);
            }
        }

        /// <summary>
        /// Draws the animation frame with specified arguments
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        /// <param name="position">Position to draw at</param>
        /// <param name="rotation">Animation Rotation</param>
        /// <param name="origin">Animation Origin</param>
        /// <param name="scale">Animation Scale</param>
        /// <param name="effect">SpriteEffect</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2? origin = null,
            Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
        {
            if (IsVisible)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i].Draw(spriteBatch, position, rotation, origin ?? Vector2.Zero,
                        scale ?? Vector2.One, effect);
                }
            }
        }

        /// <summary>
        /// Reset all layers of the animation to the first frame;
        /// </summary>
        public void Reset()
        {
            if (layers != null)
            {
                foreach (var animation in layers)
                {
                    animation.Reset();
                }
            }
        }

        /// <summary>
        /// Event check and invoke
        /// </summary>
        private void OnAnimationFinishInvoke(object sender, EventArgs e)
        {
            if (OnAnimationFinish != null)
            {
                OnAnimationFinish(sender, e);
            }
        }

        #endregion
    }
}
