
namespace MonoTinker.Code.Components.Elements
{
    using System;

    using Interfaces;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

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
                        this.origin = this.Size;
                        break;
                    case Origin.Center:
                        this.origin = this.SpriteCenter;
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
                }else if (value > this.DefaultSource.Width)
                {
                    this.source.Width = this.DefaultSource.Width;
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

        public Vector2 SpriteCenter { get; private set; }

        public Texture2D Texture
        {
            get { return this._texture2D; }
        }

        public Sprite(Texture2D texture2D)
        {
            this._texture2D = texture2D;
            this.source = texture2D.Bounds;
            this.defaultSource = this.source;
            this.Size = texture2D.Bounds.Size.ToVector2();
            this.Transform = new Transform();
            this.SpriteCenter = this._texture2D.Bounds.Center.ToVector2();
            this.Origin = Origin.TopLeft;
            this.Clr = Color.White;
        }
        public Sprite(Texture2D texture2D, Rectangle sourceRect)
        {
            this._texture2D = texture2D;
            this.source = sourceRect;
            this.defaultSource = sourceRect;
            this.Size = sourceRect.Size.ToVector2();
            this.Transform = new Transform();
            this.SpriteCenter = Size/2f;
            this.Origin = Origin.TopLeft;
            this.Clr = Color.White;
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 center,Vector2 size, bool isRotated)
        {
            this._texture2D = texture;
            this.source = source;
            this.defaultSource = source;
            this.SpriteCenter = isRotated
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
            spriteBatch.Draw(this.Texture,this.Position,this.Source,this.Clr,this.isRotated ? this.Transform.Rotation - NinetyDegreeRotation : this.Transform.Rotation,this.origin,this.Transform.Scale,this.effect,this.LayerDepth);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale)
        {
            spriteBatch.Draw(this.Texture,position,this.Source,this.Clr, this.isRotated ? rotation - NinetyDegreeRotation : rotation, origin,scale,effect,0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            spriteBatch.Draw(this.Texture,position,this.Source,color, this.isRotated ? rotation - NinetyDegreeRotation : rotation, origin,scale,effect,0);

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
            var toReturn = new Sprite(this.Texture,new Rectangle(this.DefaultSource.Location,this.DefaultSource.Size));
            toReturn.OriginCustom = this.origin;
            return toReturn;
        }

        public Sprite DirectClone()
        {
            var toReturn = new Sprite(this.Texture, new Rectangle(this.DefaultSource.Location, this.DefaultSource.Size));
            toReturn.Clr = this.Clr;
            toReturn.Effect = this.effect;
            toReturn.Position = new Vector2(this.Position.X,this.Position.Y);
            toReturn.SpriteCenter = this.SpriteCenter;
            toReturn.Transform.Scale = this.Transform.Scale;
            toReturn.Transform.Rotation = this.Transform.Rotation;
            toReturn.OriginCustom = this.origin;
            return toReturn;
        }
    }
}
