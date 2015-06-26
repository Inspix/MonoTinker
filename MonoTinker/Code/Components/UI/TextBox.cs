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

        private Vector2 charSize;

        public TextBox(Vector2 position, GraphicsDevice device, string message = null, int size = 1, WriteEffect effect = 0)
           : this(position,device,message,size)
        {
            this.Effect = effect;
        }

        public TextBox(Vector2 position, GraphicsDevice device, string message = null, int size=1)
            : base(position, device)
        {
            this.Init(size,message);
        }

        private void Init(int size, string mesage)
        {
            this.Effect = WriteEffect.CharacterByCharacter;
            this.timeToUpdate = TimeSpan.FromSeconds(1).TotalSeconds;
            this.OverrideDrawElements = true;
            this.OverrideDrawLabels = true;
            Top(size);
            for (int i = 0; i < size; i++)
            {
                Middle(size, i);
            }
            Bottom(size);

            SpriteFont font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            charSize = font.MeasureString("A");


            this.cPerLine = (int)(this.Width / charSize.X);
            this.Current = mesage.SplitToPieces(this.cPerLine).ToArray();

            Generate(font);


            this.RenderTarget2D = new RenderTarget2D(this.Device,this.Width,this.Height);
        }


        private void Generate(SpriteFont font)
        {
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
            timeToUpdate = TimeSpan.FromSeconds(0.05).TotalSeconds;
            for (int i = 0; i < Current.Length; i++)
            {
                Text x = new Text(font, (Vector2.One * 10) + Vector2.UnitY * charSize.Y * i, Current[i]);
                x.IsVisible = true;
                Labels.Add(x);

            }
        }

        private void LineByLineEffect(SpriteFont font)
        {
            for (int i = 0; i < Current.Length; i++)
            {
                Text x = new Text(font, (Vector2.One * 10) + Vector2.UnitY * charSize.Y * i, Current[i]);
                x.Transitioning = true;
                x.IsVisible = false;
                Labels.Add(x);

            }
        }

        private void WholeAtOnceEffect(SpriteFont font)
        {
            string output = String.Join("\n",Current);
            Text x = new Text(font,Vector2.One*10,output,0);
            x.IsVisible = false;
            x.Transitioning = true;
            x.FadeSpeed = 1;
            Labels.Add(x);
        }

        private int counter= 1;

        private int index;
        public override void Update(GameTime gameTime)
        {
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
                if (Labels[i].IsVisible)
                {
                    Labels[i].Draw(Batch);
                }
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
