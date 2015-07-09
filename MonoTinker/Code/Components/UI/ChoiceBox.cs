namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Elements;
    using Elements.DebugGraphics;
    using Extensions;
    using Managers;
    using Utils;

    public enum ChoiceBoxType
    {
        Item, CharacterInfo
    }

    public class ChoiceBox : InterfaceElement
    {
        private ChoiceBoxType type;
        private List<ItemTile> items;
        private List<TextBox> textboxes; 
        private List<Action> callbacks;
        private Sprite checkbox;
        private bool changing;
        private int count;
        private int currentItem = 0;
        private int nextindex = 0;
        private Button leftButton;
        private Button rightButton;

        public ChoiceBox(Vector2 position, GraphicsDevice device, ChoiceBoxType type) : base(position, device)
        {
            this.type = type;
            this.Init();
        }

        private void Init()
        {
            OverrideDrawElements = true;
            switch (type)
            {
                case ChoiceBoxType.Item:
                    leftButton = Factory.ArrowButton(Batch, Vector2.Zero, Color.AntiqueWhite, 1, true);
                    rightButton = Factory.ArrowButton(Batch, Vector2.Zero, Color.AntiqueWhite, 1, false);
                    this.Width = 96;
                    this.Height = 32;
                    leftButton.Position = new Vector2(0, Height - 32);
                    rightButton.Position = new Vector2(64, Height-32);
                    break;
                case ChoiceBoxType.CharacterInfo:
                    leftButton = Factory.ArrowButton(Batch, Vector2.Zero, Color.AntiqueWhite, 0.5f, true,"BigArrow");
                    rightButton = Factory.ArrowButton(Batch, Vector2.Zero, Color.AntiqueWhite, 0.5f, false,"BigArrow");
                    leftButton.Scale = new Vector2(0.150f,1.5f);
                    rightButton.Scale = new Vector2(0.15f, 1.5f);
                    this.Width = 295;
                    this.Height = 200;
                    leftButton.Position = new Vector2(0, Height/2f - leftButton.Size.Y/2f);
                    rightButton.Position = new Vector2(Width - rightButton.Size.X, Height / 2f - leftButton.Size.Y / 2f);
                    break;
                    
            }
            leftButton.ClickType = ClickType.Single;
            leftButton.IsVisible = false;
            rightButton.ClickType = ClickType.Single;
            leftButton.ClickCallback = () => CurrentItem--;
            rightButton.ClickCallback = () => CurrentItem++;
            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }

        public Action LeftButtonCallback
        {
            get { return this.leftButton.ClickCallback; }
            set { this.leftButton.ClickCallback = value; }
        }

        public Action RightButtonCallback
        {
            get { return this.rightButton.ClickCallback; }
            set { this.rightButton.ClickCallback = value; }
        }

        public Action OnIndexChange { get; set; }

        public int CurrentItem
        {
            get { return this.currentItem; }
            set
            {
                if (value < 0 || value >= this.count)
                {
                    Debug.Message("Choicebox index out of range");
                    return;
                }
                if (value != nextindex)
                {
                    nextindex = value;
                    changing = true;
                    return;
                }
                this.currentItem = value;
                if (OnIndexChange != null)
                {
                    OnIndexChange.Invoke();
                }
                rightButton.IsVisible = currentItem < count-1;
                leftButton.IsVisible = currentItem > 0;
            }
        }

        public void AddItem(ItemTile item, Action callback = null)
        {
            if (textboxes != null)
            {
                Debug.Error("You can not add ItemTile to a choicebox containing ItemTile/s already");
                return;
            }
            if (items == null)
            {
                items = new List<ItemTile>();
                callbacks = new List<Action>();
            }
            item.Position = new Vector2(32, (Height - 32) + 32 * currentItem);
            item.Transitioning = true;
            item.Clickable = false;
            items.Add(item);
            count++;
            callbacks.Add(callback);
        }

        public void AddItem(TextBox item, Action callback = null)
        {
            if (items != null)
            {
                Debug.Error("You can not add textboxes to a choicebox containing ItemTile/s already");
                return;
            }
            if (textboxes == null)
            {
                textboxes = new List<TextBox>();
                callbacks = new List<Action>();
            }
            item.Position = new Vector2(leftButton.Size.X, this.Height/2f- item.Size.Y/2);
            item.Transitioning = true;
            textboxes.Add(item);
            count++;
            callbacks.Add(callback);

        }

        public void RemoveItem(int index)
        {
            try
            {
                items.RemoveAt(index);
                callbacks.RemoveAt(index);
            }
            catch (Exception)
            {
                Debug.Error("ChoiceBox invalid item removeAt index");
            }
        }

        private double switchElapsed = 0;
        private double switchTimeToUpdate = TimeSpan.FromSeconds(0.1f).TotalSeconds;

        private void SwitchItem(GameTime gameTime)
        {
            switchElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (switchElapsed < switchTimeToUpdate)
            {
                return;
            }
            switchElapsed -= switchTimeToUpdate;
            if (items != null)
            {
                items[currentItem].IsVisible = false;
                if (!items[currentItem].IsVisible)
                {
                    CurrentItem = nextindex;
                    items[currentItem].IsVisible = true;
                    changing = false;
                }
            }
            else if (textboxes != null)
            {
                textboxes[currentItem].IsVisible = false;
                if (!textboxes[currentItem].IsVisible)
                {
                    CurrentItem = nextindex;
                    textboxes[currentItem].IsVisible = true;
                    changing = false;
                }
            }
            

        }

        public override void DrawElements()
        {
            if (!IsVisible) return;
            if (items != null)
            {
                items[currentItem].DrawElements();

            }
            else if (textboxes != null)
            {
                textboxes[currentItem].DrawElements();
            }
            base.DrawElements();
            leftButton.Draw(Batch);
            rightButton.Draw(Batch);
            if (items != null)
            {
                items[currentItem].DrawWithoutTip(Batch);
            }
            else if (textboxes != null)
            {
                textboxes[currentItem].Draw(Batch);
            }
            Batch.End();
            Device.SetRenderTarget(null);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            base.Draw(spriteBatch);
            if (items != null)
            {
                items[currentItem].Tip.DrawAtPosition(spriteBatch,this.Position);
            }
#if DEBUG
            DebugShapes.DrawRectagnle(spriteBatch,this.Position,this.Size,1,Color.Red);
#endif
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!IsVisible) return;
            Vector2 mouse = InputHandler.MousePos - (this.Position+this.Offset);
            if (items != null)
            {
                if (items[currentItem].Over(mouse) && InputHandler.MouseDownOnce("left") )
                {
                    if (callbacks[currentItem] != null)
                    {
                        callbacks[currentItem].Invoke();
                    }
                }
                items[currentItem].Update(gameTime);

            }
            else if (textboxes != null)
            {
                bool hover = textboxes[currentItem].Hover(mouse);
                if (hover)
                {
                    bool callbackcheck = callbacks[currentItem] != null;
                    if (InputHandler.MouseDownOnce("left"))
                    {
                        if (callbackcheck)
                        {
                            callbacks[currentItem].Invoke();
                        }
                    }
                    textboxes[currentItem].BoxTint = ColorHelper.SmoothTransition(textboxes[currentItem].BoxTint, Color.Bisque, 2);

                }
                else
                {
                    textboxes[currentItem].BoxTint = ColorHelper.SmoothTransition(textboxes[currentItem].BoxTint, Color.White, 2);
                }
                textboxes[currentItem].Update(gameTime);

            }
            if (changing)
            {
                SwitchItem(gameTime);
            }
            leftButton.Over(mouse);
            rightButton.Over(mouse);
            leftButton.Update(gameTime);
            rightButton.Update(gameTime);
        }
    }
}
