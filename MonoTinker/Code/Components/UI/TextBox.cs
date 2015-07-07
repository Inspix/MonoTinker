using System.Threading;

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
        private Origin imageAligment;
        private string[] Current;
        private int passed;
        private double timeToUpdate;
        private double timeElapsed;
        private bool effectDone;
        private bool hasImage;
        private int cPerLine;
        private int maxLines;
        private Vector2 charSize;
        private Rectangle bounds;
        private SpriteFont font;
        private int currentIndex;
        private Button nextPageBtn;

        public TextBox(Vector2 position, GraphicsDevice device, string message = null, Vector2 size = default(Vector2))
            : base(position, device)
        {
            this.Init(size,message);
        }

        public TextBox(Vector2 position, GraphicsDevice device, string message = null, Vector2 size = default(Vector2), WriteEffect effect = 0)
            : base(position,device)
        {
            this.Effect = effect;
            this.Init(size, message);

        }

        /// <summary>
        /// Effect speed in seconds
        /// </summary>
        public double EffectSpeed
        {
            get { return this.timeToUpdate; }
            set { this.timeToUpdate = TimeSpan.FromSeconds(value).TotalSeconds; }
        }

        public bool CycleableText { get; set; } = false;

        protected void Init(Vector2 size, string mesage)
        {
            this.Transitioning = true;
            this.OverrideDrawElements = true;
            this.OverrideDrawLabels = true;
            Sprite box =  BoxFactory.BoxSprite(Batch,size);
            this.Elements.Add("box", box);
            this.Width = box.SourceWidth;
            this.Height = box.SourceHeight;
            this.font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            this.charSize = this.font.MeasureString("A");


            this.maxLines = (int)(this.Height / this.charSize.Y)-1;
            this.cPerLine = (int)(this.Width / this.charSize.X);
            this.Current = mesage.SplitToPieces(this.cPerLine).ToArray();

            if (this.maxLines < this.Current.Length)
            {
                this.nextPageBtn = new Button(new Vector2(this.Width -20, this.Height -20),
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.DarkBall).DirectClone(),
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.DarkBallHover).DirectClone(),
                    AssetManager.Instance.Get<Sprite>(Sn.Menu.DarkBallClick).DirectClone());
                this.nextPageBtn.ClickType = ClickType.Single;
            }

            this.GenerateLines();
            bounds = new Rectangle(this.Position.ToPoint(),this.Size.ToPoint());

            this.RenderTarget2D = new RenderTarget2D(this.Device,this.Width,this.Height);
        }

        public bool Hover(Vector2 position)
        {
            return bounds.Contains(position - this.Position);
        }

        private void GenerateLines()
        {
            this.index = 0;
            this.passed = 0;
            this.counter = 0;
            this.effectDone = false;
            switch (this.Effect)
            {
                case WriteEffect.LineByLine:
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
                    break;
                case WriteEffect.CharacterByCharacter:
                    if (timeToUpdate.Equals(default(double)))
                    {
                        this.EffectSpeed = 0.01d;
                    }
                    TextHelper.CharacterByCharacterEffect(ref font, ref this.Labels,ref this.currentIndex,maxLines,charSize,ref Current);
                    break;
            }
            if (hasImage)
            {
                Realign();
            }
        }

        private void Realign()
        {
            if (imageAligment == Origin.CenterLeft || imageAligment == Origin.TopLeft || imageAligment == Origin.BottomLeft)
            {
                foreach (var label in Labels)
                {
                    label.PosX += Elements["image"].Size.X;
                }
            }
            if (imageAligment == Origin.TopCenter)
            {
                foreach (var label in Labels)
                {
                    label.PosY += Elements["image"].Size.Y;
                }
            }
        }

        public void AddImage(Sprite image, Origin aligment = Origin.CenterRight,Vector2 offset = default(Vector2))
        {
            Sprite copy = image.DirectClone(true);
            Vector2 size = (this.Elements["box"].Size/64) + new Vector2(2,0);
            Vector2 imgSize = image.Size / 64;
            

            if (aligment == Origin.CenterLeft || aligment == Origin.BottomLeft || aligment == Origin.TopLeft ||
                aligment == Origin.CenterRight || aligment == Origin.BottomRight || aligment == Origin.TopRight)
            {
                size.X += imgSize.X;
                Elements["box"] = BoxFactory.BoxSprite(Batch, new Vector2(size.X < imgSize.X ? imgSize.X : size.X, size.Y < imgSize.Y ? imgSize.Y : size.Y));
                this.Width = Elements["box"].SourceWidth;
                this.Height = Elements["box"].SourceHeight;
                
                switch (aligment)
                {
                    case Origin.CenterLeft:
                        copy.Position = new Vector2(0 + 10 + offset.X, (0 + 10 + offset.Y));
                        foreach (var label in Labels)
                        {
                            label.PosX += copy.Size.X;
                        }
                        break;
                    case Origin.BottomLeft:
                        copy.Position = new Vector2(0 - 10 + offset.X, (Height - copy.SourceHeight + offset.Y));
                        foreach (var label in Labels)
                        {
                            label.PosX += copy.Size.X;
                        }
                        break;
                    case Origin.TopLeft:
                        copy.Position = new Vector2(0 + 10 + offset.X, (0 + 10 + offset.Y));
                        foreach (var label in Labels)
                        {
                            label.PosX += copy.Size.X;
                        }
                        break;
                    case Origin.CenterRight:
                        copy.Position = new Vector2(Width - copy.SourceWidth + offset.X, (Height/2 - copy.SourceHeight/2 + offset.Y));
                        break;
                    case Origin.BottomRight:
                        copy.Position = new Vector2(Width - copy.SourceWidth + offset.X, (Height - copy.SourceHeight + offset.Y));
                        break;
                    case Origin.TopRight:
                        copy.Position = new Vector2(Width - copy.SourceWidth + offset.X, (0 + 10 + offset.Y));
                        break;
                }
            }

            switch (aligment)
            {
                case Origin.Center:
                    Elements["box"] = BoxFactory.BoxSprite(Batch, new Vector2(size.X < imgSize.X ? imgSize.X : size.X, size.Y < imgSize.Y ? imgSize.Y : size.Y));
                    this.Width = Elements["box"].SourceWidth;
                    this.Height = Elements["box"].SourceHeight;
                    copy.Position = new Vector2((Width / 2 - copy.SourceWidth / 2) + offset.X, (Height / 2f - copy.SourceHeight / 2f) + offset.Y);
                    break;
                case Origin.TopCenter:
                    size.Y += imgSize.Y+1;
                    Elements["box"] = BoxFactory.BoxSprite(Batch, new Vector2(size.X < imgSize.X ? imgSize.X : size.X, size.Y < imgSize.Y ? imgSize.Y : size.Y));
                    this.Width = Elements["box"].SourceWidth;
                    this.Height = Elements["box"].SourceHeight;
                    copy.Position = new Vector2((Width/2 - copy.SourceWidth/2) + offset.X, 0 + 10 + offset.Y);
                    foreach (var label in Labels)
                    {
                        label.PosY += copy.SourceHeight;
                    }
                    break;
                case Origin.BottomCenter:
                    size.Y += imgSize.Y+1;
                    Elements["box"] = BoxFactory.BoxSprite(Batch, new Vector2(size.X < imgSize.X ? imgSize.X : size.X, size.Y < imgSize.Y ? imgSize.Y : size.Y));
                    this.Width = Elements["box"].SourceWidth;
                    this.Height = Elements["box"].SourceHeight;
                    copy.Position = new Vector2((Width / 2 - copy.SourceWidth / 2) + offset.X, (Height - copy.SourceHeight - 10) + offset.Y);
                    break;
            }

            this.imageAligment = aligment;
            this.hasImage = true;
            if (nextPageBtn != null)
            {
                this.nextPageBtn.Position = new Vector2(Width-20,Height-20);
            }
            bounds = new Rectangle(this.Position.ToPoint(),this.Size.ToPoint());
            this.Elements.Add("image", copy);
            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
            
        }

        
        private int counter= 0;
        private int index;
        private int mod;

        public override void Update(GameTime gameTime)
        {
            //Vector2 mousePos = InputHandler.MousePos() - this.Position;

            if (this.nextPageBtn != null)
            {
                bool generating = false;
                if (this.nextPageBtn.Clicked)
                {
                    if (this.currentIndex >= this.Current.Length)
                    {
                        if (CycleableText)
                        {
                            this.currentIndex = 0;
                            this.mod = 0;
                            this.GenerateLines();
                            generating = true;
                        }
                        else
                        {
                            this.IsVisible = false;
                        }
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
                this.nextPageBtn.Update(gameTime);
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
                    this.Labels[this.index].Append(this.Current[this.index + mod][counter]);
                    this.counter++;
                    this.passed = index + 1;
                    if (this.counter >= this.Current[this.index + mod].Length)
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
            if (this.nextPageBtn != null && this.nextPageBtn.IsVisible)
            {
                this.nextPageBtn.Draw(this.Batch);
            }
            this.Batch.End();
            this.Device.SetRenderTarget(null);
        }
    }
}
