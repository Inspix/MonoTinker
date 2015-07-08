using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Components.Extensions;
using MonoTinker.Code.Components.GameComponents;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components.UI
{
    public class StatPicker : InterfaceElement
    {
        private Stats currentStats;
        private Stats newStats;
        private Button[] buttons;
        private bool incButtons;
        private int pointsRemaining;
        private int startingpoints;

        public StatPicker(Vector2 position, GraphicsDevice device,Stats startingStats, int pointsRemaining = 5) : base(position, device)
        {
            this.currentStats = startingStats;
            this.newStats = currentStats.DirectCopy();
            this.pointsRemaining = startingpoints = pointsRemaining;
            this.Init();
        }

        private void Init()
        {
            OverrideDrawElements = true;
            buttons = new Button[10];
            string[] values = new[] {"Strenght", "Agility", "Vitality", "Intellect", "Wisdom"};
            SpriteFont font = AssetManager.Instance.Get<SpriteFont>("Standart");
            Vector2 fontSize = font.MeasureString("A")*0.5f;
            Sprite box = BoxFactory.BoxSpriteWithText(Batch,
                values, TextAlignment.Center, 0.5f,false,true);
            Width = box.SourceWidth;
            Height = box.SourceHeight;
            
            Text strengtValue = new Text(font,new Vector2(box.SourceWidth-30,22), currentStats.Strenght.ToString());
            strengtValue.Scale = Vector2.One*0.4f;
            Text agilityValue = new Text(font, strengtValue.Position, currentStats.Agility.ToString());
            agilityValue.PosY += fontSize.Y+15;
            agilityValue.Scale = Vector2.One * 0.4f;
            Text vitalityValue = new Text(font, agilityValue.Position, currentStats.Vitality.ToString());
            vitalityValue.PosY += fontSize.Y + 15;
            vitalityValue.Scale = Vector2.One * 0.4f;
            Text intellectValue = new Text(font, vitalityValue.Position, currentStats.Intellect.ToString());
            intellectValue.PosY += fontSize.Y + 15;
            intellectValue.Scale = Vector2.One * 0.4f;
            Text wisdomValue = new Text(font, intellectValue.Position, currentStats.Wisdom.ToString());
            wisdomValue.PosY += fontSize.Y + 15;
            wisdomValue.Scale = Vector2.One * 0.4f;
            Text pointsLeft = new Text(font,box.SpriteCenter * Vector2.UnitX,"Points left: "+pointsRemaining);
            pointsLeft.Scale = Vector2.One*0.35f;
            pointsLeft.PosX -= (pointsLeft.Size.X)/2;
            pointsLeft.PosY += box.SourceHeight - (fontSize.Y + 15);

            buttons[0] = Factory.ArrowButton(Batch, strengtValue.Position - new Vector2(25,2), Color.White, 0.5f, false);
            buttons[0].ClickType = ClickType.Single;
            buttons[0].SetClickCallbackWithTag(values[0], Increase);
            buttons[0].Transitioning = true;
            buttons[1] = Factory.ArrowButton(Batch, strengtValue.Position - new Vector2(155, 2), Color.White, 0.5f, true);
            buttons[1].ClickType = ClickType.Single;
            buttons[1].SetClickCallbackWithTag(values[0], Decrease);
            buttons[1].IsVisible = false;
            buttons[1].Transitioning = true;
            int index = 1;
            for (int i = 2; i < buttons.Length; i+=2)
            {
                buttons[i] = buttons[i - 2].DirectCopy();
                buttons[i].SetClickCallbackWithTag(values[index], Increase);
                buttons[i].Position += new Vector2(0, fontSize.Y + 15);
                buttons[i+1] = buttons[i - 1].DirectCopy();
                buttons[i+1].SetClickCallbackWithTag(values[index], Decrease);
                buttons[i+1].Position += new Vector2(0,fontSize.Y + 15);
                index++;
            }
            Elements.Add("box", box);
            Labels.Add(strengtValue);
            Labels.Add(agilityValue);
            Labels.Add(vitalityValue);
            Labels.Add(intellectValue);
            Labels.Add(wisdomValue);
            Labels.Add(pointsLeft);

            this.RenderTarget2D = new RenderTarget2D(Device,Width,Height);
        }

        private void Decrease(string tag)
        {
            switch (tag)
            {
                case "Strenght":
                    if (newStats.Strenght - 1 < currentStats.Strenght)
                    {
                        return;
                    }
                    Labels[0].Contents = (--newStats.Strenght).ToString();
                    if (newStats.Strenght == currentStats.Strenght) buttons[1].IsVisible = false;
                    break;
                case "Agility":
                    if (newStats.Agility - 1 < currentStats.Agility)
                    {
                        return;
                    }
                    Labels[1].Contents = (--newStats.Agility).ToString();
                    if (newStats.Agility == currentStats.Agility) buttons[3].IsVisible = false;
                    break;
                case "Vitality":
                    if (newStats.Vitality - 1 < currentStats.Vitality)
                    {
                        return;
                    }
                    Labels[2].Contents = (--newStats.Vitality).ToString();
                    if (newStats.Vitality == currentStats.Vitality) buttons[5].IsVisible = false;
                    break;
                case "Intellect":
                    if (newStats.Intellect - 1 < currentStats.Intellect)
                    {
                        return;
                    }
                    Labels[3].Contents = (--newStats.Intellect).ToString();
                    if (newStats.Intellect == currentStats.Intellect) buttons[7].IsVisible = false;
                    break;
                case "Wisdom":
                    if (newStats.Wisdom - 1 < currentStats.Wisdom)
                    {
                        return;
                    }
                    Labels[4].Contents = (--newStats.Wisdom).ToString();
                    if (newStats.Wisdom == currentStats.Wisdom) buttons[9].IsVisible = false;
                    break;
            }
            pointsRemaining++;
            Labels[5].Contents = "Points left: " + pointsRemaining;
            if (!incButtons)
            {
                ToggleIncreaseButtons();
            }
        }

        private void Increase(string tag)
        {
            if (pointsRemaining <= 0)
            {
                return;
            }
            switch (tag)
            {
                case "Strenght":
                    Labels[0].Contents = (++newStats.Strenght).ToString();
                    if (newStats.Strenght > currentStats.Strenght)
                    {
                        buttons[1].IsVisible = true;
                    }
                    break;
                case "Agility":
                    
                    Labels[1].Contents = (++newStats.Agility).ToString();
                    if (newStats.Agility > currentStats.Agility)
                    {
                        buttons[3].IsVisible = true;
                    }
                    break;
                case "Vitality":
                    
                    Labels[2].Contents = (++newStats.Vitality).ToString();
                    if (newStats.Vitality > currentStats.Vitality)
                    {
                        buttons[5].IsVisible = true;
                    }
                    break;
                case "Intellect":
                    
                    Labels[3].Contents = (++newStats.Intellect).ToString();
                    if (newStats.Intellect > currentStats.Intellect)
                    {
                        buttons[7].IsVisible = true;
                    }
                    break;
                case "Wisdom":
                    
                    Labels[4].Contents = (++newStats.Wisdom).ToString();
                    if (newStats.Wisdom > currentStats.Wisdom)
                    {
                        buttons[9].IsVisible = true;
                    }
                    break;
            }
            pointsRemaining--;
            Labels[5].Contents = "Points left: " + pointsRemaining;
            if (pointsRemaining <= 0)
            {
                ToggleIncreaseButtons();
            }
        }

        public void ChangeStartingStats(Stats stats)
        {
            Labels[0].Contents = stats.Strenght.ToString();
            Labels[1].Contents = stats.Agility.ToString();
            Labels[2].Contents = stats.Vitality.ToString();
            Labels[3].Contents = stats.Intellect.ToString();
            Labels[4].Contents = stats.Wisdom.ToString();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].IsVisible = i%2 == 0;
            }
            this.pointsRemaining = startingpoints;
            Labels[5].Contents = "Points left: " + pointsRemaining;
            this.currentStats = stats;
            this.newStats = stats.DirectCopy();
        }


        public override void DrawElements()
        {
            base.DrawElements();
            foreach (var button in buttons)
            {
                button.Draw(Batch);
            }
            Batch.End();
            Device.SetRenderTarget(null);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!IsVisible)
            {
                return;
            }
            Vector2 mousePos = InputHandler.MousePos - this.Position;
            foreach (var button in buttons)
            {
                if (!button.IsVisible)
                {
                    continue;
                }
                button.Over(mousePos);
                button.Update(gameTime);
            }
        }

        private void ToggleIncreaseButtons()
        {
            if (pointsRemaining <= 0)
            {
                for (int i = 0; i < buttons.Length; i += 2)
                {
                    buttons[i].IsVisible = false;
                }
            }
            else
            {
                for (int i = 0; i < buttons.Length; i += 2)
                {
                    buttons[i].IsVisible = true;
                }
            }
        }

    }
}
