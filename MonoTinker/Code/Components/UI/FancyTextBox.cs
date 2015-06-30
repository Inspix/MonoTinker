namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Elements;
    using Extensions;
    using Managers;
    using Utils;

    public class FancyTextBox : InterfaceElement
    {
        private WriteEffect effect;
        private bool effectDone;
        private int passed;
        private int index;
        private int maxLines = 3;
        private Button nextPage;
        private int cPerLine;
        private int currentIndex;
        private string[] text;
        private double timeToUpdate;
        private double timeElapsed;
        private SpriteFont font;

        private Vector2 charSize;


        public FancyTextBox(Vector2 position, GraphicsDevice device, string message, int width, WriteEffect effect = WriteEffect.LineByLine) : base(position, device)
        {
            this.effect = effect;
            this.Init(message,width);
        }

        private void Init(string message, int width)
        {
            if (width > 28)
            {
                width = 28;
            }
            this.OverrideDrawElements = true;
            this.OverrideDrawLabels = true;
            this.Transitioning = true;
            this.Alpha = 0;
            this.IsVisible = true;
            this.font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            Sprite leftPart = AssetManager.Instance.Get<Sprite>(Sn.Menu.BigFrameLeft).DirectClone();
            Sprite middlePart = AssetManager.Instance.Get<Sprite>(Sn.Menu.BigFrameMiddle).DirectClone();
            Sprite buttonClick = AssetManager.Instance.Get<Sprite>(Sn.Menu.BigFrameRightButtonClick).DirectClone();
            this.charSize = font.MeasureString("a");
            this.Width = leftPart.SourceWidth + (middlePart.SourceWidth * width);
            this.Height = middlePart.SourceHeight;
            this.cPerLine = (int)(this.Width / this.charSize.X);
            this.Width += buttonClick.SourceWidth;
            this.text = message.SplitToPieces(this.cPerLine).ToArray();
            leftPart.Position = Vector2.Zero;
            for (int i = 1; i <= width; i++)
            {
                middlePart.Position = leftPart.Position + Vector2.UnitX*leftPart.DefaultSource.Size.X * i;
                Elements.Add("middle" + i, middlePart.DirectClone());
            }

            this.nextPage = new Button(middlePart.Position + Vector2.UnitX* middlePart.SourceWidth,
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.BigFrameRightButton).DirectClone(),
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.BigFrameRightButtonHover).DirectClone(),
                    buttonClick);
                this.nextPage.ClickType = ClickType.Single;
            

            Generate();

            Elements.Add("left", leftPart);

            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }

        public double EffectSpeed
        {
            get { return this.timeToUpdate; }
            set { this.timeToUpdate = TimeSpan.FromSeconds(value).TotalSeconds; }
        }

        private int counter = 1;
        private void Generate()
        {
            this.index = 0;
            this.passed = 0;
            this.counter = 0;
            this.effectDone = false;
            switch (this.effect)
            {
                case WriteEffect.LineByLine:
                    if (timeToUpdate.Equals(default(double)))
                    {
                        this.EffectSpeed = 1;
                    }
                    TextHelper.LineByLineEffect(ref font, ref this.Labels, ref this.currentIndex, maxLines, charSize, ref text, new Vector2(18, 1));
                    break;
                case WriteEffect.WholeAtOnce:
                    if (timeToUpdate.Equals(default(double)))
                    {
                        this.EffectSpeed = 0.1;
                    }
                    TextHelper.WholeAtOnceEffect(ref font, ref Labels, ref currentIndex, maxLines, ref text, new Vector2(18,1));
                    break;
                case WriteEffect.CharacterByCharacter:
                    if (timeToUpdate.Equals(default(double)))
                    {
                        this.EffectSpeed = 0.01d;
                    }
                    TextHelper.CharacterByCharacterEffect(ref font, ref this.Labels, ref this.currentIndex, maxLines, charSize, ref text, new Vector2(18, 1));
                    break;
            }


        }
        private int mod;

        public override void Update(GameTime gameTime)
        {
            Vector2 mousePos = InputHandler.MousePos() - this.Transform.Position;

            if (this.nextPage != null)
            {
                bool generating = false;
                this.nextPage.Over(mousePos);
                if (this.nextPage.Clicked)
                {
                    if (this.currentIndex >= this.text.Length)
                    {
                        this.IsVisible = false;
                    }
                    else
                    {
                        foreach (var label in this.Labels)
                        {
                            label.Transitioning = false;
                            label.IsVisible = false;
                        }
                        if (!effectDone && effect == WriteEffect.CharacterByCharacter)
                        {
                            this.mod += Labels.Count;
                        }
                        this.Generate();
                        generating = true;
                    }

                }
                this.nextPage.Update();
                if (generating)
                {
                    return;
                }
            }

            if (!this.effectDone && this.effect != WriteEffect.CharacterByCharacter)
            {
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
            else if (!this.effectDone)
            {
                this.timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.timeElapsed >= this.timeToUpdate)
                {
                    this.timeElapsed -= this.timeToUpdate;
                    this.Labels[this.index].Append(this.text[index + mod][counter++]);
                    this.passed = index + 1;
                    if (this.counter >= this.text[this.index + mod].Length)
                    {

                        this.counter = 0;
                        this.index++;
                        if (this.index >= this.Labels.Count)
                        {
                            this.mod += index;
                            this.index = 0;
                            this.effectDone = true;
                        }
                    }
                }
            }


            base.Update(gameTime);
        }

        public override void DrawElements()
        {

            base.DrawElements();
            for (int i = 0; i < this.passed; i++)
            {
                if (this.Labels.Count > 0 && this.Labels[i].IsVisible)
                {
                    this.Labels[i].Draw(this.Batch);
                }
            }
            if (this.nextPage != null && this.nextPage.IsVisible)
            {
                this.nextPage.Draw(this.Batch);
            }
            this.Batch.End();
            this.Device.SetRenderTarget(null);
        }
    }
}
