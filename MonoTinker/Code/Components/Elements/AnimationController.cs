// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationController.cs" company="Inspix">
// Copyright
// </copyright>
// <summary>
// AnimationController class
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MonoTinker.Code.Components.Elements
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Utils;

    /// <summary>
    /// Delegate for passing the name of the Animation of the given State
    /// </summary>
    /// <param name="stateName">
    /// Animation state name
    /// </param>
    public delegate void StateAnimationFinish(string stateName);

    /// <summary>
    /// Controller for Multiple animations - States
    /// </summary>
    public class AnimationController
    {
        #region Private fields
        /// <summary>
        /// Dictionary that stores the state Id and its animation
        /// </summary>
        private Dictionary<string, AnimationV2> states;

        /// <summary>
        /// Stores the current state
        /// </summary>
        private string currentState;


        /// <summary>
        /// Contains position, scale and rotation
        /// </summary>
        private Transform transform;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationController"/> class.
        /// <remarks>Reset on state change - True by default</remarks>
        /// </summary>
        public AnimationController()
        {
            this.states = new Dictionary<string, AnimationV2>();
            this.ResetOnStateChange = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationController"/> class.
        /// </summary>
        /// <param name="states">
        /// Already filled dictionary with tags and states
        /// </param>
        public AnimationController(Dictionary<string, AnimationV2> states)
        {
            this.ResetOnStateChange = true;
            this.States = states;
        } 
        #endregion

        /// <summary>
        /// Event that fires whenever any of the animations finishes
        /// </summary>
        public event StateAnimationFinish OnStateAnimationFinish;

        #region Properties
        /// <summary>
        /// Gets or sets the States of the controller
        /// </summary>
        public Dictionary<string, AnimationV2> States
        {
            get
            {
                return this.states;
            }

            set
            {
                if (value == null)
                {
                    Debug.Error("The animation controler states can not be empty");
                    return;
                }

                this.currentState = value.First().Key;
                this.states = value;
            }
        }

        public Transform Transform
        {
            get { return this.transform ?? (this.transform = new Transform()); }
        }

        public string Tag { private get; set; }

        /// <summary>
        /// Gets or sets if the current state animation should reset on state change
        /// </summary>
        public bool ResetOnStateChange { get; set; }

        /// <summary>
        /// Sets the FPS to every animation in the controller
        /// </summary>
        public int Fps
        {
            set
            {
                foreach (var animationV2 in this.states)
                {
                    animationV2.Value.FramesPerSecond = value;
                }
            }
        }

        /// <summary>
        /// Gets the current state string ID
        /// </summary>
        public string CurrentState
        {
            get
            {
                return this.currentState;
            }

            private set
            {
                if (!this.states.ContainsKey(value))
                {
                    Debug.Error("The animation controler does not have the state: {0}", value);
                }
                else if (this.currentState != value)
                {
                    if (this.ResetOnStateChange)
                    {
                        this.states[value].Reset();
                    }

                    this.currentState = value;
                }
            }
        } 
        #endregion

        /// <summary>
        /// Gets the animation of the given state
        /// </summary>
        /// <param name="state">
        /// State string ID
        /// </param>
        /// <returns>
        /// <see cref="AnimationV2"/>
        /// </returns>
        public AnimationV2 this[string state]
        {
            get { return this.states[state]; }
        }

        #region Methods
        /// <summary>
        /// Add a state to the controller and subscribe to the Animations OnAnimationFinish event
        /// </summary>
        /// <param name="stateName">
        /// State string ID
        /// </param>
        /// <param name="animation">
        /// <see cref="AnimationV2"/> for the given state
        /// </param>
        public void AddState(string stateName, AnimationV2 animation)
        {
            if (this.states.ContainsKey(stateName))
            {
                Debug.Error("The Animation controler already has a state : {0}", stateName);
                return;
            }

            if (this.currentState == null)
            {
                this.currentState = stateName;
            }

            this.states.Add(stateName, animation);
            this.states[stateName].OnAnimationFinish += (sender, args) => this.OnStateAnimationFinishInvoke();
        }

        /// <summary>
        /// Overwrite a current state with new AnimationV2
        /// </summary>
        /// <param name="stateName">
        /// State string ID to overwrite
        /// </param>
        /// <param name="animation">
        /// New AnimationV2
        /// </param>
        public void OverWriteState(string stateName, AnimationV2 animation)
        {
            if (!this.states.ContainsKey(stateName))
            {
                Debug.Error("The animation doesn't have state: {0} to overwrite to..", stateName);
                return;
            }

            this.states[stateName] = null;
            this.states[stateName] = animation;
        }

        /// <summary>
        /// Changes the current state
        /// </summary>
        /// <param name="statename">
        /// The new state string ID
        /// </param>
        public void ChangeState(string statename)
        {
            this.CurrentState = Tag+statename;
        }

        /// <summary>
        /// Update method that updates the current state animation with time snapshot
        /// </summary>
        /// <param name="gameTime">
        /// GameTime snapshot
        /// </param>
        public void Update(GameTime gameTime)
        {
            this.states[this.currentState].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(spriteBatch, this.Transform.Position,this.Transform.Rotation,Vector2.Zero,this.Transform.Scale);
        }

        /// <summary>
        /// Simple draw method
        /// </summary>
        /// <param name="spriteBatch">
        /// SpriteBatch to draw with
        /// </param>
        /// <param name="position">
        /// Position to draw at
        /// </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            this.Draw(spriteBatch,position,this.Transform.Rotation);
        }

        /// <summary>
        /// Advanced draw method
        /// </summary>
        /// <param name="spriteBatch">
        /// SpriteBatch to draw with
        /// </param>
        /// <param name="position">
        /// Position to draw at
        /// </param>
        /// <param name="rotation">
        /// The rotation of the AnimationV2
        /// </param>
        /// <param name="origin">
        /// The origin of the AnimationV2 (pivot point)
        /// </param>
        /// <param name="scale">
        /// The scale of the Animations
        /// </param>
        /// <param name="effect">
        /// The SpriteEffect of the Animations
        /// </param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
        {
            this.states[this.currentState].Draw(spriteBatch, position, rotation, origin, scale, effect);
        }

        /// <summary>
        /// Invokes the StateAnimationFinish event if there are any subscribers
        /// </summary>
        private void OnStateAnimationFinishInvoke()
        {
            if (this.OnStateAnimationFinish != null)
            {
                this.OnStateAnimationFinish(this.CurrentState);
            }
        } 
        #endregion
    }
}
