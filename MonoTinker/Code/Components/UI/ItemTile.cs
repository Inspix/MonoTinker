namespace MonoTinker.Code.Components.UI
{
    using System;

    using Managers;
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class ItemTile : Sprite
    {
        private string itemName;

        private ToolTip toolTip;

        private Vector2 toolTipOffset;

        private bool hoverOver;
        private bool clicked;

        private double timeElapsed;

        private double timeToUpdate;

        public ItemTile(Sprite itemSprite, Vector2 position, string itemname, string prefix = null)
            : base(itemSprite.Texture,itemSprite.DefaultSource)
        {
            base.Position = position;
            base.Origin = Origin.TopLeft;
            this.itemName = itemname;
            this.Prefix = prefix;
            this.Init();
        }

        private void Init()
        {
            this.toolTip = new ToolTip(this.Position,ScreenManager.Device,new string[] {itemName,Prefix});
            toolTip.IsVisible = false;
            toolTip.FadeSpeed = 10;
            toolTip.Transitioning = true;
            toolTipOffset = new Vector2(this.DefaultSource.Width, -this.toolTip.Size.Y);
            toolTip.Position = this.Position + this.toolTipOffset;
            timeToUpdate = TimeSpan.FromSeconds(0.2).TotalSeconds;

        }

        public string ItemName
        {
            get
            {
                return this.itemName;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Debug.Error("The item name cannot be null");
                    return;
                }
                this.itemName = value;
            }
        }

        public string Prefix { get; private set; }

        public bool Over(Vector2 position)
        {
            Rectangle rect = new Rectangle(this.Position.ToPoint(),this.DefaultSource.Size);
            bool result = rect.Contains(position);
            if (result)
            {
                Console.WriteLine(position + " " + this.Position);
            }
            this.hoverOver = result;
            return result;
        }

        public void Update(GameTime gameTime)
        {
            if (clicked)
            {
                this.toolTip.IsVisible = false;
                toolTip.Update(gameTime);
                this.Move();
                clicked = !InputHandler.MouseDownOnce("left");
                return;
            }
            if (hoverOver && !clicked)
            {
                clicked = InputHandler.MouseDownOnce("left");
                timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed >= timeToUpdate)
                {
                    this.toolTip.IsVisible = true;
                }
            }
            else
            {
                timeElapsed = 0;
                this.toolTip.IsVisible = false;
            }
            toolTip.Update(gameTime);

        }

        private void Move()
        {
            Vector2 movement = InputHandler.MouseDelta();
            this.Position += movement;
            this.toolTip.Position += movement;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            this.toolTip.DrawElements();
            this.toolTip.Draw(spriteBatch);
        }
    }
}
