using System;
using System.Text;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.UI
{
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Text : Fadeable , ITransformable
    {
        private SpriteFont font;

        public Color Clr;

        private StringBuilder contents;

        public Text(SpriteFont font, Vector2 position, string contents, float alpha = 1, bool isVisible = true)
        {
            this.font = font;
            this.isVisible = isVisible;
            this.Position = position;
            this.contents = new StringBuilder(contents);
            this.Clr = Color.White;
            this.alpha = alpha;
            this.DefaultAlpha = 1;
            this.ScaleF = 1;
        }

        public Vector2 Size
        {
            get { return this.font.MeasureString(this.Contents)*Scale; }
        }

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; }

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
