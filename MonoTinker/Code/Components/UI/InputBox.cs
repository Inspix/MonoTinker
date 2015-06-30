using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Elements.DebugGraphics;
using MonoTinker.Code.Components.Extensions;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.UI
{
    public class InputBox : InterfaceElement
    {
        private Vector2 cursorPosition;
        private Vector2 defaultCursorPosition;
        private Vector2 fontScale;
        private SpriteFont font;
        private bool blink;
        public InputBox(Vector2 position, GraphicsDevice device, int width, int height) : base(position, device)
        {
            this.Width = width;
            this.Height = height;
            this.Init();
        }

        private void Init()
        {
            int boxWidth = 0;
            int boxHeight = 0;
            font = AssetManager.Instance.Get<SpriteFont>("SplashScreenFont");
            font.MeasureString("A");
            fontScale = new Vector2(0.5f,0.3f);
            TextBoxFactory.GenerateBox(new Point(10,0),ref Elements,ref boxWidth,ref boxHeight);
            Vector2 center = new Vector2((this.Width/2f) - (boxWidth/2f), (this.Height / 2f) - (boxHeight / 2f));
            foreach (Sprite sprite in Elements.Values)
            {
                sprite.Position += center;
            }
            defaultCursorPosition = Elements[0].Position + Vector2.One * 12;
            cursorPosition = defaultCursorPosition;
            Text input = new Text(font, defaultCursorPosition, "");
            input.Transform.Scale = fontScale;
            Labels.Add(input);

            timeToUpdate = TimeSpan.FromSeconds(1).TotalSeconds;
            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }

        public Action<string> Callback { get; set; }

        public override void DrawElements()
        {
            Device.SetRenderTarget(this.RenderTarget2D);
            Device.Clear(Color.FromNonPremultiplied(0,0,0,100));
            Batch.Begin();
            foreach (var spriteAtla in Elements)
            {
                spriteAtla.Value.Draw(Batch);
            }
            if (blink)
            {
                DebugShapes.DrawLine(Batch,cursorPosition,cursorPosition + Vector2.UnitY * 40,Color.White);
            }
            foreach (var label in Labels)
            {
                label.Draw(Batch);
            }
            Batch.End();
            Device.SetRenderTarget(null);
        }

        private double timeToUpdate;
        private double timeElapsed;
        public override void Update(GameTime gameTime)
        {
            foreach (Keys currentKey in InputHandler.GetCurrentKeys())
            {
                if ((int) currentKey >= 65 && (int) currentKey <= 90 && currentKey.DownOnce())
                {
                    Labels[0].Append(currentKey.ToString());
                    cursorPosition = new Vector2((font.MeasureString(Labels[0].Contents).X)*fontScale.X, 0) +
                                     defaultCursorPosition;
                }

                if (currentKey.DownOnce() && currentKey == Keys.Back)
                {
                    if (Labels[0].Contents.Length != 0)
                    {
                        Labels[0].RemoveLast();
                        cursorPosition = new Vector2((font.MeasureString(Labels[0].Contents).X) * fontScale.X, 0) +
                                         defaultCursorPosition;
                    }
                }
                if (currentKey.DownOnce() && currentKey == Keys.Enter)
                {
                    Callback.Invoke(this.Labels[0].Contents);
                }
            }
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed >= timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                blink = !blink;
            }
            base.Update(gameTime);
        }
    }
}
