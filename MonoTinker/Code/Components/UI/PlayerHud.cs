namespace MonoTinker.Code.Components.UI
{
    using System;
    using Extensions;
    using Utils;
    using Managers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class PlayerHud : InterfaceElement
    {
        public PlayerHud(Vector2 position, GraphicsDevice device)
            : base(position, device)
        {
            this.Init();
        }

        private void Init()
        {
            SpriteFont font = AssetManager.Instance.Get<SpriteFont>("UIFont");
            Text hp = new Text(font,new Vector2(105,25),"100%");
            hp.Transitioning = true;
            Text mp = new Text(font, new Vector2(105, 45),"100%");
            mp.Transitioning = true;
            Text sp = new Text(font,new Vector2(105,65),"100%");
            sp.Transitioning = true;
            this.RenderTarget2D = new RenderTarget2D(this.Device,(int)ScreenManager.ScreenDimensions.X,(int)ScreenManager.ScreenDimensions.Y);
            Sprite hud = AssetManager.Instance.Get<Sprite>(SpriteNames.Hud);
            hud.Position = Vector2.One * 20;
            Sprite healthbar = AssetManager.Instance.Get<Sprite>(SpriteNames.HealthBar);
            healthbar.Position = new Vector2(103,26);
            Sprite manaBar = AssetManager.Instance.Get<Sprite>(SpriteNames.ManaBar);
            manaBar.Position = new Vector2(103, 46);
            Sprite spiritBar = AssetManager.Instance.Get<Sprite>(SpriteNames.SpiritBar);
            spiritBar.Position = new Vector2(103, 66);
            
            this.Labels.Add(hp);
            this.Labels.Add(mp);
            this.Labels.Add(sp);
            this.Elements.Add("Hud",hud);
            this.Elements.Add("HealthBar",healthbar);
            this.Elements.Add("ManaBar",manaBar);
            this.Elements.Add("SpiritBar",spiritBar);
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle healthBar = new Rectangle(
                Elements["HealthBar"].Position.ToPoint(),
                Elements["HealthBar"].DefaultSource.Size);
            this.Labels[0].IsVisible = healthBar.Contains(Mouse.GetState(ScreenManager.Window).Position);
            this.Labels[1].IsVisible = healthBar.OffsetExt(Vector2.UnitY * 20).Contains(Mouse.GetState(ScreenManager.Window).Position);
            this.Labels[2].IsVisible = healthBar.OffsetExt(Vector2.UnitY * 40).Contains(Mouse.GetState(ScreenManager.Window).Position);

            if (Keys.NumPad1.Down())
            {
                this.Elements["HealthBar"].SourceWidth--;
                this.Labels[0].Contents = Math.Round((this.Elements["HealthBar"].SourceWidth /  (double)this.Elements["HealthBar"].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad2.Down())
            {
                this.Elements["HealthBar"].SourceWidth++;
                this.Labels[0].Contents = Math.Round((this.Elements["HealthBar"].SourceWidth /  (double)this.Elements["HealthBar"].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad4.Down())
            {
                this.Elements["ManaBar"].SourceWidth--;
                this.Labels[1].Contents = Math.Round((this.Elements["ManaBar"].SourceWidth / (double)this.Elements["ManaBar"].DefaultSource.Width) * 100) + "%"; ;

            }
            if (Keys.NumPad5.Down())
            {
                this.Elements["ManaBar"].SourceWidth++;
                this.Labels[1].Contents = Math.Round((this.Elements["ManaBar"].SourceWidth / (double)this.Elements["ManaBar"].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad7.Down())
            {
                this.Elements["SpiritBar"].SourceWidth--;
                this.Labels[2].Contents = Math.Round((this.Elements["SpiritBar"].SourceWidth / (double)this.Elements["SpiritBar"].DefaultSource.Width) * 100) + "%"; ;
            }
            if (Keys.NumPad8.Down())
            {
                this.Elements["SpiritBar"].SourceWidth++;
                this.Labels[2].Contents = Math.Round((this.Elements["SpiritBar"].SourceWidth / (double)this.Elements["SpiritBar"].DefaultSource.Width) * 100) + "%"; ;
            }
            base.Update(gameTime);
        }
    }
}
