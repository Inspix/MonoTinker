using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Elements.DebugGraphics;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.UI
{
    public class ColorPicker : InterfaceElement
    {
        private Color[] colors;
        public ColorPicker(Vector2 position, GraphicsDevice device,int rows,Color[] colors = null) : base(position, device)
        {
            this.colors = colors;
            this.Init(rows);
        }

        private void Init(int rows)
        {
            this.Transitioning = true;
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
            if (colors == null)
            {
                colors = new Color[slotCount];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = new Color(
                        ScreenManager.Rng.Next(100, 256), 
                        ScreenManager.Rng.Next(100, 256),
                        ScreenManager.Rng.Next(100, 256));
                }
            }

            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }

        public Action<Color> PickCallback { private get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 mousePos = InputHandler.MousePos() - this.Transform.Position;
            foreach (var spriteAtla in Elements.Where(s => s.Key.Contains("slot")))
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
            Device.SetRenderTarget(this.RenderTarget2D);
            Device.Clear(Color.FromNonPremultiplied(0,0,0,0));
            Batch.Begin();
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw(Batch);
                DebugShapes.DrawFilledRectangle(Batch, this.Elements[i].Position + ((i % 4 != 0) ? new Vector2(8, 4) : new Vector2(18, 4)), Vector2.One*32,colors[i]);
            }
            Batch.End();
            Device.SetRenderTarget(null);
        }
    }
}
