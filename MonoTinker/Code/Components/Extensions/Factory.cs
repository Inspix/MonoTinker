using System;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.GameComponents;
using MonoTinker.Code.Components.UI;
using MonoTinker.Code.Managers;
using MonoTinker.Code.Utils;
using OpenTK.Graphics.OpenGL;

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
            int count,int fps = 30)
        {
            AnimationV2 result = new AnimationV2(layers[0].Skip(startingIndex).Take(count).ToArray(),atlas, fps);
            for (int i = 1; i < layers.Count; i++)
            {

                result.AddLayer(new Animation(layers[i].Skip(startingIndex).Take(count).ToArray(), atlas, fps));

            }
            return result;
        }

        public static AnimationV2 CreateAnimationWithLayers(SpriteAtlas atlas, List<string[]> layers, int startingIndex,
           int count, Vector2 offset, int fps = 30)
        {
            AnimationV2 result = new AnimationV2(layers[0].Skip(startingIndex).Take(count).ToArray(), atlas, fps);
            for (int i = 1; i < layers.Count; i++)
            {

                result.AddLayer(new Animation(layers[i].Skip(startingIndex).Take(count).ToArray(), atlas, fps),"",offset);

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

        public static AnimationV2 CharacterAnimation()
        {
            AnimationV2 toReturn = new AnimationV2(AssetManager.Instance.Get<Animation>(An.Walk.Human + "Down"));
            return toReturn;
        }

        public static void AddLayer(ref AnimationV2 anim, string layertag,Vector2 offset = default(Vector2))
        { 
            anim.AddLayer(AssetManager.Instance.Get<Animation>(layertag).Copy(), layertag,offset);
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

            ScreenManager.Device.SetRenderTarget(button);
            ScreenManager.Device.Clear(Color.FromNonPremultiplied(0, 0, 0, 0));
            batch.Begin();
            buttonL.Draw(batch);
            buttonR.Draw(batch);
            ball.Draw(batch);
            ball2.Draw(batch);
            batch.End();

            ScreenManager.Device.SetRenderTarget(buttonH);
            ScreenManager.Device.Clear(Color.FromNonPremultiplied(0, 0, 0, 0));
            batch.Begin();
            buttonLHover.Draw(batch);
            buttonRHover.Draw(batch);
            ball.Draw(batch);
            ball2.Draw(batch);
            batch.End();

            ScreenManager.Device.SetRenderTarget(buttonC);
            ScreenManager.Device.Clear(Color.FromNonPremultiplied(0, 0, 0, 0));
            batch.Begin();
            buttonLClicked.Draw(batch);
            buttonRClicked.Draw(batch);
            ball.Draw(batch);
            ball2.Draw(batch);
            batch.End();

            ScreenManager.Device.SetRenderTarget(null);
            Sprite b = new Sprite(button);
            Sprite h = new Sprite(buttonH);
            Sprite c = new Sprite(buttonC);

            Button result = new Button(Vector2.Zero, b, h, c);
            result.Transform.Scale = new Vector2(scale, scale);
            result.Position = position;
            return result;
        }
    }
   
}
