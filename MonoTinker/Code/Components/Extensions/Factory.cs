using System;
using MonoTinker.Code.Components.GameComponents;
using MonoTinker.Code.Managers;

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

                result.AddLayer(new Animation(layers[i].Skip(startingIndex).Take(count).ToArray(), atlas, fps),offset);

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
    }
   
}
