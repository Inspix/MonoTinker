using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoTinker.Code.Components.Elements;

namespace MonoTinker.Code.Components.Extensions
{
    public static class Factory
    {
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
    }
   
}
