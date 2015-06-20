using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    public enum AnimInfo
    {
        PerFrame,Relative
    }

    public class Animation : SpriteSheet
    {
        private int framesMissing;
        private Vector2 frameSize;
        private int frame;
        private double timeElapsed;
        private double timeToUpdate;
        private AnimInfo info;

        public double FramesPerSecond
        {
            set { this.timeToUpdate = (1f/value); }
        }

        public Sprite CurrentFrame
        {
            get
            {
                return this[frame];
            }
        }
        
        public Vector2 Center
        {
            get
            {
                switch (info)
                {
                    case AnimInfo.PerFrame:
                        return this[frame].Center;
                    default:
                        return this.frameSize / 2f;
                }
            }
        }

        public Vector2 Size
        {
            get
            {
                switch (info)
                {
                    case AnimInfo.PerFrame:
                        return this[frame].Size;
                    default:
                        return this.frameSize;
                }
            }
        }

        public int FramesMissing { set { this.framesMissing = value; } }

        public Animation(Texture2D texture2D,Vector2 frameSize) : base(texture2D, frameSize)
        {
            this.frameSize = frameSize;
            info = AnimInfo.Relative;
            this.FramesPerSecond = this.Count;
        }

        public Animation(Texture2D texture2D, Vector2 frameSize, int missingframes) : base(texture2D, frameSize,missingframes)
        {
            this.frameSize = frameSize;
            info = AnimInfo.Relative;
            this.FramesPerSecond = this.Count - missingframes;
        }

        public Animation(Texture2D texture2D, Vector2 frameSize, int fps,int missingFrames) : base(texture2D, frameSize, missingFrames)
        {
            this.frameSize = frameSize;
            info = AnimInfo.Relative;
            this.FramesPerSecond = fps;
        }


        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                this.frame++;
                if (frame >= this.Count-framesMissing)
                {
                    frame = 0;
                }
            }
        }
    }
}
