using System;
using Microsoft.Xna.Framework;

namespace MonoTinker.Code.Components
{
    public class AnimationV2
    {
        private string[] sprites;
        private SpriteAtlas atlas;
        private double timeElapsed;
        private double timeToUpdate;
        private int currentFrame;

        public int FramesPerSecond
        {
            set { this.timeToUpdate = (1f/value); }
        }

        public AnimationV2(string[] frames, SpriteAtlas source, int fps = 30)
        {
            this.sprites = frames;
            this.atlas = source;
            this.FramesPerSecond = fps;
        }

        public Sprite this[int index]
        {
            get { return this.atlas[sprites[index]]; }
        }

        public Sprite CurrentFrame
        {
            get { return this[currentFrame]; }
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                Console.WriteLine(sprites[currentFrame]);
                this.currentFrame++;
                
                if (currentFrame >= this.sprites.Length)
                {
                    currentFrame = 0;
                }
            }
        }
        
    }
}
