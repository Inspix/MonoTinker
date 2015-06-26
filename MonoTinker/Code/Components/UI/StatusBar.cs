namespace MonoTinker.Code.Components.UI
{
    using System;
    using Extensions;
    using Utils;
    using Managers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class StatusBar : InterfaceElement
    {
        private const string Hud = "hud";
        private const string HP = "HealthBar";
        private const string MP = "ManaBar";
        private const string SP = "SpiritBar";

        public StatusBar(Vector2 position, GraphicsDevice device)
            : base(position, device)
        {
            this.Init();
        }

        public int FadeSpeed
        {
            get
            {
                return this.fadeSpeed;
            }
            set
            {
                this.fadeSpeed = value;
            }
        }

        private void Init()
        {
            SpriteFont font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            Text hp = new Text(font,new Vector2(85,5),"100%");
            hp.IsVisible = false;
            hp.Transitioning = true;
            Text mp = new Text(font, new Vector2(85, 25),"100%");
            mp.IsVisible = false;
            mp.Transitioning = true;
            Text sp = new Text(font,new Vector2(85,45),"100%");
            sp.IsVisible = false;
            sp.Transitioning = true;
            Sprite hud = AssetManager.Instance.Get<Sprite>(SpriteNames.Hud);
            hud.Position = Vector2.Zero;
            Sprite healthbar = AssetManager.Instance.Get<Sprite>(SpriteNames.HealthBar);
            healthbar.Position = new Vector2(83,6);
            Sprite manaBar = AssetManager.Instance.Get<Sprite>(SpriteNames.ManaBar);
            manaBar.Position = new Vector2(83, 26);
            Sprite spiritBar = AssetManager.Instance.Get<Sprite>(SpriteNames.SpiritBar);
            spiritBar.Position = new Vector2(83, 46);
            
            this.Labels.Add(hp);
            this.Labels.Add(mp);
            this.Labels.Add(sp);
            this.Elements.Add(Hud,hud);
            this.Elements.Add(HP,healthbar);
            this.Elements.Add(MP,manaBar);
            this.Elements.Add(SP,spiritBar);
            this.RenderTarget2D = new RenderTarget2D(this.Device, hud.DefaultSource.Width, hud.DefaultSource.Height);

        }

        public override void Update(GameTime gameTime)
        {
            Vector2 mousePosition = Mouse.GetState(ScreenManager.Window).Position.ToVector2() - this.Transform.Position;
            
            this.Labels[0].IsVisible = Elements[HP].Contains(mousePosition);
            this.Labels[1].IsVisible = Elements[MP].Contains(mousePosition);
            this.Labels[2].IsVisible = Elements[SP].Contains(mousePosition);

            if (Keys.NumPad1.Down())
            {
                this.Elements[HP].SourceWidth--;
                this.Labels[0].Contents = Math.Round((this.Elements[HP].SourceWidth /  (double)this.Elements["HealthBar"].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad2.Down())
            {
                this.Elements[HP].SourceWidth++;
                this.Labels[0].Contents = Math.Round((this.Elements[HP].SourceWidth /  (double)this.Elements["HealthBar"].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad4.Down())
            {
                this.Elements[MP].SourceWidth--;
                this.Labels[1].Contents = Math.Round((this.Elements[MP].SourceWidth / (double)this.Elements[MP].DefaultSource.Width) * 100) + "%"; ;

            }
            if (Keys.NumPad5.Down())
            {
                this.Elements[MP].SourceWidth++;
                this.Labels[1].Contents = Math.Round((this.Elements[MP].SourceWidth / (double)this.Elements[MP].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad7.Down())
            {
                this.Elements[MP].SourceWidth--;
                this.Labels[2].Contents = Math.Round((this.Elements[MP].SourceWidth / (double)this.Elements[MP].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad8.Down())
            {
                this.Elements[MP].SourceWidth++;
                this.Labels[2].Contents = Math.Round((this.Elements[MP].SourceWidth / (double)this.Elements[MP].DefaultSource.Width) * 100) + "%"; ;
            }
            base.Update(gameTime);
        }
    }
}
