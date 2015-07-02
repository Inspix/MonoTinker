using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.UI
{
    public enum SliderValueDirection
    {
        TopToBottom,
        BottomToTop
    }

    public class Slider : InterfaceElement
    {
        private Vector2 topLimit;
        private Vector2 bottomLimit;
        private Button handle;
        private Action<float> onValueChangeCallback;

        public Slider(Vector2 position, GraphicsDevice device, int lenght) : base(position, device)
        {
            this.Init(lenght);
        }

        private void Init(int lenght)
        {
            this.OverrideDrawElements = true;
            Sprite ballbottom = AssetManager.Instance.Get<Sprite>(Sn.Menu.DarkBall).DirectClone();
            Sprite ballTop = AssetManager.Instance.Get<Sprite>(Sn.Menu.GreenBall).DirectClone();
            Sprite toppart = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderTop).DirectClone();
            Sprite middlepart = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderMiddle).DirectClone();
            Sprite bottompart = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderBottom).DirectClone();
            Sprite hand = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderHandle).DirectClone();
            Sprite handHover = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderHandle).DirectClone();
            Sprite handClicked = AssetManager.Instance.Get<Sprite>(Sn.Menu.SliderHandle).DirectClone();

            hand.Clr = Color.White;
            handHover.Clr = Color.BurlyWood;
            handClicked.Clr = Color.Green;

            toppart.Position = Vector2.Zero;
            ballTop.Position = toppart.Position + Vector2.One*5;
            this.Width += toppart.SourceWidth;
            this.Height += toppart.SourceHeight;
            Elements.Add("top", toppart);
            middlepart.Position = (toppart.Position + toppart.SourceHeight*Vector2.UnitY) + Vector2.UnitX*7;
            handle = new Button(middlepart.Position - Vector2.UnitX, hand, handHover, handClicked);
            handle.ClickType = ClickType.Continuous;
            handle.InflateBox = Vector2.UnitY*10;

            topLimit = middlepart.Position - Vector2.UnitX;

            for (int i = 0; i < lenght; i++)
            {
                Elements.Add("mid" + i + 1, middlepart.DirectClone());
                middlepart.Position = (middlepart.Position + middlepart.SourceHeight*Vector2.UnitY);
                this.Height += middlepart.SourceHeight;
            }
            bottomLimit = (middlepart.Position - Vector2.UnitX) - (Vector2.UnitY* middlepart.SourceHeight*3f);
            bottompart.Position = new Vector2(toppart.Position.X, middlepart.Position.Y);
            ballbottom.Position = bottompart.Position + Vector2.One*5;
            this.Height += bottompart.SourceHeight;
            Elements.Add("bot", bottompart);
            Elements.Add("balltop",ballTop);
            Elements.Add("ballbottom",ballbottom);

            this.RenderTarget2D = new RenderTarget2D(Device, Width, Height);
        }

        public SliderValueDirection Direction { get; set; }

        public Action<float> OnValueChangeCallback
        {
            private get { return this.onValueChangeCallback; }
            set { this.onValueChangeCallback = value; }
        }

        public Sprite TopBall
        {
            get { return this.Elements["balltop"]; }
            set
            {
                Vector2 pos = this.Elements["balltop"].Position;
                this.Elements["balltop"] = value;
                this.Elements["balltop"].Position = pos;
            }
        }

        public Sprite BottomBall
        {
            get { return this.Elements["ballbottom"]; }
            set
            {
                Vector2 pos = this.Elements["ballbottom"].Position;
                this.Elements["ballbottom"] = value;
                this.Elements["ballbottom"].Position = pos;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 mousePos = InputHandler.MousePos() - this.Transform.Position;
            handle.Over(mousePos);
            if (handle.Clicked)
            {
                Vector2 positionChange = new Vector2(0, InputHandler.MouseDelta().Y);
                if (positionChange != Vector2.Zero)
                {
                    handle.Position += positionChange;
                    handle.Position = new Vector2(handle.Position.X, MathHelper.Clamp(handle.Position.Y, topLimit.Y, bottomLimit.Y));
                    OnPositionChange();
                }
                
            }
            handle.Update();

        }

        private void OnPositionChange()
        {
            if (onValueChangeCallback != null)
            {
                switch (Direction)
                {
                    case SliderValueDirection.TopToBottom:
                        onValueChangeCallback.Invoke(1.25f - ((handle.Position.Y/bottomLimit.Y)*1f));
                        break;
                    case SliderValueDirection.BottomToTop:
                        onValueChangeCallback.Invoke((handle.Position.Y/bottomLimit.Y)*1f);
                        break;
                }
            }
        }

        public void Reset()
        {
            handle.Position = topLimit;
        }

        public override void DrawElements()
        {
            base.DrawElements();
            this.handle.Draw(Batch);
            Batch.End();
            Device.SetRenderTarget(null);

        }
    }
}
