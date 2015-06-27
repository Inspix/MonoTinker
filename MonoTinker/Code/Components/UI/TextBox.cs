namespace MonoTinker.Code.Components.UI
{
    using System;
    using System.Linq;
    using Extensions;
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

    public enum BoxType
    {
        Small,
        Big,
        Tooltip
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
            : this(position,device,message,size)
        {
            this.Effect = effect;
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

        private void Init(Point size, string mesage)
        {
            this.Transitioning = true;
            this.timeToUpdate = TimeSpan.FromSeconds(1).TotalSeconds;
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
                    AssetManager.Instance.Get<Sprite>(SpriteNames.DarkBall).DirectClone(),
                    AssetManager.Instance.Get<Sprite>(SpriteNames.DarkBallHover).DirectClone(),
                    AssetManager.Instance.Get<Sprite>(SpriteNames.DarkBallClick).DirectClone());
                this.nextPage.ClickType = ClickType.Single;
            }

            this.Generate();


            this.RenderTarget2D = new RenderTarget2D(this.Device,this.Width,this.Height);
        }

        private void Generate()
        {
            this.passed = 0;
            this.effectDone = false;
            switch (this.Effect)
            {
                case WriteEffect.LineByLine:
                    this.LineByLineEffect();
                    break;
                case WriteEffect.WholeAtOnce:
                    this.WholeAtOnceEffect();
                    break;
                case WriteEffect.CharacterByCharacter:
                    this.CharacterByCharacterEffect();
                    break;
            }
        }

        private void CharacterByCharacterEffect()
        {
            this.Labels.Clear();
            this.timeToUpdate = TimeSpan.FromSeconds(0.01).TotalSeconds;
            if (this.currentIndex + this.maxLines > this.Current.Length)
            {
                this.maxLines = this.Current.Length - this.currentIndex;
            }
            int row = 0;
            for (int i = this.currentIndex; i < this.currentIndex + this.maxLines; i++)
            {
                Text x = new Text(this.font, (Vector2.One * 10) + Vector2.UnitY * this.charSize.Y * row, this.Current[i]);
                x.IsVisible = true;
                this.Labels.Add(x);
                row++;

            }
            this.currentIndex += this.maxLines;
        }

        private void LineByLineEffect()
        {
            this.Labels.Clear();
            if (this.currentIndex + this.maxLines > this.Current.Length)
            {
                this.maxLines = this.Current.Length - this.currentIndex;
            }
            int row = 0;
            for (int i = this.currentIndex; i < this.currentIndex + this.maxLines; i++)
            {
                Text x = new Text(this.font, (Vector2.One * 10) + Vector2.UnitY * this.charSize.Y * row, this.Current[i],0,false);
                x.Transitioning = true;
                this.Labels.Add(x);
                row++;

            }
            this.currentIndex += this.maxLines;
        }

        private void WholeAtOnceEffect()
        {
            this.Labels.Clear();
            string output = String.Join("\n",
                this.Current.Skip(this.currentIndex)
                    .Take(this.currentIndex > this.Current.Length
                    ? this.Current.Length - this.currentIndex
                    : this.maxLines));
            Text x = new Text(this.font,Vector2.One*10,output,0);
            x.IsVisible = false;
            x.Transitioning = true;
            x.FadeSpeed = 1;
            this.Labels.Add(x);
            this.currentIndex += this.maxLines;
        }


        private int counter= 1;
        private int index;

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
                    this.Labels[this.index].Contents = this.Current[this.index].Substring(0, this.counter++);
                    this.passed = this.index + 1;
                    if (this.counter > this.Current[this.index].Length)
                    {

                        this.counter = 0;
                        this.index++;
                        if (this.index >= this.Labels.Count)
                        {
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
            Sprite topLeft = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameTopLeft).Clone() as Sprite;
            topLeft.Position = Vector2.Zero;
            Width += topLeft.SourceWidth;
            Height += topLeft.SourceHeight;
            this.Elements.Add("topLeft", topLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite topMiddle = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameTopMiddle).Clone() as Sprite;
                topMiddle.Position = Vector2.UnitX * this.Width;
                Width += topMiddle.SourceWidth;
                this.Elements.Add("topMiddle" + i, topMiddle);
            }
            Sprite topRight = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameTopRight).Clone() as Sprite;
            topRight.Position = Vector2.UnitX * Width;
            Width += topRight.SourceWidth;
            this.Elements.Add("topRight", topRight);
        }

        private void Middle(int size, int mod)
        {
            Sprite middleLeft = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddleLeft).Clone() as Sprite;
            middleLeft.Position = Vector2.UnitY * Height;
            Height += middleLeft.SourceHeight;
            this.Elements.Add("middleLeft" + mod, middleLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite middle = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddle).Clone() as Sprite;
                middle.Position = middleLeft.Position + (Vector2.UnitX * middleLeft.SourceWidth * i);
                this.Elements.Add("middle" + i + "_" + mod, middle);
            }
            Sprite middleRight = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameMiddleRight).Clone() as Sprite;
            middleRight.Position = middleLeft.Position + (Vector2.UnitX * (Width - middleRight.SourceWidth));
            this.Elements.Add("middleRight" + mod, middleRight);
        }

        private void Bottom(int size)
        {
            Sprite bottomLeft = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameBottomLeft).Clone() as Sprite;
            bottomLeft.Position = Vector2.UnitY * Height;
            Height += bottomLeft.SourceHeight;
            this.Elements.Add("bottomLeft", bottomLeft);
            for (int i = 1; i <= size; i++)
            {
                Sprite bottomMiddle = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameBottomMiddle).Clone() as Sprite;
                bottomMiddle.Position = bottomLeft.Position + (Vector2.UnitX * bottomMiddle.SourceWidth * i);
                this.Elements.Add("bottomMiddle" + i, bottomMiddle);
            }
            Sprite bottomRight = AssetManager.Instance.Get<Sprite>(SpriteNames.FrameBottomRight).Clone() as Sprite;
            bottomRight.Position = bottomLeft.Position + (Vector2.UnitX * (Width - bottomRight.SourceWidth));
            this.Elements.Add("bottomRight", bottomRight);
        } 
        #endregion
    }
}
