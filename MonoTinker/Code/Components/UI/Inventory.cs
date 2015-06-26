namespace MonoTinker.Code.Components.UI
{
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
        public Inventory(Vector2 position, GraphicsDevice device, int rows)
            : base(position, device)
        {
            this.rows = rows;
            this.Init();
        }

        private void Init()
        {
            this.Transitioning = true;
            this.OverrideDrawElements = true;
            Sprite handle = AssetManager.Instance.Get<Sprite>(SpriteNames.SliderTop).Clone() as Sprite;
            Sprite handle2 = AssetManager.Instance.Get<Sprite>(SpriteNames.SliderTop).Clone() as Sprite;
            handle.Position = Vector2.UnitX * 13;
            handle2.Position = Vector2.UnitX * 165;
            Height += handle.SourceHeight - 2;
            Elements.Add("Handle",handle);
            Elements.Add("Handle2",handle2);

            closeButton = new Button(handle2.Position + Vector2.One * 5,
                AssetManager.Instance.Get<Sprite>(SpriteNames.RedBall),
                AssetManager.Instance.Get<Sprite>(SpriteNames.RedBallHover),
                AssetManager.Instance.Get<Sprite>(SpriteNames.RedBallClick));
            moveButton = new Button(handle.Position + Vector2.One * 5,
                AssetManager.Instance.Get<Sprite>(SpriteNames.BlueBall),
                AssetManager.Instance.Get<Sprite>(SpriteNames.BlueBallHover),
                AssetManager.Instance.Get<Sprite>(SpriteNames.BlueBallClick));
            moveButton.Type = ClickType.Toggle;



            for (int i = 1; i <= rows; i++)
            {
                Sprite left = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemLeftSilver).Clone() as Sprite;
                Sprite middle = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemMiddleSilver).Clone() as Sprite;
                Sprite middle2 = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemMiddleSilver).Clone() as Sprite;

                Sprite right = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemRightSilver).Clone() as Sprite;

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
                this.Elements.Add("row" + i + "left", left);
                this.Elements.Add("row" + i + "middle", middle);
                this.Elements.Add("row" + i + "middle2", middle2);
                this.Elements.Add("row" + i + "right", right);

            }

            this.RenderTarget2D = new RenderTarget2D(base.Device, this.Width, this.Height);
        }

        public void AddRows(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Sprite left = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemLeftSilver).Clone() as Sprite;
                Sprite middle = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemMiddleSilver).Clone() as Sprite;
                Sprite middle2 = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemMiddleSilver).Clone() as Sprite;
                Sprite right = AssetManager.Instance.Get<Sprite>(SpriteNames.ItemRightSilver).Clone() as Sprite;
                
                left.Position = Vector2.UnitY * this.Height;
                middle.Position = left.Position + Vector2.UnitX * left.SourceWidth;
                middle2.Position = middle.Position + Vector2.UnitX * middle.SourceWidth;
                right.Position = middle2.Position + Vector2.UnitX * middle2.SourceWidth;
                Height += left.SourceHeight;
                this.RenderTarget2D = new RenderTarget2D(Device, Width, Height);
                this.Elements.Add("row" + Elements.Count / 3 + 1 + "left", left);
                this.Elements.Add("row" + Elements.Count / 3 + 1 + "middle", middle);
                this.Elements.Add("row" + Elements.Count / 3 + 1 + "middle2", middle2);
                this.Elements.Add("row" + Elements.Count / 3 + 1 + "right", right);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (OpenKey.DownOnce()) this.IsVisible = true;

            if (!this.IsVisible) return;
           
            Vector2 mousePos = InputHandler.MousePos() - this.Transform.Position;
            closeButton.Over(mousePos);
            moveButton.Over(mousePos);
            foreach (Sprite spriteAtla in Elements.Values)
            {
                spriteAtla.Clr = spriteAtla.Contains(mousePos) ? Color.LightGray : Color.White;
            }

            if (moveButton.Clicked)
            {
                this.Transform.Position = InputHandler.MousePos() - new Vector2(25,10);
            }

            if (closeButton.Clicked)
            {
                this.IsVisible = false;
            }

            closeButton.Update();
            moveButton.Update();

            if (Keys.Enter.DownOnce())
            {
                this.AddRows(1);
            }
            base.Update(gameTime);
        }

        public override void DrawElements()
        {
            base.DrawElements();
            closeButton.Draw(Batch);
            moveButton.Draw(Batch);
            Batch.End();
            Device.SetRenderTarget(null);
            
        }
    }
}
