namespace MonoTinker.Code.Components.UI
{
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

        public Button(Vector2 position, Sprite normalState, Sprite hoverState, Sprite clickState)
        {
            this.normal = normalState.Clone() as Sprite;
            this.hover = hoverState.Clone() as Sprite;
            this.click = clickState.Clone() as Sprite;
            normal.Position = position;
            hover.Position = position;
            click.Position = position;
            this.bounds = new Rectangle(position.ToPoint(), normalState.DefaultSource.Size);
        }

        public ClickType Type
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
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (hovering)
            {
                if (clicked)
                {
                    click.Draw(spriteBatch);
                }
                else
                {
                    hover.Draw(spriteBatch);
                }
                
            }
            else
            {
                normal.Draw(spriteBatch);
            }
        }
    }
}
