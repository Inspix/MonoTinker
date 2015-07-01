using System;
using MonoTinker.Code.Components.GameComponents;
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
    }
   
}
