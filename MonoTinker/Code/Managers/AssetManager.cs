using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Managers
{
    public enum TextureType
    {
        Sprite,Animation,SpriteSheet
    }

    public class AssetManager
    {
        private static AssetManager instance;
        private ContentManager content;

        private Dictionary<string, SpriteSheet> spriteSheet;          // To Store our sprite sheets
        private Dictionary<string, Sprite> sprites;                 // To Store our sprites
        private Dictionary<string, Animation> animations;           // To Store our animations
        // TODO: dictionary for sounds  

        private AssetManager()
        {
            this.spriteSheet = new Dictionary<string, SpriteSheet>();
            this.sprites = new Dictionary<string, Sprite>();
            this.animations = new Dictionary<string, Animation>();
        }

        public static AssetManager Instance                         // Instance Getter
        {
            get { return instance ?? (instance = new AssetManager()); }
        }

        public void LoadContent(ContentManager content)             // Preload Content
        {
            this.content = content;
        }

        public void UnloadContent()
        {
            this.content.Unload();
        }

        public T Get<T>(string id)                                  // Get an asset based on type
        {
            T obj1 = default(T);
            if (typeof(T) == typeof(Texture2D))
            {
                object toReturn = this.sprites[id].Texture;
                return (T)toReturn;
            }
            if (typeof(T) == typeof(Sprite))
            {
                object toReturn = this.sprites[id];
                return (T) toReturn;
            }
            if (typeof(T) == typeof(Animation))
            {
                object toReturn = this.animations[id];
                return (T) toReturn;
            }
           
            Debug.Error("error at Get<T> method: unsupported type ({0})", (typeof(T)));
            return obj1;
        }
    }
}
