using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components.Tiles
{
    public enum LightSimpleEffect
    {
        None, Shimmering, Puslating,Fading
    }
    public class Light : Tile
    {
        public bool Active;
        private Vector2 center;
        private Vector2 scale;
        private Random random;
        private double timeToUpdate;
        private bool[] triggers;
        private int[] counters;
        private int[] values;
        private LightSimpleEffect effect;
        public Light(Texture2D texture, Rectangle source, Vector2 position,LightSimpleEffect effect = LightSimpleEffect.None,
            int opacity = 255,Vector2 scale = default(Vector2)) : base(texture, source, position)
        {
            this.center = source.Center.ToVector2();
            this.scale = scale == default (Vector2) ? Vector2.One : scale;
            this.Opacity = opacity;
            this.effect = effect;
            Init();
        }

        public int Opacity
        {
            get { return Clr.A; }
            set
            {
                
                Color newc = Clr;
                newc.A = (byte)MathHelper.Clamp(value, 0, 255);
                this.Clr = newc;
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
                    this.Opacity = 10;
                    timeToUpdate = TimeSpan.FromSeconds(0.01f).TotalSeconds;

                    counters = new int[1];
                    values = new int[2];
                    triggers = new[] {true,true};
                    random = new Random();
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,Position,source,Clr,0,Vector2.Zero, scale,SpriteEffects.None, 0);
        }


        private void Shimmering(GameTime gameTime)
        {
            //TODO Shimmering "Effect"
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
                    this.Opacity += 12;
                    
                }
                if (counters[0] > values[1] && triggers[1])
                {
                    triggers[1] = false;
                }
                if (!triggers[1])
                {
                    this.Opacity -= 6;
                    if (this.Opacity == 0)
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
            if (timeElapsed >= timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                this.Opacity -= 2;
                if (Opacity <= 1)
                {
                    Active = true;
                }
            }
        }
    }
}
