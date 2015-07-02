using System;
using MonoTinker.Code.Components.Extensions;

namespace MonoTinker.Code.Components.UI
{
    using Elements;

    using Utils;
    
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public enum ClickType
    {
        Single,
        Toggle,
        Continuous
    }

    public class Button : Fadeable
    {
        private Sprite normal;
        private Sprite hover;
        private Sprite click;
        private Rectangle bounds;
        private bool hovering;
        private bool clicked;
        private ClickType type;
        private Transform transform;
        private bool inflate;
        private Vector2 inflateSize;
        private bool callbackOnClick;
        private Action clickCallback;

        public Button(Vector2 position, Sprite normalState, Sprite hoverState, Sprite clickState)
        {
            this.normal = normalState;
            this.hover = hoverState;
            this.click = clickState;
            this.transform = new Transform(position);
            this.bounds = new Rectangle(position.ToPoint(), normalState.DefaultSource.Size);
        }

        public ClickType ClickType
        {
            set
            {
                this.type = value;
            }
        }

        public Transform Transform
        {
            get { return this.transform; }
        }

        public Vector2 InflateBox
        {
            set
            {
                this.inflate = true;
                this.inflateSize = value;
                this.Position = this.Position;
            }
        }

        public Vector2 Position
        {
            get { return this.transform.Position; }
            set
            {
                this.transform.Position = value;
                this.bounds = new Rectangle(this.transform.Position.ToPoint(),(normal.DefaultSource.Size.ToVector2() * Transform.Scale).ToPoint());
                if (inflate)
                {
                    this.bounds = bounds.InflateExt(inflateSize);
                }
            }
        }

        public Vector2 Size
        {
            get { return this.normal.Size*this.Transform.Scale; }
        }

        public bool Clicked
        {
            get
            {
                return this.clicked;
            }
        }

        /// <summary>
        /// Working only on single click mode
        /// </summary>
        public Action ClickCallback
        {
            private get { return this.clickCallback; }
            set
            {
                this.callbackOnClick = true;
                this.clickCallback = value;
            }
        }

        public bool Over(Vector2 point)
        {
            bool result = bounds.Contains(point);
            this.hovering = result;
            return result;
        }

        public void Update()
        {
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
                if (clicked && callbackOnClick)
                {
                    ClickCallback.Invoke();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (hovering)
            {
                if (clicked)
                {
                    click.Draw(spriteBatch,Transform.Position,Transform.Rotation,Transform.Scale);
                }
                else
                {
                    hover.Draw(spriteBatch, Transform.Position, Transform.Rotation, Transform.Scale);
                }
                
            }
            else
            {
                normal.Draw(spriteBatch, Transform.Position, Transform.Rotation, Transform.Scale);
            }
        }

        
    }
}
