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
        Big,
        Small
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

        private void Init(Point size, string mesage)
        {
            this.Effect = WriteEffect.WholeAtOnce;
            this.Transitioning = true;
            this.timeToUpdate = TimeSpan.FromSeconds(1).TotalSeconds;
            this.OverrideDrawElements = true;
            this.OverrideDrawLabels = true;
            Top(size.X);
            for (int i = 0; i < size.Y; i++)
            {
                Middle(size.X, i);
            }
            Bottom(size.X);

            font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            charSize = font.MeasureString("A");


            this.maxLines = (int)(this.Height / charSize.Y)-1;
            this.cPerLine = (int)(this.Width / charSize.X);
            this.Current = mesage.SplitToPieces(this.cPerLine).ToArray();

            if (maxLines < Current.Length)
            {
                this.nextPage = new Button(new Vector2(Width-20,Height-20),
                    AssetManager.Instance.Get<Sprite>(SpriteNames.DarkBall).Clone() as Sprite,
                    AssetManager.Instance.Get<Sprite>(SpriteNames.DarkBallHover).Clone() as Sprite,
                    AssetManager.Instance.Get<Sprite>(SpriteNames.DarkBallClick).Clone() as Sprite);
                this.nextPage.Type = ClickType.Single;
            }

            Generate(font);


            this.RenderTarget2D = new RenderTarget2D(this.Device,this.Width,this.Height);
        }


        private void Generate(SpriteFont font)
        {
            passed = 0;
            effectDone = false;
            switch (Effect)
            {
                case WriteEffect.LineByLine:
                    this.LineByLineEffect(font);
                    break;
                case WriteEffect.WholeAtOnce:
                    this.WholeAtOnceEffect(font);
                    break;
                case WriteEffect.CharacterByCharacter:
                    this.CharacterByCharacterEffect(font);
                    break;
            }
        }

        private void CharacterByCharacterEffect(SpriteFont font)
        {
            this.Labels.Clear();
            timeToUpdate = TimeSpan.FromSeconds(0.01).TotalSeconds;
            if (currentIndex + maxLines > Current.Length)
            {
                maxLines = Current.Length - currentIndex;
            }
            int row = 0;
            for (int i = currentIndex; i < currentIndex + maxLines; i++)
            {
                Text x = new Text(font, (Vector2.One * 10) + Vector2.UnitY * charSize.Y * row, Current[i]);
                x.IsVisible = true;
                Labels.Add(x);
                row++;

            }
            this.currentIndex += maxLines;
        }

        private void LineByLineEffect(SpriteFont font)
        {
            this.Labels.Clear();
            if (currentIndex > Current.Length)
            {
                maxLines = Current.Length - currentIndex;
            }
            int row = 0;
            for (int i = currentIndex; i < currentIndex + maxLines; i++)
            {
                Text x = new Text(font, (Vector2.One * 10) + Vector2.UnitY * charSize.Y * row, Current[i]);
                x.Transitioning = true;
                x.IsVisible = false;
                Labels.Add(x);
                row++;

            }
            this.currentIndex += Labels.Count;
        }

        private void WholeAtOnceEffect(SpriteFont font)
        {
            this.Labels.Clear();
            string output = String.Join("\n",Current.Skip(currentIndex).Take(currentIndex > Current.Length ? Current.Length - currentIndex : maxLines));
            Text x = new Text(font,Vector2.One*10,output,0);
            x.IsVisible = false;
            x.Transitioning = true;
            x.FadeSpeed = 1;
            Labels.Add(x);
            currentIndex += maxLines;
        }

        private int counter= 1;

        private int index;
        public override void Update(GameTime gameTime)
        {
            Vector2 mousePos = InputHandler.MousePos() - this.Transform.Position;

            if (nextPage != null)
            {
                nextPage.Over(mousePos);
                if (nextPage.Clicked)
                {
                    if (currentIndex >= Current.Length)
                    {
                        this.IsVisible = false;
                    }
                    else
                    {
                        Generate(font);
                    }
                    
                }
                nextPage.Update();
            }

            if (!effectDone && Effect != WriteEffect.CharacterByCharacter)
            {
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed >= timeToUpdate)
                {
                    timeElapsed -= timeToUpdate;
                    Labels[passed++].IsVisible = true;
                    if (passed>= Labels.Count)
                    {
                        effectDone = true;
                    }
                } 
            }
            else if (!effectDone)
            {
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed >= timeToUpdate)
                {
                    timeElapsed -= timeToUpdate;
                    Labels[index].Contents = Current[index].Substring(0, counter++);
                    passed = index + 1;
                    if (counter > Current[index].Length)
                    {
                        
                        counter = 0;
                        index++;
                        if (index >= Labels.Count)
                        {
                            index = 0;
                            effectDone = true;
                        }
                    }
                }
            }

           
            base.Update(gameTime);
        }

        public override void DrawElements()
        {
            
            base.DrawElements();
            for (int i = 0; i < passed; i++)
            {
                if (Labels.Count > 0 && Labels[i].IsVisible)
                {
                    Labels[i].Draw(Batch);
                }
            }
            if (nextPage.IsVisible && nextPage != null)
            {
                nextPage.Draw(Batch);
            }
            Batch.End();
            Device.SetRenderTarget(null);
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
