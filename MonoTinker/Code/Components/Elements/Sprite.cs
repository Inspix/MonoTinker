
namespace MonoTinker.Code.Components
{
    using System;
    using System.Runtime.CompilerServices;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Elements;
    using Interfaces;


    public enum Origin
    {
        Center,TopLeft, TopRight,BottomLeft,BottomRight
    }

    public class Sprite : ISimpleDrawable,ICloneable
    {
        protected Texture2D _texture2D;
        protected const float NinetyDegreeRotation = (float)(Math.PI/2f);
        public Transform Transform;
        protected Rectangle source;

        protected Rectangle defaultSource;
        protected SpriteEffects effect;
        protected Vector2 origin;
        protected bool isRotated;

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
            get
            {
                return this.source;
            }
            private set
            {
                this.source = value;
            }
        }

        public Rectangle DefaultSource
        {
            get
            {
                return this.defaultSource;
            }
        }

        public int SourceWidth
        {
            get
            {
                return this.source.Width;
            }
            set
            {
                if (value < 0)
                {
                    this.source.Width = 0;
                }else if (value > DefaultSource.Width)
                {
                    this.source.Width = DefaultSource.Width;
                }
                else
                {
                    this.source.Width = value;
                }
            }
        }

        public int SourceHeight
        {
            get
            {
                return this.source.Height;
            }
            set
            {
                this.source.Height = value > this.defaultSource.Height ? this.defaultSource.Height : value;
            }
        }

        public bool Contains(Vector2 point)
        {
            Rectangle position = new Rectangle(this.Position.ToPoint(),this.DefaultSource.Size);
            return position.Contains(point);
        }


        public void RestoreDefaultSource()
        {
            this.Source = this.defaultSource;
        }
        

        public Color Clr { get; set; }

        public Vector2 Size { get; private set; }

        public Vector2 Center { get; private set; }

        public Texture2D Texture
        {
            get { return this._texture2D; }
        }

        public Sprite(Texture2D texture2D)
        {
            this._texture2D = texture2D;
            this.source = texture2D.Bounds;
            this.defaultSource = source;
            this.Size = texture2D.Bounds.Size.ToVector2();
            this.Transform = new Transform();
            this.Center = _texture2D.Bounds.Center.ToVector2();
            this.Origin = Origin.Center;
            this.Clr = Color.White;
        }
        public Sprite(Texture2D texture2D, Rectangle sourceRect)
        {
            this._texture2D = texture2D;
            this.source = sourceRect;
            this.defaultSource = sourceRect;
            this.Size = sourceRect.Size.ToVector2();
            this.Transform = new Transform();
            this.Center = source.Center.ToVector2();
            this.Origin = Origin.Center;
            this.Clr = Color.White;
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 center,Vector2 size, bool isRotated)
        {
            this._texture2D = texture;
            this.source = source;
            this.defaultSource = source;
            this.Center = isRotated
                ? new Vector2(source.Width*(1 - center.Y), source.Height*center.X)
                : new Vector2(source.Width*center.X, source.Height*center.Y);
            this.isRotated = isRotated;
            this.Size = size;
            this.Transform = new Transform();
            this.Origin = Origin.Center;
            this.Clr = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,Position,Source,Clr,isRotated ? Transform.Rotation - NinetyDegreeRotation : Transform.Rotation,origin,Transform.Scale,effect,LayerDepth);
        }

        public Vector2 Position
        {
            get
            {
                return this.Transform.Position;
            }
            set { this.Transform.Position = value; }
        }

        public object Clone()
        {
            Sprite toReturn = new Sprite(this.Texture,new Rectangle(this.DefaultSource.Location,this.DefaultSource.Size));
            toReturn.OriginCustom = this.origin;
            return toReturn;
        }
    }
}
