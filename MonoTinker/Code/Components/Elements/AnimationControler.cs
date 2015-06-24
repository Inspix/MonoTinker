using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.Elements
{
    public delegate void AnimationFinish(string stateName);

    public class AnimationControler
    {
        private Dictionary<string, AnimationV2> states;
        private string currentState;
        public bool ResetOnStateChange;
        public event AnimationFinish OnAnimationFinish;

        public AnimationControler()
        {
            this.states = new Dictionary<string, AnimationV2>();
            ResetOnStateChange = true;
        }

        public AnimationControler(Dictionary<string, AnimationV2> states)
        {
            ResetOnStateChange = true;
            this.States = states;
        }

        public Dictionary<string, AnimationV2> States
        {
            get { return this.states; }
            set {
                if (value == null)
                {
                    Debug.Error("The animation controler states can not be empty");
                    return;
                }
                currentState = value.First().Key;
                this.states = value;
            }
        }

        public AnimationV2 this[string state]
        {
            get { return this.states[state]; }
        }

        public int Fps
        {
            set
            {
                foreach (var animationV2 in states)
                {
                    animationV2.Value.FramesPerSecond = value;
                }
            }
        }

        public void AddState(string stateName, AnimationV2 animation)
        {
            if (states.ContainsKey(stateName))
            {
                Debug.Error("The Animation controler already has a state : {0}", stateName);
                return;
            }
            if (currentState == null)
            {
                currentState = stateName;
            }
            
            this.states.Add(stateName,animation);
            this.states[stateName].OnAnimationFinish += (sender, args) => OnAnimationFinishInvoke();
        }

        public void OverWriteState(string stateName, AnimationV2 animation)
        {
            if (!states.ContainsKey(stateName))
            {
                Debug.Error("The animation doesn't have state: {0} to overwrite to..",stateName);
                return;

            }
            this.states[stateName] = animation;
        }
        
        public string CurrentState
        {
            get { return this.currentState; }
            private set
            {
                if (!states.ContainsKey(value))
                {
                    Debug.Error("The animation controler does not have the state: {0}", value);
                }
                else if (currentState != value)
                {
                    if (ResetOnStateChange)
                        this.states[value].Reset();
                      
                    this.currentState = value;
                }
            }
        }

        public void OnAnimationFinishInvoke()
        {
            if (OnAnimationFinish != null)
            {
                OnAnimationFinish(CurrentState);
            }
        }

        public void ChangeState(string statename)
        {
            this.CurrentState = statename;
        }

        public void Update(GameTime gameTime)
        {
            states[currentState].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            states[currentState].Draw(spriteBatch, position,0);
        }
        

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2? origin = null, Vector2? scale = null, SpriteEffects effect = SpriteEffects.None)
        {
            states[currentState].Draw(spriteBatch,position,rotation,origin,scale,effect);
        }
    }
}
