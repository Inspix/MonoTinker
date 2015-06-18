using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components
{
    public class Sprite
    {
        private Texture2D _texture2D;
        private Rectangle source;
        private Vector2 scale;

        public Rectangle Source
        {
            get { return this.source; }
        }

        public Vector2 Scale
        {
            get { return this.scale; }
            set
            {
                //this.source.Width = (int)Math.Ceiling(this.Texture.Width * value.X);
                //this.source.Height = (int)Math.Ceiling(this.Texture.Height * value.Y);
                this.scale = value;
                this.Center = new Vector2(this.source.Width/2f,this.source.Height/2f);
            }
        }

        protected int SourceX
        {
            set { this.source.X = value; }
        }

        protected int SourceY
        {
            set { this.source.Y = value; }
        }

        public Vector2 Size
        {
            get { return this.source.SizeVec2(); } 
        }

        public Vector2 Center { get; private set; }

        public Texture2D Texture
        {
            get { return this._texture2D; }
        }

        public Sprite(Texture2D texture2D)
        {
            this._texture2D = texture2D;
            this.source = texture2D.Bounds;
            this.Scale = Vector2.One;
        }

        public Sprite(string path, ContentManager content): this(content.Load<Texture2D>(path))
        {
        }

        public Sprite(Texture2D texture2D, Rectangle sourceRect)
        {
            this._texture2D = texture2D;
            this.source = sourceRect;
            this.Scale = Vector2.One;
        }
    }
}
