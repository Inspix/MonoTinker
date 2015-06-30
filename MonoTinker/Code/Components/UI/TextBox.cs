namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;
    using Extensions;

    using Elements;
    using Managers;
    using Utils;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public enum WriteEffect
    {
        LineByLine,
        CharacterByCharacter,
        WholeAtOnce
    }

    public class TextBox : InterfaceElement
    {
        private WriteEffect Effect;
        private string[] Current;
        private int passed;
        private double timeToUpdate;
        private double timeElapsed;
        private bool effectDone;
        private int cPerLine;
        private int maxLines;
        private Vector2 charSize;
        private SpriteFont font;
        private int currentIndex;
        private Button nextPage;

        public TextBox(Vector2 position, GraphicsDevice device, string message = null, Vector2 size = default(Vector2))
            : base(position, device)
        {
            this.Init(size.ToPoint(),message);
        }

        public TextBox(Vector2 position, GraphicsDevice device, string message = null, Vector2 size = default(Vector2), WriteEffect effect = 0)
            : base(position,device)
        {
            this.Effect = effect;
            this.Init(size.ToPoint(), message);

        }

        public Vector2 Position
        {
            get
            {
                return this.Transform.Position;
            }

            set
            {
                this.Transform.Position = value;
            }
        }

        /// <summary>
        /// Effect speed in seconds
        /// </summary>
        public double EffectSpeed
        {
            get { return this.timeToUpdate; }
            set { this.timeToUpdate = TimeSpan.FromSeconds(value).TotalSeconds; }
        }

        protected void Init(Point size, string mesage)
        {
            this.Transitioning = true;
            this.OverrideDrawElements = true;
            this.OverrideDrawLabels = true;
            this.Top(size.X);
            for (int i = 0; i < size.Y; i++)
            {
                this.Middle(size.X, i);
            }
            this.Bottom(size.X);

            this.font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            this.charSize = this.font.MeasureString("A");


            this.maxLines = (int)(this.Height / this.charSize.Y)-1;
            this.cPerLine = (int)(this.Width / this.charSize.X);
            this.Current = mesage.SplitToPieces(this.cPerLine).ToArray();

            if (this.maxLines < this.Current.Length)
            {
                this.nextPage = new Button(new Vector2(this.Width -20, this.Height -20),
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.DarkBall).DirectClone(),
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.DarkBallHover).DirectClone(),
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.DarkBallClick).DirectClone());
                this.nextPage.ClickType = ClickType.Single;
            }

            this.GenerateLines();


            this.RenderTarget2D = new RenderTarget2D(this.Device,this.Width,this.Height);
        }

        private void GenerateLines()
        {
            this.index = 0;
            this.passed = 0;
            this.counter = 1;
            this.effectDone = false;
            switch (this.Effect)
            {
                case WriteEffect.LineByLine:
                    //this.LineByLineEffect();
                    if (timeToUpdate.Equals(default(double)))
                    {
                        this.EffectSpeed = 1;
                    }
                    TextHelper.LineByLineEffect(ref font, ref this.Labels,ref this.currentIndex,maxLines,charSize,ref Current);
                    break;
                case WriteEffect.WholeAtOnce:
                    if (timeToUpdate.Equals(default(double)))
                    {
                        this.EffectSpeed = 0.1;
                    }
                    TextHelper.WholeAtOnceEffect(ref font,ref Labels,ref currentIndex,maxLines,ref Current);
                    //this.WholeAtOnceEffect();
                    break;
                case WriteEffect.CharacterByCharacter:
                    if (timeToUpdate.Equals(default(double)))
                    {
                        this.EffectSpeed = 0.01d;
                    }
                    TextHelper.CharacterByCharacterEffect(ref font, ref this.Labels,ref this.currentIndex,maxLines,charSize,ref Current);
                    //this.CharacterByCharacterEffect();
                    break;
            }
        }


        private int counter= 1;
        private int index;
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
                    if (this.currentIndex >= this.Current.Length)
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
                        if (!effectDone && Effect == WriteEffect.CharacterByCharacter)
                        {
                            this.mod += Labels.Count;
                        }
                        this.GenerateLines();
                        generating = true;
                    }
                    
                }
                this.nextPage.Update();
                if (generating)
                {
                    return;
                }
            }

            if (!this.effectDone && this.Effect != WriteEffect.CharacterByCharacter)
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
                    this.Labels[this.index].Contents = this.Current[this.index + mod].Substring(0, this.counter++);
                    this.passed = index + 1;
                    if (this.counter > this.Current[this.index + mod].Length)
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
