using MonoTinker.Code.Components.GameComponents;

namespace MonoTinker.Code.Components.UI
{
    using System;

    using Elements;
    using Managers;
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class ItemTile : Sprite
    {
        private Item item;
        private ToolTip toolTip;
        private Vector2 toolTipOffset;
        private Vector2 positionOffset;
        private bool hoverOver;
        private bool clicked;
        private double timeElapsed;
        private double timeToUpdate;
        private bool toBeDeleted;

        public ItemTile(Sprite itemSprite, Vector2 position, Item item)
            : base(itemSprite.Texture, itemSprite.DefaultSource)
        {
            base.Position = position;
            base.Origin = Origin.TopLeft;
            this.item = item;
            this.Init();
        }

        private void Init()
        {
            this.toolTip = new ToolTip(this.Position, ScreenManager.Device, this.item);
            toolTip.IsVisible = false;
            toolTip.FadeSpeed = 10;
            toolTip.Transitioning = true;
            toolTipOffset = new Vector2(this.DefaultSource.Width, -this.toolTip.Size.Y);
            toolTip.Position = this.Position + this.toolTipOffset;
            timeToUpdate = TimeSpan.FromSeconds(0.2).TotalSeconds;
        }

        public Vector2 PositionOffset
        {
            get { return this.positionOffset; }
            set { this.positionOffset = value; }
        }

        public bool Selected
        {
            get { return this.clicked; }
            set { this.clicked = value; }
        }

        public bool ToBeDeleted
        {
            get { return this.toBeDeleted; }
            set { this.toBeDeleted = value; }
        }

        public ToolTip Tip
        {
            get { return this.toolTip; }
        }

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
                return;
            }
            if (hoverOver && !clicked)
            {
                if (!GameManager.ItemMoving)
                {
                    clicked = InputHandler.MouseDownOnce("left");
                }
                timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed >= timeToUpdate)
                {
                    if (this.Position.X + this.toolTip.Size.X + 30 > ScreenManager.ScreenDimensions.X)
                    {
                        this.toolTip.Position = new Vector2(this.Position.X - this.toolTip.Size.X, this.toolTip.Position.Y);
                    }
                    else if (this.Position.X + this.toolTip.Size.X - 30 < ScreenManager.ScreenDimensions.X)
                    {
                        toolTip.Position = this.Position + this.toolTipOffset;
                    }
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

        public void UpdateFromInventory(GameTime gameTime)
        {
            if (clicked)
            {
                this.toolTip.IsVisible = false;
                toolTip.Update(gameTime);
                this.Move();
                return;
            }
            if (hoverOver && !clicked)
            {
                if (!GameManager.ItemMoving)
                {
                    clicked = InputHandler.MouseDownOnce("left");
                }
                timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed >= timeToUpdate)
                {
                    if (this.positionOffset.X + this.toolTip.Size.X + 30 > ScreenManager.ScreenDimensions.X)
                    {
                        this.toolTip.Position = new Vector2(this.Position.X - this.toolTip.Size.X, this.toolTip.Position.Y);
                    }
                    else if (this.positionOffset.X + this.toolTip.Size.X - 30 < ScreenManager.ScreenDimensions.X)
                    {
                        toolTip.Position = this.Position + this.toolTipOffset;
                    }
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
        

        public void Move()
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

        public void DrawWithoutTip(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        

        public new ItemTile DirectClone()
        {
            ItemTile clone = new ItemTile(base.DirectClone(),this.Position,new Item(this.item.Name,
                new Stats(this.item.Status.Strenght,
                            this.item.Status.Agility,
                            this.item.Status.Vitality,
                            this.item.Status.Intellect,
                            this.item.Status.Wisdom),this.item.Rarity));
            return clone;
        }

        public ItemTile DirectCloneFromInventory()
        {
            ItemTile clone = new ItemTile(base.DirectClone(), this.PositionOffset, new Item(this.item.Name,
                new Stats(this.item.Status.Strenght,
                            this.item.Status.Agility,
                            this.item.Status.Vitality,
                            this.item.Status.Intellect,
                            this.item.Status.Wisdom),this.item.Rarity));
            clone.Selected = true;
            return clone;
        }
    }
}
