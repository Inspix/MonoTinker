

namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;

    using Elements;

    using Managers;
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Inventory : InterfaceElement
    {
        private int rows;
        private Keys OpenKey = Keybindings.InventoryKey;
        private Button closeButton;
        private Button moveButton;
        private ItemTile[] items;
        private int slotCount;
        public Inventory(Vector2 position, GraphicsDevice device, int rows)
            : base(position, device)
        {
            this.rows = rows;
            this.items = new ItemTile[rows*4];
            this.Init();
        }

        private void Init()
        {
            this.Transitioning = true;
            this.OverrideDrawElements = true;

            Sprite handle = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderTop).DirectClone();
            Sprite handle2 = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderTop).DirectClone();
            handle.Position = Vector2.UnitX * 13;
            handle2.Position = Vector2.UnitX * 165;

            Height += handle.SourceHeight - 2;
            Elements.Add("Handle",handle);
            Elements.Add("Handle2",handle2);

            closeButton = new Button(handle2.Position + Vector2.One * 5,
                AssetManager.Instance.Get<Sprite>(Sn.Menu.RedBall).DirectClone(),
                AssetManager.Instance.Get<Sprite>(Sn.Menu.RedBallHover).DirectClone(),
                AssetManager.Instance.Get<Sprite>(Sn.Menu.RedBallClick).DirectClone());
            moveButton = new Button(handle.Position + Vector2.One * 5,
                AssetManager.Instance.Get<Sprite>(Sn.Menu.BlueBall).DirectClone(),
                AssetManager.Instance.Get<Sprite>(Sn.Menu.BlueBallHover).DirectClone(),
                AssetManager.Instance.Get<Sprite>(Sn.Menu.BlueBallClick).DirectClone());
            moveButton.ClickType = ClickType.Toggle;


            slotCount = 1;
            for (int i = 1; i <= rows; i++)
            {
                Sprite left = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemLeftSilver).DirectClone();
                Sprite middle = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemMiddleSilver).DirectClone();
                Sprite middle2 = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemMiddleSilver).DirectClone();
                Sprite right = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemRightSilver).DirectClone();

                if (Width == 0)
                {
                    this.Width += left.DefaultSource.Width;
                    this.Width += middle.DefaultSource.Width*2;
                    this.Width += right.DefaultSource.Width;
                }
                left.Position = Vector2.UnitY*this.Height;
                middle.Position = left.Position + Vector2.UnitX * left.SourceWidth;
                middle2.Position = middle.Position + Vector2.UnitX * middle.SourceWidth;
                right.Position = middle2.Position + Vector2.UnitX * middle2.SourceWidth;
                Height += left.SourceHeight;
                this.Elements.Add("slot" + slotCount++, left);
                this.Elements.Add("slot" + slotCount++, middle);
                this.Elements.Add("slot" + slotCount++, middle2);
                this.Elements.Add("slot" + slotCount++, right);

            }

            this.RenderTarget2D = new RenderTarget2D(base.Device, this.Width, this.Height);
        }

        public void AddRows(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Sprite left = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemLeftSilver).DirectClone();
                Sprite middle = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemMiddleSilver).DirectClone();
                Sprite middle2 = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemMiddleSilver).DirectClone();
                Sprite right = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemRightSilver).DirectClone();
                
                left.Position = Vector2.UnitY * this.Height;
                middle.Position = left.Position + Vector2.UnitX * left.SourceWidth;
                middle2.Position = middle.Position + Vector2.UnitX * middle.SourceWidth;
                right.Position = middle2.Position + Vector2.UnitX * middle2.SourceWidth;
                Height += left.SourceHeight;
                this.RenderTarget2D = new RenderTarget2D(Device, Width, Height);
                this.Elements.Add("slot" + slotCount++, left);
                this.Elements.Add("slot" + slotCount++, middle);
                this.Elements.Add("slot" + slotCount++, middle2);
                this.Elements.Add("slot" + slotCount++, right);
                ItemTile[] newArray = new ItemTile[items.Length + 4];
                Array.Copy(items,newArray,items.Length);
                items = newArray;
            }
        }

        public ItemTile this[int x]
        {
            get { return this.items[x]; }
        }


        public override void Update(GameTime gameTime)
        {
            if (OpenKey.DownOnce())
            {
                this.IsVisible = true;
            }

            if (!this.IsVisible) return;
           
            Vector2 mousePos = InputHandler.MousePos() - this.Position;
            Vector2 delta = InputHandler.MouseDelta();
            closeButton.Over(mousePos);
            if (fadeIn || fadeOut)
            {
                
            }
            else
            {
                moveButton.Over(mousePos);
            }
            foreach (var spriteAtla in Elements.Where(s => s.Key.Contains("slot")))
            {
                bool result = spriteAtla.Value.Contains(mousePos);
                spriteAtla.Value.Clr = result ? Color.LightGray : Color.White;
                if (GameManager.ItemMoving && result)
                {
                    int index = int.Parse(spriteAtla.Key.Replace("slot", ""));
                    bool testForItem = items[index-1] == null;
                    if (testForItem)
                    {
                        if (InputHandler.MouseDownOnce("left"))
                        {
                            GameManager.PutItemToInventory(this, index-1);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (moveButton.Clicked)
            {
                this.Move(delta);
            }

            if (closeButton.Clicked)
            {
                this.IsVisible = false;
            }
            var item = items.FirstOrDefault(s => s != null && s.Selected);
            if (item != null && !GameManager.ItemMoving)
            {
                item.PositionOffset = this.Position + item.Position;
                GameManager.GetItemFromInventory(this,Array.IndexOf(items,item));
            }

            if (!GameManager.ItemMoving)
            {
                foreach (var itemTile in items.Where(itemTile => itemTile != null))
                {
                    itemTile.Over(mousePos);
                    itemTile.UpdateFromInventory(gameTime);
                } 
            }

            closeButton.Update(gameTime);
            moveButton.Update(gameTime);

            if (Keys.Enter.DownOnce())
            {
                this.AddRows(1);
            }
            base.Update(gameTime);
        }

        private void Move(Vector2 delta)
        {
            this.Position += delta;
            foreach (var itemTile in items.Where(itemTile => itemTile != null))
            {
                itemTile.PositionOffset = this.Position + Vector2.UnitX * 100;
            }

        }

        public override void DrawElements()
        {
            base.DrawElements();
            closeButton.Draw(Batch);
            moveButton.Draw(Batch);
            foreach (var itemTile in items)
            {
                if (itemTile != null)
                {
                    itemTile.DrawWithoutTip(Batch);
                }
            }
            Batch.End();
            Device.SetRenderTarget(null);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (var itemTile in items)
            {
                if (itemTile != null)
                {
                    itemTile.Tip.DrawAtPosition(spriteBatch,this.Position);
                }
            }
        }

        public void AddItemToSlot(ItemTile item, int slot)
        {
            item.Position = this.Elements["slot" + slot].Position + ((slot % 4 != 1) ? new Vector2(7,3) : new Vector2(15, 3));
            items[slot-1] = item.DirectClone();
        }

        public void RemoveItemFromSlot(int slotnumber)
        {
            items[slotnumber-1] = null;
        }
    }
}
