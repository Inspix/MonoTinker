

using OpenTK.Graphics;

namespace MonoTinker.Code.Components.Tiles
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Elements;


    public enum LightSimpleEffect
    {
        None, Shimmering, Puslating,Fading
    }
    public class LightTile : Tile
    {
        public bool Active;
        private Random random;
        private double timeToUpdate;
        private bool[] triggers;
        private int[] counters;
        private int[] values;
        private new LightSimpleEffect effect;

        public LightTile(Sprite sprite, Vector2 position,LightSimpleEffect effect = LightSimpleEffect.None,
            float opacity = 1) : base(sprite, position)
        {
            base.Alpha = opacity;
            this.effect = effect;
            this.Init();
            
        }

        public new LightSimpleEffect Effect
        {
            get { return this.effect; }
            set
            {
                this.effect = value;
                this.Init(); 
            }
        }

        public void Update(GameTime gameTime)
        {
            //Todo More effects
            switch (effect)
            {
                case LightSimpleEffect.Fading: Fading(gameTime);
                    break;
                case LightSimpleEffect.Shimmering:
                    Shimmering(gameTime);
                    break;
            }
        }

        private void Init()
        {
            Active = true;
            switch (effect)
            {
                case LightSimpleEffect.Fading:
                    timeToUpdate = TimeSpan.FromSeconds(0.2f).TotalSeconds;
                    break;
                case LightSimpleEffect.Shimmering:
                    this.Alpha = 10;
                    timeToUpdate = TimeSpan.FromSeconds(0.01f).TotalSeconds;

                    counters = new int[1];
                    values = new int[2];
                    triggers = new[] {true,true};
                    random = new Random();
                    break;
            }
        }
        
        private void Shimmering(GameTime gameTime)
        {
            if (triggers[0])
            {
                values[0] = random.Next(100, 250);
                values[1] = random.Next(values[0]+5, values[0]+10);
                triggers[0] = false;
            }
          
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed >= timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                counters[0]++;
                if (counters[0] >= values[0] && counters[0] <= values[1] && triggers[1])
                {
                    this.Alpha += 12;
                    
                }
                if (counters[0] > values[1] && triggers[1])
                {
                    triggers[1] = false;
                }
                if (!triggers[1])
                {
                    this.Alpha -= 6;
                    if (this.Alpha <= 0)
                    {
                        triggers[1] = true;
                        triggers[0] = true;
                        counters[0] = 0; 

                    }
                }
            }
        }

        private double timeElapsed;
        
        private void Fading(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (this.timeElapsed >= this.timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                this.Alpha -= 2;
                if (Alpha <= 1)
                {
                    Active = true;
                }
            }
        }
    }
}
