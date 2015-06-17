using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    public class Sprite
    {
        private Texture2D _texture2D;
        private Rectangle source;

        public Rectangle Source
        {
            get { return this.source; }
        }

        protected int SourceX
        {
            set { this.source.X = value; }
        }

        protected int SourceY
        {
            set { this.source.Y = value; }
        }

        public Vector2 Size { get; }
        public Vector2 Center { get; }

        public Texture2D Texture
        {
            get { return this._texture2D; }
        }

        public Sprite(Texture2D texture2D)
        {
            this._texture2D = texture2D;
            this.Size = new Vector2(_texture2D.Width,_texture2D.Height);
            this.Center = new Vector2(_texture2D.Width/2f, _texture2D.Height/2f);
            this.source = new Rectangle(0,0,(int)Size.X,(int)Size.Y);
        }

        public Sprite(string path, ContentManager content): this(content.Load<Texture2D>(path))
        {
        }

        public Sprite(Texture2D texture2D, Rectangle sourceRect)
        {
            this._texture2D = texture2D;
            this.source = sourceRect;
            this.Size = new Vector2(sourceRect.Width,sourceRect.Height);
        }
    }
}
