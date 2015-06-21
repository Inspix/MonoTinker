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
        private LightSimpleEffect effect;
        public Light(Texture2D texture, Rectangle source, Vector2 position,LightSimpleEffect effect = LightSimpleEffect.None,
            int opacity = 255,Vector2 scale = default(Vector2)) : base(texture, source, position)
        {
            this.center = source.Center.ToVector2();
            this.scale = scale == default (Vector2) ? Vector2.One : scale;
            this.Opacity = opacity;
            this.effect = effect;
            Active = true;
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
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,Position,source,Clr,0,Vector2.Zero, scale,SpriteEffects.None, 0);
        }


        private void Shimmering(GameTime gameTime)
        {
            //TODO Shimmering "Effect" 
        }

        private double timeElapsed;
        private double timeToUpdate = TimeSpan.FromSeconds(0.1).TotalSeconds;
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
