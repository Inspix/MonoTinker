using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Elements.DebugGraphics;
using MonoTinker.Code.Components.Extensions;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;
using OpenTK.Graphics.ES11;

namespace MonoTinker.Code.Components.UI
{
    public class ColorPicker : InterfaceElement
    {
        private Color[] colors;
        private Button rngButton;
        private bool hasRandomButton;
        public ColorPicker(Vector2 position, GraphicsDevice device,int rows,bool hasRandomButton = false,Color[] colors = null) : base(position, device)
        {
            this.colors = colors;
            this.hasRandomButton = hasRandomButton;
            this.Init(rows);
        }

        private void Init(int rows)
        {
            int slotCount = 1;
            for (int i = 1; i <= rows; i++)
            {
                Sprite left = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemLeftSilver).DirectClone();
                Sprite middle = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemMiddleSilver).DirectClone();
                Sprite middle2 = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemMiddleSilver).DirectClone();
                Sprite right = AssetManager.Instance.Get<Sprite>(Sn.Menu.ItemRightSilver).DirectClone();

                if (Width == 0)
                {
                    this.Width += left.DefaultSource.Width;
                    this.Width += middle.DefaultSource.Width * 2;
                    this.Width += right.DefaultSource.Width;
                }
                left.Position = Vector2.UnitY * this.Height;
                middle.Position = left.Position + Vector2.UnitX * left.SourceWidth;
                middle2.Position = middle.Position + Vector2.UnitX * middle.SourceWidth;
                right.Position = middle2.Position + Vector2.UnitX * middle2.SourceWidth;
                Height += left.SourceHeight;
                this.Elements.Add("slot" + slotCount++, left);
                this.Elements.Add("slot" + slotCount++, middle);
                this.Elements.Add("slot" + slotCount++, middle2);
                this.Elements.Add("slot" + slotCount++, right);
            }
            if (hasRandomButton)
            {
                rngButton = Factory.FancyButton(Batch, Vector2.Zero, Sn.Menu.BlueBall, 1.3f);
                rngButton.Position = new Vector2((this.Width/2f - rngButton.Size.X/2f),this.Height+10);
                rngButton.ClickType = ClickType.Single;
                rngButton.ClickCallback = RandomizeColors;
                Text rngText = new Text(AssetManager.Instance.Get<SpriteFont>("Standart"),rngButton.Position + new Vector2(40,12),"Random");
                rngText.Clr = Color.Black;
                rngText.Transform.Scale = Vector2.One * 0.25f; 
                Labels.Add(rngText);
                this.Height += 50;

            }


            if (colors == null)
            {
                colors = new Color[slotCount];
                RandomizeColors();
            }

            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }


        private void RandomizeColors()
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(
                    ScreenManager.Rng.Next(0, 256),
                    ScreenManager.Rng.Next(0, 256),
                    ScreenManager.Rng.Next(0, 256));
            }
        }
        public Action<Color> PickCallback { private get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!IsVisible)
            {
                return;
            }
            
            Vector2 mousePos = InputHandler.MousePos() - this.Transform.Position;
            if (hasRandomButton)
            {
                rngButton.Over(mousePos);
                rngButton.Update();
            }
            foreach (var spriteAtla in Elements)
            {
                bool result = spriteAtla.Value.Contains(mousePos);
                spriteAtla.Value.Clr = result ? Color.DarkGoldenrod: Color.White;
                if (result)
                {
                    int index = int.Parse(spriteAtla.Key.Replace("slot", ""));
                    if (InputHandler.MouseDownOnce("left"))
                    {
                        PickCallback.Invoke(colors[index-1]);
                    }
                }
            }
        }

        public override void DrawElements()
        {
            if (!IsVisible)
            {
                return;
            }
            Device.SetRenderTarget(this.RenderTarget2D);
            Device.Clear(Color.FromNonPremultiplied(0,0,0,0));
            Batch.Begin();
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw(Batch);
                DebugShapes.DrawFilledRectangle(Batch, this.Elements[i].Position + ((i % 4 != 0) ? new Vector2(8, 4) : new Vector2(18, 4)), new Vector2(32,33), colors[i]);
            }
            if (hasRandomButton)
            {
                rngButton.Draw(Batch);
            }
            foreach (var label in Labels)
            {
                label.Draw(Batch);
            }
            Batch.End();
            Device.SetRenderTarget(null);
        }

        public static ColorPicker SkinTonePicker(Vector2 position)
        {
            return new ColorPicker(position,ScreenManager.Device,4,false,ColorHelper.SkinTones());
        }
        
    }
}
