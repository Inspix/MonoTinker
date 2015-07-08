using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.GameComponents;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;
using Button = MonoTinker.Code.Components.UI.Button;

namespace MonoTinker.Code.Components.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;

    using Elements;

    public static class Factory
    {
        private static Random rng = ScreenManager.Rng;

        public static AnimationV2 CreateAnimationWithLayers(SpriteAtlas atlas, List<string[]> layers, int startingIndex,
            int count, string tag, int fps = 30)
        {
            AnimationV2 result = new AnimationV2(layers[0].Skip(startingIndex).Take(count).ToArray(),atlas, fps);
            for (int i = 1; i < layers.Count; i++)
            {

                result.AddLayer(new Animation(layers[i].Skip(startingIndex).Take(count).ToArray(), atlas, fps),tag);

            }
            return result;
        }

        public static Item CreateItem(string name, int maxStr, int maxAgi, int maxVit, int maxInt, int maxWis, ItemRarity itemRarity = ItemRarity.Common)
        {
            Stats stats = new Stats(rng.Next(1,maxStr+1), rng.Next(1, maxAgi + 1), rng.Next(1, maxVit + 1), rng.Next(1, maxInt + 1), rng.Next(1, maxWis + 1));
            if (itemRarity == ItemRarity.Common)
            {
                itemRarity = stats.TotalStats > 20 ? ItemRarity.Uncommon : ItemRarity.Common;
            }
            return new Item(name,stats, itemRarity);
        }

        public static Animation CreateAnimation(ref SpriteAtlas atlas,  string[] framenames, int startingindex, int count, int fps = 10)
        {
            return new Animation(framenames.Skip(startingindex).Take(count).ToArray(), atlas, fps);
        }

        public static Animation[] CreateWalking(string[] framenames,ref SpriteAtlas atlas)
        {
            Animation[] result = new Animation[8];
            result[0] = CreateAnimation(ref atlas, framenames, 0, 1);  // Idle up
            result[1] = CreateAnimation(ref atlas, framenames, 18, 1); // Idle down
            result[2] = CreateAnimation(ref atlas, framenames, 9, 1);  // Idle left
            result[3] = CreateAnimation(ref atlas, framenames, 27, 1); // Idle right
            result[4] = CreateAnimation(ref atlas, framenames, 1, 8);  // walk up
            result[5] = CreateAnimation(ref atlas, framenames, 19, 8); // walk down
            result[6] = CreateAnimation(ref atlas, framenames, 10, 8); // walk left
            result[7] = CreateAnimation(ref atlas, framenames, 28, 8); // walk righ
            return result;
        }

        public static Animation[] CreateUDLR(string[] framenames, ref SpriteAtlas atlas, int perRow)
        {
            Animation[] result = new Animation[4];
            result[0] = CreateAnimation(ref atlas, framenames, 0, perRow);  //  up
            result[1] = CreateAnimation(ref atlas, framenames, perRow, perRow); //  left
            result[2] = CreateAnimation(ref atlas, framenames, perRow * 2, perRow);  //  down
            result[3] = CreateAnimation(ref atlas, framenames, perRow * 3, perRow); //  right
            return result;
        }
       

        public static AnimationV2 HumanDownWalk()
        {
            AnimationV2 toReturn = new AnimationV2(AssetManager.Instance.Get<Animation>(An.Walk.BodyHuman + "Down"));
            toReturn.AddLayer(AssetManager.Instance.Get<Animation>(An.Walk.TorsoLeatherShirt + "Down"),"shirt");
            toReturn.AddLayer(AssetManager.Instance.Get<Animation>(An.Walk.LegsPants + "Down"), "legs");

            return toReturn;
        }

        public static void AddLayer(ref AnimationV2 anim, string layertag, Color clr = default(Color))
        {
            Animation result = AssetManager.Instance.Get<Animation>(layertag).Copy();
            result.Tint = clr == default(Color) ? Color.White : clr;
            anim.AddLayer(result, layertag);
        }


        public static Button FancyButton(SpriteBatch batch,Vector2 position,string balltype = Sn.Menu.BlueBall, float scale = 1)
        {
            Sprite buttonL = AssetManager.Instance.Get<Sprite>(Sn.Menu.ButtonFancyLeft).DirectClone();
            buttonL.Clr = ColorHelper.Saturation(buttonL.Clr, 0.8f);
            Sprite buttonLHover = AssetManager.Instance.Get<Sprite>(Sn.Menu.ButtonFancyLeft).DirectClone();
            Sprite buttonLClicked = AssetManager.Instance.Get<Sprite>(Sn.Menu.ButtonFancyLeftClicked).DirectClone();
            Sprite buttonR = AssetManager.Instance.Get<Sprite>(Sn.Menu.ButtonFancyRight).DirectClone();
            buttonR.Clr = ColorHelper.Saturation(buttonR.Clr, 0.8f);
            Sprite buttonRHover = AssetManager.Instance.Get<Sprite>(Sn.Menu.ButtonFancyRight).DirectClone();
            Sprite buttonRClicked = AssetManager.Instance.Get<Sprite>(Sn.Menu.ButtonFancyRightClicked).DirectClone();

            Sprite ball = AssetManager.Instance.Get<Sprite>(balltype).DirectClone();
            Sprite ball2 = AssetManager.Instance.Get<Sprite>(balltype).DirectClone();


            buttonL.Position = Vector2.Zero;
            buttonR.Position = Vector2.Zero + Vector2.UnitX * buttonL.SourceWidth;

            buttonLHover.Position = Vector2.Zero;
            buttonRHover.Position = Vector2.Zero + Vector2.UnitX * buttonL.SourceWidth;

            buttonLClicked.Position = Vector2.Zero;
            buttonRClicked.Position = Vector2.Zero + Vector2.UnitX * buttonL.SourceWidth;

            ball.Position = buttonL.Position + Vector2.One * 5;
            ball2.Position = buttonR.Position + new Vector2((buttonR.SourceWidth - ball2.SourceWidth), 10) - Vector2.One * 5;

            RenderTarget2D button = new RenderTarget2D(batch.GraphicsDevice, buttonL.SourceWidth + buttonR.SourceWidth, buttonL.SourceHeight);
            RenderTarget2D buttonH = new RenderTarget2D(batch.GraphicsDevice, buttonL.SourceWidth + buttonR.SourceWidth, buttonL.SourceHeight);
            RenderTarget2D buttonC = new RenderTarget2D(batch.GraphicsDevice, buttonL.SourceWidth + buttonR.SourceWidth, buttonL.SourceHeight);

            batch.GraphicsDevice.SetRenderTarget(button);
            batch.GraphicsDevice.Clear(Color.FromNonPremultiplied(0, 0, 0, 0));
            batch.Begin();
            buttonL.Draw(batch);
            buttonR.Draw(batch);
            ball.Draw(batch);
            ball2.Draw(batch);
            batch.End();

            batch.GraphicsDevice.SetRenderTarget(buttonH);
            batch.GraphicsDevice.Clear(Color.FromNonPremultiplied(0, 0, 0, 0));
            batch.Begin();
            buttonLHover.Draw(batch);
            buttonRHover.Draw(batch);
            ball.Draw(batch);
            ball2.Draw(batch);
            batch.End();

            batch.GraphicsDevice.SetRenderTarget(buttonC);
            batch.GraphicsDevice.Clear(Color.FromNonPremultiplied(0, 0, 0, 0));
            batch.Begin();
            buttonLClicked.Draw(batch);
            buttonRClicked.Draw(batch);
            ball.Draw(batch);
            ball2.Draw(batch);
            batch.End();

            batch.GraphicsDevice.SetRenderTarget(null);
            Sprite b = new Sprite(button);
            Sprite h = new Sprite(buttonH);
            Sprite c = new Sprite(buttonC);

            Button result = new Button(Vector2.Zero, b, h, c);
            result.Scale = new Vector2(scale, scale);
            result.Position = position;
            return result;
        }

        public static Button ArrowButton(SpriteBatch batch, Vector2 position, Color color, float scale, bool flip, string type = null)
        {
            Sprite arrow = AssetManager.Instance.Get<Sprite>(type ?? "BasicArrow").DirectClone();
            arrow.Effect = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Sprite arrowHover = arrow.DirectClone();
            Sprite arrowClick = arrow.DirectClone();


            arrow.Clr = color;
            arrowHover.Clr = color*0.6f;
            arrowClick.Clr = color*0.3f;
            Button result = new Button(position, arrow, arrowHover, arrowClick);
            result.ScaleF = scale;
            return result;
        }

    }
   
}
