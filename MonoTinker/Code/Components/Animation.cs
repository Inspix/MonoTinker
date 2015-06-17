using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    public class Animation : Sprite
    {
        private int framesMissing;
        private readonly int frameWidth;
        private readonly int frameHight;
        private readonly int framesTotal;
        private readonly int framesPerRow;
        private readonly int framesPerCol;
        private int row;
        private int rowFrame;
        private int frame;
        private double timeElapsed;
        private double timeToUpdate;

        public double FramesPerSecond
        {
            set { this.timeToUpdate = (1f/value); }
        }

        public new Vector2 Center 
        {
            get { return new Vector2(this.Source.Width/2f,this.Source.Height/2f);}
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
                rowFrame++;
                SourceX = rowFrame*frameWidth;
                if (rowFrame >= framesPerRow)
                {
                    rowFrame = 0;
                    row++;
                    SourceX = 0;
                    SourceY = row*frameHight;
                    if (frame > framesTotal - this.framesMissing)
                    {
                        frame = 0;
                        row = 0;
                        rowFrame = 0;
                        SourceX = 0;
                        SourceY = 0;
                    }
                }
            }
        }

        public void Reset()
        {
            frame = 0;
            row = 0;
            rowFrame = 0;
            SourceX = 0;
            SourceY = 0;
        }
    }
}
