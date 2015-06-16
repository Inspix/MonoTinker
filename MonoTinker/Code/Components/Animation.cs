using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    public class Animation : Sprite
    {
        private int framesMissing;
        private int frameWidth;
        private int frameHight;
        private int framesTotal;
        private int framesPerRow;
        private int framesPerCol;
        private int Row;
        private int RowFrame;
        private int frame;
        private double timeElapsed;
        private double timeToUpdate;

        public double FramesPerSecond
        {
            set { this.timeToUpdate = (1f/value); }
        }

        public int FramesMissing { set { this.framesMissing = value; } }

        public Animation(Texture2D texture2D, Rectangle sourceRect) : base(texture2D, sourceRect)
        {
            this.frameHight = sourceRect.Height;
            this.frameWidth = sourceRect.Width;
            this.framesPerRow = texture2D.Width/frameWidth;
            this.framesPerCol = texture2D.Height/frameHight;
            this.framesTotal = framesPerRow*framesPerCol;
            this.FramesPerSecond = framesTotal;
        }

        public Animation(Texture2D texture2D, Rectangle sourceRect, int fps) : this(texture2D, sourceRect)
        {
            this.FramesPerSecond = fps;
        }

        public Animation(Texture2D texture2D, Rectangle sourceRect, int fps, int framesMissing = 0) : this(texture2D, sourceRect)
        {
            this.FramesPerSecond = fps;
            this.FramesMissing = framesMissing;
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                frame++;
                RowFrame++;
                SourceX = RowFrame*frameWidth;
                if (RowFrame >= framesPerRow)
                {
                    RowFrame = 0;
                    Row++;
                    SourceX = 0;
                    SourceY = Row*frameHight;
                    if (frame > framesTotal - this.framesMissing)
                    {
                        frame = 0;
                        Row = 0;
                        RowFrame = 0;
                        SourceX = 0;
                        SourceY = 0;
                    }
                }

            }
        }

        public void Reset()
        {
            frame = 0;
            Row = 0;
            RowFrame = 0;
            SourceX = 0;
            SourceY = 0;
        }
    }
}
