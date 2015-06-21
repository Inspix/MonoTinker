using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components
{
    public enum Origin
    {
        Center,TopLeft, TopRight,BottomLeft,BottomRight
    }

    public class Sprite : ISimpleDrawable
    {
        protected Texture2D _texture2D;
        protected const float NinetyDegreeRotation = (float)(Math.PI/2f);
        public Transform Transform;
        protected Rectangle? source;
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
            get { return source ?? this._texture2D.Bounds; }
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
            this.source = null;
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
            this.Size = sourceRect.Size.ToVector2();
            this.Transform = new Transform();
            this.Center = _texture2D.Bounds.Center.ToVector2();
            this.Origin = Origin.Center;
            this.Clr = Color.White;
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 center,Vector2 size, bool isRotated)
        {
            this._texture2D = texture;
            this.source = source;
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
            spriteBatch.Draw(Texture,Transform.Position,Source,Clr,isRotated ? Transform.Rotation - NinetyDegreeRotation : Transform.Rotation,origin,Transform.Scale,effect,LayerDepth);
        }

        public Vector2 Position
        {
            get
            {
                return this.Transform.Position;
            }
            set { this.Transform.Position = value; }
        }
    }
}
