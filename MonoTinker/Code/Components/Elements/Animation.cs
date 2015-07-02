// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Animation.cs" company="Inspix">
// Copyright  
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MonoTinker.Code.Components.Elements
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Base animation class
    /// Gets its frames based on string named Sprites in the SpriteAtlas. See <see cref="SpriteAtlas"/>
    /// for more information
    /// </summary>
    public class Animation
    {
        #region Private Fields
        /// <summary>
        /// Stores frame names
        /// </summary>
        private readonly string[] frameNames;

        /// <summary>
        /// SpriteAtlas that contains the sprites needed by the animation
        /// </summary>
        private readonly SpriteAtlas atlas;

        /// <summary>
        /// Color Tint of the whole animation
        /// </summary>
        private Color tint;

        /// <summary>
        /// Set to true after the animation is finished(Looping off only)
        /// </summary>
        private bool finished;

        /// <summary>
        /// Number of frames per second
        /// </summary>
        private int fps;

        /// <summary>
        /// Time counter used to check how much time is passed
        /// </summary>
        private double timeElapsed;


        /// <summary>
        /// Time in seconds to update at ( Set by FramesPerSecond property);
        /// </summary>
        private double timeToUpdate;

        /// <summary>
        /// Current frame of the animation
        /// </summary>
        private int currentFrame;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="frames">
        /// Frame names
        /// </param>
        /// <param name="source">
        /// SpriteAtlas containing the frames
        /// </param>
        /// <param name="fps">
        /// Frames per second
        /// </param>
        public Animation(string[] frames, SpriteAtlas source, int fps = 30)
        {
            this.frameNames = frames;
            this.atlas = source;
            this.FramesPerSecond = fps;
            this.Tint = Color.White;
            this.Looping = true;
        } 
        #endregion

        /// <summary>
        /// Event that fires when the animation finishes(Only when set to not loop)
        /// </summary>
        public event EventHandler OnAnimationFinish;

        #region Properties
        /// <summary>
        /// Sets the frames per second(Changes time to update based on Value given)
        /// </summary>
        public int FramesPerSecond
        {
            get { return this.fps; }
            set
            {
                this.fps = value;
                this.timeToUpdate = 1f / value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation should loop.
        /// </summary>
        public bool Looping { get; set; }


        /// <summary>
        /// Gets or sets the animation position offset
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets the current frame of the animation
        /// </summary>
        public Sprite CurrentFrame
        {
            get { return this[this.currentFrame]; }

        }

        /// <summary>
        /// Gets the frame count
        /// </summary>
        public int Count
        {
            get { return this.frameNames.Length; }
        }

        /// <summary>
        /// Gets or sets the tint of the whole animation (White 100% alpha, all other Colors 75% alpha)
        /// </summary>
        public Color Tint
        {
            get
            {
                return this.tint;
            }

            set
            {
                this.tint = value;
            }
        } 
        #endregion

        /// <summary>
        /// Indexer that returns a Sprite from the SpriteAtlas, based on Integer index
        /// </summary>
        /// <param name="index">
        /// The index of the Sprite
        /// </param>
        /// <returns>
        /// Returns <see cref="Sprite"/> from the Animations SpriteAtlas
        /// </returns>
        public Sprite this[int index]
        {
            get { return this.atlas[this.frameNames[index]]; }
        }

        #region Methods
        /// <summary>
        /// Standard method that draws the animation at given position
        /// </summary>
        /// <param name="spriteBatch">
        /// SpriteBatch that draws with
        /// </param>
        /// <param name="position">
        /// Position to draw at
        /// </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            this.Draw(spriteBatch, position, 0);
        }

        /// <summary>
        /// Advanced method that draws the animation
        /// </summary>
        /// <param name="spriteBatch">
        /// SpriteBatch to draw with
        /// </param>
        /// <param name="position">
        /// The position to draw at
        /// </param>
        /// <param name="rotation">
        /// The rotation of the Animation
        /// </param>
        /// <param name="origin">
        /// The origin of the Sprite(pivot point)
        /// </param>
        /// <param name="scale">
        /// The scale of the Animation
        /// </param>
        /// <param name="effect">
        /// SpriteEffect of the Animation
        /// </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
        {
            spriteBatch.Draw(this.CurrentFrame.Texture, position + this.Offset, this.CurrentFrame.Source, this.Tint, rotation, origin ?? Vector2.Zero, scale ?? Vector2.One, effect, 0);
        }

        /// <summary>
        /// Resets the Animation
        /// </summary>
        public void Reset()
        {
            this.currentFrame = 0;
            this.timeElapsed = 0;
            this.finished = false;
        }

        /// <summary>
        /// Update method that changes the frames of the animation based on Time snapshot
        /// Use this method if you want to get the desired frames per second
        /// </summary>
        /// <param name="gameTime">
        /// Time snapshot
        /// </param>
        public void Update(GameTime gameTime)
        {

            if (!this.finished)
            {
                this.timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                if (this.timeElapsed > this.timeToUpdate)
                {
                    this.timeElapsed -= this.timeToUpdate;
                    Console.WriteLine(this.frameNames[this.currentFrame]);
                    if (this.frameNames.Length != 1)
                    {
                        this.currentFrame++;
                    }

                    if (this.currentFrame >= this.frameNames.Length)
                    {
                        if (!this.Looping)
                        {
                            this.finished = true;
                            this.OnAnimationFinishInvoke(this, new EventArgs());
                        }

                        this.currentFrame = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Update method that changes the current frame instantly(Used mostly in <see cref="AnimationV2"/>.
        /// </summary>
        public void Update()
        {

            if (!this.finished)
            {
                this.currentFrame++;
                if (this.currentFrame >= this.frameNames.Length)
                {
                    if (!this.Looping)
                    {
                        this.finished = true;
                        this.OnAnimationFinishInvoke(this, new EventArgs());
                    }

                    this.currentFrame = 0;
                }
            }
        }

        public Animation Copy()
        {
            return new Animation(this.frameNames,this.atlas,this.fps);
        }

        /// <summary>
        /// Invoke the <see cref="OnAnimationFinish"/> event if there are any subscribers
        /// </summary>
        /// <param name="sender">
        /// Object sending the event
        /// </param>
        /// <param name="e">
        /// EventArgs (empty)
        /// </param>
        private void OnAnimationFinishInvoke(object sender, EventArgs e)
        {
            if (this.OnAnimationFinish != null)
            {
                this.OnAnimationFinish(sender, e);
            }
        } 

        #endregion
    }
}
