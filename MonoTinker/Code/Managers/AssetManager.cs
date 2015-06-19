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
            this.AddSprite("ghettoville1","BG");
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

        public Texture2D Texture(string key,TextureType type = TextureType.Sprite)  // Get a texture from classes that have a Texture2D
        {
            switch (type)
            {
                case TextureType.Animation:
                    if (animations.ContainsKey(key))
                    {
                        //return animations[key].Texture;
                    }
                    Debug.Error("Invalid animation key: \"{0}\"", key);
                    return null;

                case TextureType.Sprite:
                    if (sprites.ContainsKey(key))
                    {
                        return this.sprites[key].Texture;
                    }
                    Debug.Error("Invalid sprite key: \"{0}\"", key);
                    return null;

                    case TextureType.SpriteSheet:
                    if (spriteSheet.ContainsKey(key))
                    {
                        return this.spriteSheet[key].Texture;
                    }
                    Debug.Error("Invalid sprite sheet key: \"{0}\"", key);
                    return null;
                default:
                    return null;
            }
        }

        public Sprite Sprite(string key)
        {
            if (sprites.ContainsKey(key))
            {
                return this.sprites[key];
            }
            Debug.Error("Invalide sprite key: \"{0}\"", key);
            return null;
        }

        public Animation Animations(string key)
        {
            if (animations.ContainsKey(key))
            {
                return this.animations[key];
            }
            Debug.Error("Invalid animation key: \"{0}\"", key);
            return null;
        }
        



        #region Add Sprite Methods
        public void AddSprite(string filename, string id = "")
        {
            try
            {
                sprites.Add(id ?? filename, new Sprite(filename, content));
            }
            catch (Exception e)
            {
                Debug.Warning("exception at AssetManager - AddSprite (string,string)");
                Debug.Error(e.Source);
            }
        }/*

        public void AddSprite(string spriteSheetId, string newId, Rectangle source)
        {
            try
            {
                Sprite toAdd = new Sprite(spriteSheet[spriteSheetId], source);
                sprites.Add(newId, toAdd);
            }
            catch (Exception e)
            {
                Debug.Warning("exception at AssetManager - AddSprite (string,string,Rectangle)");
                Debug.Error(e.Source);
            }
        }*/
        #endregion

        #region Add Animation Methods
        /*public void AddAnimation(Rectangle source, string filename, string id = "")
        {
            try
            {
                animations.Add(id ?? filename, new Animation(content.Load<Texture2D>(filename), source));
            }
            catch (Exception e)
            {
                Debug.Warning("exception at AssetManager - AddAnimation (Rectangle,string,string)");
                Debug.Error(e.Source);
            }
        }

        public void AddAnimation(Rectangle source, int fps, string filename, string id = "")
        {
            try
            {
                animations.Add(id ?? filename, new Animation(content.Load<Texture2D>(filename), source, fps));
            }
            catch (Exception e)
            {
                Debug.Warning("exception at AssetManager - AddAnimation (Rectangle,int fps,string,string)");
                Debug.Error(e.Source);
            }
        }

        public void AddAnimation(Rectangle source, int fps, int missingFrames, string filename, string id = "")
        {
            try
            {
                animations.Add(id ?? filename, new Animation(content.Load<Texture2D>(filename), source, fps, missingFrames));
            }
            catch (Exception e)
            {
                Debug.Warning("exception at AssetManager - AddAnimation (Rectangle,int,int,string,string)");
                Debug.Error(e.Source);
            }
        } */
        #endregion

        /*public void AddSpriteSheet(string filename, string id = "")
        {
            try
            {
                spriteSheet.Add(id ?? filename, content.Load<Texture2D>(filename));
            }
            catch (Exception e)
            {
                Debug.Warning("exception at AssetManager - AddSpriteSheet (string,string)");
                Debug.Error(e.Source);
            }
        }*/
    }
}
