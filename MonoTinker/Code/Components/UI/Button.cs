namespace MonoTinker.Code.Components.UI
{
    using System;

#if DEBUG
    using Elements.DebugGraphics; 
#endif

    using Elements;
    using Extensions;
    using Interfaces;
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public enum ClickType
    {
        Single,
        Toggle,
        Continuous
    }

    public class Button : Fadeable, IAdvancedDrawable,ITransformable
    {
        #region Fields

        private Vector2 position;
        private Vector2 scale;
        private Sprite normal;
        private Sprite hover;
        private Sprite click;
        private Rectangle bounds;
        private bool hovering;
        private bool clicked;
        private ClickType type;
        private string tag;
        private bool inflate;
        private Vector2 inflateSize;
        private bool callbackOnClick;
        private Action simpleClickCallback;
        private Action<string> clickCallback; 
        #endregion

        public Button(Vector2 position, Sprite normalState, Sprite hoverState, Sprite clickState)
        {
            this.normal = normalState;
            this.hover = hoverState;
            this.click = clickState;
            this.position = position;
            this.bounds = new Rectangle(position.ToPoint(), normalState.DefaultSource.Size);
            this.ScaleF = 1;
        }

        #region Properties

        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                RecalculateBounds();
            }
        }

        public float PosX
        {
            get { return this.position.X; }
            set
            {
                this.position = new Vector2(value,Position.Y);
                RecalculateBounds();
            }
        }

        public float PosY
        {
            get { return this.position.Y; }
            set
            {
                this.position = new Vector2(Position.X, value);
                RecalculateBounds();
            }
        }

        public float Rotation { get; set; }

        public float ScaleF
        {
            get { return (this.Scale.X + this.Scale.Y)/2; }
            set { this.Scale = new Vector2(value,value);}
        }

        public Vector2 Scale
        {
            get { return this.scale; }
            set
            {
                this.scale = value;
                RecalculateBounds();
            }
        }

        public Vector2 Size
        {
            get { return this.normal.Size*this.Scale; }
        }

        public Vector2 InflateBox
        {
            set
            {
                this.inflate = true;
                this.inflateSize = value;
                RecalculateBounds();
            }
        }

        public string Tag
        {
            get { return this.tag; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Debug.Error("Empty button tag...");
                    return;
                }
                this.tag = value;
            }
        }

        public ClickType ClickType
        {
            set
            {
                this.type = value;
            }
        }

        public bool Clicked
        {
            get
            {
                return this.clicked;
            }
        }

        public Action ClickCallback
        {
            private get { return this.simpleClickCallback; }
            set
            {
                this.callbackOnClick = true;
                this.simpleClickCallback = value;
            }
        }

        #endregion

        #region Methods

        public bool Over(Vector2 point)
        {
            bool result = bounds.Contains(point);
            this.hovering = result;
            return result;
        }

        /// <summary>
        /// Working only on single click mode
        /// </summary>
        public void SetClickCallbackWithTag(string buttontag, Action<string> action)
        {
            this.Tag = buttontag;
            this.callbackOnClick = true;
            this.clickCallback = action;
        }

        public void RecalculateBounds()
        {
            this.bounds = new Rectangle(this.Position.ToPoint(), (normal.DefaultSource.Size.ToVector2() * Scale).ToPoint());
            if (inflate)
            {
                this.bounds = bounds.InflateExt(inflateSize);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (fadeIn || fadeOut)
            {
                base.Transition();
            }
            if (type == ClickType.Toggle)
            {
                if (Clicked)
                {
                    this.clicked = !InputHandler.MouseDownOnce("left");
                    return;
                }
                this.clicked = this.hovering && InputHandler.MouseDownOnce("left");
            }
            else if (type == ClickType.Continuous)
            {
                this.clicked = this.hovering && InputHandler.MouseDown("left");
            }
            else
            {
                this.clicked = this.hovering && InputHandler.MouseDownOnce("left");
                if (!clicked || !callbackOnClick) return;
                if (clickCallback != null)
                {
                    clickCallback.Invoke(tag);
                }
                else if (simpleClickCallback != null)
                {
                    simpleClickCallback.Invoke();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                if (hovering)
                {
                    if (clicked)
                    {
                        click.Draw(spriteBatch, Position, Rotation, Scale, click.Clr * Alpha);
                    }
                    else
                    {
                        hover.Draw(spriteBatch, Position, Rotation, Scale, hover.Clr * Alpha);
                    }

                }
                else
                {
                    normal.Draw(spriteBatch, Position, Rotation, Scale, normal.Clr * Alpha);
                }
            }

#if DEBUG
            DebugShapes.DrawRectagnle(spriteBatch, this.bounds.Location.ToVector2(), this.bounds.Size.ToVector2(), 1, Color.Blue);
#endif
        }

        public Button DirectCopy()
        {
            Button copy = new Button(this.Position, normal.DirectClone(), hover.DirectClone(), click.DirectClone());
            copy.ClickType = type;
            copy.Scale = this.Scale;
            copy.Rotation = this.Rotation;
            copy.RecalculateBounds();
            copy.IsVisible = this.IsVisible;
            copy.Transitioning = this.transitioning;
            copy.FadeSpeed = this.fadeSpeed;
            return copy;
        } 
        #endregion

    }
}
