
namespace MonoTinker.Code.Components.Elements
{
    using System;
#if DEBUG
    using DebugGraphics;
#endif

    using Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public enum Origin
    {
        Center,TopLeft, TopRight,BottomLeft,BottomRight
    }

    public class Sprite : ISimpleDrawable,ITransformable
    {
        protected const float NinetyDegreeRotation = (float)(Math.PI/2f);
        protected Texture2D _texture2D;
        protected Rectangle source;
        protected Vector2 origin;
        protected bool isRotated;
        protected SpriteEffects effect;

        #region Constructors

        public Sprite(Texture2D texture2D) : this(texture2D, texture2D.Bounds)
        {

        }

        public Sprite(Texture2D texture2D, Rectangle sourceRect) : this(texture2D, sourceRect, new Vector2(0.5f, 0.5f), false)
        {
            this.Origin = Origin.TopLeft;
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 center, bool isRotated) :
            this(texture, Vector2.Zero, source, center, Color.White, isRotated)
        {

        }

        public Sprite(Texture2D texture, Vector2 position, Rectangle source, Vector2 center, Color color,
            bool isrotated)
        {
            this._texture2D = texture;
            this.ScaleF = 1;
            this.Position = position;
            this.source = source;
            this.DefaultSource = source;
            this.SpriteCenter = isRotated
                ? new Vector2(source.Width * (1 - center.Y), source.Height * center.X)
                : new Vector2(source.Width * center.X, source.Height * center.Y);
            this.isRotated = isrotated;
            this.Origin = Origin.Center;
            this.Clr = color;
        }

        #endregion

        #region Properties

        public Rectangle DefaultSource { get; }

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
                }
                else if (value > this.DefaultSource.Width)
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
                this.source.Height = value > this.DefaultSource.Height ? this.DefaultSource.Height : value;
            }
        }

        public Texture2D Texture
        {
            get { return this._texture2D; }
        }

        public Vector2 Size
        {
            get { return this.DefaultSource.Size.ToVector2() * this.Scale; }
        }

        public Vector2 SpriteCenter { get; private set; }

        public Vector2 Scale { get; set; }

        public float ScaleF
        {
            get { return (this.Scale.X + this.Scale.Y) / 2; }
            set { this.Scale = Vector2.One * value; }
        }

        public float Rotation { get; set; }

        public Vector2 Position { get; set; }

        public float PosX
        {
            get { return this.Position.X; }
            set { this.Position = new Vector2(value,Position.Y); }
        }

        public float PosY
        {
            get { return this.Position.Y; }
            set { this.Position = new Vector2(Position.X,value); }
        }

        public float LayerDepth { get; set; }

        public SpriteEffects Effect
        {
            set { this.effect = value; }
        }

        public Origin Origin
        {
            set
            {
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
            get { return this.origin; }
            set { this.origin = value; }
        }

        public Color Clr { get; set; }

        #endregion

        #region Methods

        public bool Contains(Vector2 point)
        {
            Rectangle position = new Rectangle(this.Position.ToPoint(), this.DefaultSource.Size);
            return position.Contains(point);
        }

        public void RestoreDefaultSource()
        {
            this.Source = this.DefaultSource;
        }

        public Sprite DirectClone()
        {
            var toReturn = new Sprite(this.Texture, new Rectangle(this.DefaultSource.Location, this.DefaultSource.Size));
            toReturn.Clr = this.Clr;
            toReturn.Effect = this.effect;
            toReturn.Position = new Vector2(this.Position.X,this.Position.Y);
            toReturn.SpriteCenter = this.SpriteCenter;
            toReturn.Scale = this.Scale;
            toReturn.Rotation = this.Rotation;
            toReturn.OriginCustom = this.origin;
            return toReturn;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch,this.Position,this.Rotation,this.Scale);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale)
        {
            Draw(spriteBatch,position,rotation,scale,this.Clr);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            spriteBatch.Draw(this.Texture,position,this.Source,color, this.isRotated ? rotation - NinetyDegreeRotation : rotation, origin,scale,effect,0);
#if DEBUG
            DebugShapes.DrawRectagnle(spriteBatch,position -  origin*scale,this.Size,1,Color.Red);
#endif
        }

        #endregion
    }
}
