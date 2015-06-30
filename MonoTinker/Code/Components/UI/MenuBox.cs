

namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Elements;
    using Managers;
    using Utils;

    public class MenuBox : InterfaceElement
    {
        private Action[] optionActions;
        private SpriteFont font;
        private string[] options;
        private Vector2 charSize;
        private double timeToUpdate;
        private double timeElapsed;
        private bool effectDone;
        private int passed;
        private Effect grayScale;
        private int selectedIndex;
        private Vector2 textScale;

        public MenuBox(Vector2 position, GraphicsDevice device, string[] options, Vector2 size) : base(position, device)
        {
            this.options = options;
            this.Init(size.ToPoint());
        }

        private void Init(Point size)
        {
            this.grayScale = AssetManager.Instance.Get<Effect>("Grayscale").Clone();
            this.font = AssetManager.Instance.Get<SpriteFont>("SplashScreenFont");
            this.timeToUpdate = TimeSpan.FromSeconds(1.2).TotalSeconds;
            this.charSize = font.MeasureString("A");
            this.FadeSpeed = 1;
            this.Transitioning = true;
            Top(size.X);
            for (int i = 0; i < size.Y; i++)
            {
                Middle(size.X,i);
            }
            Bottom(size.X);

            textScale = new Vector2(
                Width/(charSize.X * (options.Max(s => s.Length)+2)),
                Height/(charSize.Y * (options.Length+5) ));
            int currentIndex = 0;
            TextHelper.LineByLineEffect(ref font, ref this.Labels, ref currentIndex, options.Length, charSize * textScale, ref options);
            foreach (var label in Labels)
            {
                label.Transform.Scale = textScale;
                label.Clr = Color.Wheat;
                label.Transform.Position = new Vector2((this.Width/2f) - (label.Size.X/2)*textScale.X, label.Transform.PosY);
            }
            
            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }

        public Transform MenuTransform
        {
            get { return base.Transform; }
        }

        public Action this[int x]
        {
            get
            {
                return this.optionActions[x];
            }
            set
            {
                if (optionActions == null)
                {
                    optionActions = new Action[options.Length];
                }
                this.optionActions[x] = value;
            }
        }

        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set {
                if (value < 0)
                {
                    return;
                }
                if (value >= this.options.Length)
                {
                    return;
                }
                this.selectedIndex = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < options.Length; i++)
            {
                Labels[i].Transform.Scale = SelectedIndex == i
                    ? new Vector2(this.Labels[i].Transform.Scale.X, (MathHelper.SmoothStep(Labels[i].Transform.Scale.Y, textScale.Y, 0.1f)))
                    : new Vector2(this.Labels[i].Transform.Scale.X, (MathHelper.SmoothStep(Labels[i].Transform.Scale.Y, textScale.Y -0.2f, 0.1f)));
                Labels[i].Clr = SelectedIndex == i
                    ? ColorHelper.SmoothTransition(Labels[i].Clr, Color.OrangeRed, 0.02f)
                    : ColorHelper.SmoothTransition(Labels[i].Clr, Color.Wheat, 0.02f);
            }
            if (Keys.Enter.DownOnce())
            {
                if (selectedIndex < options.Length && selectedIndex >= 0)
                {
                    if (optionActions[selectedIndex] != null)
                    {
                        optionActions[selectedIndex].Invoke();
                    }
                }
            }
            if (this.effectDone) return;
            this.timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.timeElapsed >= this.timeToUpdate)
            {
                this.timeElapsed -= this.timeToUpdate;
                this.Labels[this.passed++].IsVisible = true;
                if (this.passed >= this.Labels.Count)
                {
                    this.effectDone = true;
                }
            }
            
        }

        public override void DrawElements()
        {
            Device.SetRenderTarget(this.RenderTarget2D);
            Device.Clear(Color.FromNonPremultiplied(0,0,0,0));
            Batch.Begin(SpriteSortMode.Immediate);
            grayScale.CurrentTechnique.Passes[0].Apply();
            foreach (var sprite in Elements)
            {
                sprite.Value.Draw(Batch);
            }
            Batch.End();
            Batch.Begin();
            foreach (var option in Labels)
            {
                option.Draw(Batch);
            }
            Batch.End();
            Device.SetRenderTarget(null);
        }

        #region Generation
        private void Top(int size)
        {
            Sprite topLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopLeft).DirectClone();
            topLeft.Position = Vector2.Zero;
            Width += topLeft.SourceWidth;
            Height += topLeft.SourceHeight;
            this.Elements.Add("topLeft", topLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite topMiddle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopMiddle).DirectClone();
                topMiddle.Position = Vector2.UnitX * this.Width;
                Width += topMiddle.SourceWidth;
                this.Elements.Add("topMiddle" + i, topMiddle);
            }
            Sprite topRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameTopRight).DirectClone();
            topRight.Position = Vector2.UnitX * Width;
            Width += topRight.SourceWidth;
            this.Elements.Add("topRight", topRight);
        }

        private void Middle(int size, int mod)
        {
            Sprite middleLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddleLeft).DirectClone();
            middleLeft.Position = Vector2.UnitY * Height;
            Height += middleLeft.SourceHeight;
            this.Elements.Add("middleLeft" + mod, middleLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite middle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddle).DirectClone();
                middle.Position = middleLeft.Position + (Vector2.UnitX * middleLeft.SourceWidth * i);
                this.Elements.Add("middle" + i + "_" + mod, middle);
            }
            Sprite middleRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameMiddleRight).DirectClone();
            middleRight.Position = middleLeft.Position + (Vector2.UnitX * (Width - middleRight.SourceWidth));
            this.Elements.Add("middleRight" + mod, middleRight);
        }

        private void Bottom(int size)
        {
            Sprite bottomLeft = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomLeft).DirectClone();
            bottomLeft.Position = Vector2.UnitY * Height;
            Height += bottomLeft.SourceHeight;
            this.Elements.Add("bottomLeft", bottomLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite bottomMiddle = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomMiddle).DirectClone();
                bottomMiddle.Position = bottomLeft.Position + (Vector2.UnitX * bottomMiddle.SourceWidth * i);
                this.Elements.Add("bottomMiddle" + i, bottomMiddle);
            }
            Sprite bottomRight = AssetManager.Instance.Get<Sprite>(Sn.Menu.FrameBottomRight).DirectClone();
            bottomRight.Position = bottomLeft.Position + (Vector2.UnitX * (Width - bottomRight.SourceWidth));
            this.Elements.Add("bottomRight", bottomRight);
        }
        #endregion
    }
}
