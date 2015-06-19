using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    public enum Origin
    {
        Center,TopLeft, TopRight,BottomLeft,BottomRight
    }

    public class Sprite
    {
        private Texture2D _texture2D;
        public Transform Transform;
        private Rectangle? source;
        private SpriteEffects effect;
        private Vector2 origin;
        public Color Color;

        public SpriteEffects Effect
        {
            set { this.effect = value; }
        }

        public float LayerDepth;

        public Origin Origin
        {
            set {
                switch (value)
                {
                    case Origin.TopLeft:
                        this.origin = Vector2.Zero;
                        break;
                    case Origin.TopRight:
                        this.origin = new Vector2(this.Size.X, 0);
                        break;
                    case Origin.BottomLeft:
                        this.origin = new Vector2(0, this.Size.Y);
                        break;
                    case Origin.BottomRight:
                        this.origin = Size;
                        break;
                    case Origin.Center:
                        this.origin = Center;
                        break;
                }
            }
        }

        public Vector2 OriginCustom
        {
            set { this.origin = value; }
        }

        public Rectangle Source
        {
            get { return source ?? this._texture2D.Bounds; }
        }
        
        public Vector2 Size
        {
            get { return this._texture2D.Bounds.Size.ToVector2(); } 
        }

        public Vector2 Center { get; private set; }

        public Texture2D Texture
        {
            get { return this._texture2D; }
        }

        public Sprite(Texture2D texture2D)
        {
            this._texture2D = texture2D;
            this.source = null;
            this.Transform = new Transform();
            this.Center = _texture2D.Bounds.Center.ToVector2();
            this.Origin = Origin.Center;
            this.Color = Color.White;
        }

        public Sprite(string path, ContentManager content): this(content.Load<Texture2D>(path))
        {
        }

        public Sprite(Texture2D texture2D, Rectangle sourceRect)
        {
            this._texture2D = texture2D;
            this.source = sourceRect;
            this.Transform = new Transform();
            this.Center = _texture2D.Bounds.Center.ToVector2();
            this.Origin = Origin.Center;
            this.Color = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,Transform.Position,Source,Color,Transform.Rotation,origin,Transform.Scale,effect,LayerDepth);
        }
    }
}
