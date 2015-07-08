namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Utils;
    using Components.Interfaces;
    using Components.Elements.DebugGraphics;
   

    public class Text : Fadeable , ITransformable
    {
        private SpriteFont font;

        public Color Clr;
        private Rectangle bounds;
        private Vector2 position;
        private Vector2 scale;
        private StringBuilder contents;

        public Text(SpriteFont font, Vector2 position, string contents, float alpha = 1, bool isVisible = true)
        {
            this.font = font;
            this.contents = new StringBuilder(contents);
            this.ScaleF = 1;
            this.isVisible = isVisible;
            this.Position = position;
            this.bounds = new Rectangle(this.Position.ToPoint(),this.Size.ToPoint());
            this.Clr = Color.White;
            this.alpha = alpha;
            this.DefaultAlpha = 1;
        }

        public Vector2 Size
        {
            get { return this.font.MeasureString(this.Contents)*Scale; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                bounds.Location = position.ToPoint();
                bounds.Size = Size.ToPoint();
            }
        }

        public Vector2 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
                bounds.Location = position.ToPoint();
                bounds.Size = Size.ToPoint();
            }
        }

        public float Rotation { get; set; }

        public float ScaleF
        {
            get { return (this.Scale.X + this.Scale.Y) / 2; }
            set { this.Scale = new Vector2(value, value); }
        }

        public float PosX
        {
            get { return this.Position.X; }
            set
            {
                this.Position = new Vector2(value, Position.Y);
            }
        }

        public float PosY
        {
            get { return this.Position.Y; }
            set
            {
                this.Position = new Vector2(Position.X, value);
            }
        }

        public bool Contains(Vector2 pos)
        {
            return bounds.Contains(pos - new Vector2(0,5.5f));
        }

        public Action<Text> OnLabelChange { get; set; }

        public string Contents
        {
            get
            {
                return this.contents.ToString();
            }
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.contents = new StringBuilder("...");
                    if (OnLabelChange == null)
                    {
                        return;
                    }
                    OnLabelChange.Invoke(this);
                }
                else
                {
                    if (value == this.Contents)
                    {
                        return;
                    }
                    this.contents = new StringBuilder(value);
                    if (OnLabelChange == null)
                    {
                        return;
                    }
                    OnLabelChange.Invoke(this);
                }
            }
        }

        public void Append(string text)
        {
            this.contents.Append(text);
        }

        public void Append(char c)
        {
            this.contents.Append(c);
        }

        public void RemoveLast()
        {
            this.contents.Remove(contents.Length - 1, 1);
        }


        public virtual void Update(GameTime gameTime)
        {
            if (this.fadeIn || this.fadeOut)
            {
                this.Transition();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.font,this.Contents,this.Position,this.Clr * this.Alpha,this.Rotation,Vector2.Zero, this.Scale,SpriteEffects.None, 0);
        }
    }
}
