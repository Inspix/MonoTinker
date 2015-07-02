using MonoTinker.Code.Components.Extensions;

namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using Elements;
    using Elements.DebugGraphics;
    using Managers;
    using Utils;

    public class MenuBox : InterfaceElement
    {
        private Action[] optionActions;
        private Action[] callbacks;
        private Action<int> onIndexChange;

        private SpriteFont font;
        private string[] options;
        private Vector2 charSize;
        private Vector2 itemsOffset;
        private double timeToUpdate;
        private double timeElapsed;
        private bool effectDone;
        private int passed;
        private Effect grayScale;
        private int selectedIndex;
        private Vector2 textScale;

        public MenuBox(Vector2 position, GraphicsDevice device, string[] options, Vector2 size,Vector2 itemsOffset = default(Vector2)) : base(position, device)
        {
            this.options = options;
            this.itemsOffset = itemsOffset;
            this.Init(size.ToPoint());
        }

        private void Init(Point size)
        {
            this.grayScale = AssetManager.Instance.Get<Effect>("Grayscale").Clone();
            this.font = AssetManager.Instance.Get<SpriteFont>("Standart");
            this.timeToUpdate = TimeSpan.FromSeconds(0.5).TotalSeconds;
            this.charSize = font.MeasureString("A");
            this.FadeSpeed = 1;
            this.Transitioning = true;
            TextBoxFactory.GenerateBox(size,ref this.Elements,ref this.Width,ref this.Height);
            textScale = new Vector2(
                Width/(charSize.X * (options.Max(s => s.Length)+2)),
                1 );
            int currentIndex = 0;
            TextHelper.LineByLineEffect(ref font, ref this.Labels, ref currentIndex, options.Length, charSize * textScale + Vector2.UnitY*25, ref options,itemsOffset);
            foreach (var label in Labels)
            {
                label.Transform.Scale = textScale;
                label.FadeSpeed = 2;
                label.Clr = Color.Wheat;
                label.Transform.Position = new Vector2((this.Width/2f) - (label.Size.X/2)*textScale.X, label.Transform.PosY);
            }
            


            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }

        public Text Label(int index)
        {
            Labels[index].OnLabelChange = OnLabelChange;
            return this.Labels[index];
        }

        private void OnLabelChange(Text txt)
        {
            txt.Position = new Vector2((this.Width/2f) - (txt.Size.X/2)*textScale.X, txt.Transform.PosY);
        }

        public Transform MenuTransform
        {
            get { return base.Transform; }
        }

        public Action<int> OnIndexChange
        {
            private get
            {
                return onIndexChange;
            }
            set { this.onIndexChange = value; }
        }

        public Action this[int x, bool callback = false]
        {
            get
            {
                if (!callback)
                {
                    return this.optionActions[x];
                }
                return this.callbacks[x];
            }

            set
            {
                if (optionActions == null && !callback)
                {
                    optionActions = new Action[options.Length];
                }
                if (callbacks == null && callback)
                {
                    callbacks = new Action[options.Length];
                }
                if (callback)
                {
                    this.callbacks[x] = value;
                }
                else
                {
                    this.optionActions[x] = value;
                }
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
                if (onIndexChange != null)
                {
                    onIndexChange.Invoke(value);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < options.Length; i++)
            {
                Labels[i].Transform.Scale = SelectedIndex == i
                    ? new Vector2(this.Labels[i].Transform.Scale.X, (MathHelper.SmoothStep(Labels[i].Transform.Scale.Y, textScale.Y, 0.5f)))
                    : new Vector2(this.Labels[i].Transform.Scale.X, (MathHelper.SmoothStep(Labels[i].Transform.Scale.Y, textScale.Y -0.1f, 0.5f)));
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
            DebugShapes.DrawRectagnle(Batch,Vector2.One, new Vector2(Width,Height), 1f,Color.Red);
            Batch.End();
            Device.SetRenderTarget(null);
        }
    }
}
