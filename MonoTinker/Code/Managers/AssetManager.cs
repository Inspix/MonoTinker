
namespace MonoTinker.Code.Managers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using Components;
    using Components.Elements;
    using Utils;

    public enum TextureType
    {
        Sprite,Animation,SpriteSheet
    }

    public class AssetManager
    {
        private static AssetManager instance;
        private ContentManager content;

        private SpriteAtlas spriteAtlas;          // To Store our sprite sheets

        private Dictionary<string, Animation> animations;  // To Store our animations

        private Dictionary<string, SpriteFont> fonts;
        // TODO: dictionary for sounds  

        private AssetManager()
        {
            this.spriteAtlas = new SpriteAtlas();
            this.animations = new Dictionary<string, Animation>();
            this.fonts = new Dictionary<string, SpriteFont>();
        }

        public static AssetManager Instance                         // Instance Getter
        {
            get { return instance ?? (instance = new AssetManager()); }
        }

        public void LoadContent(ContentManager content)             // Preload Content
        {
            this.content = content;
            this.AddSprite("UI/frame","UIFrame");
            this.spriteAtlas.PopulateFromSpriteSheetAlt(content,"UI/hud");
            this.fonts.Add("UIFont",content.Load<SpriteFont>("UI/InterfaceFont"));
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
                object toReturn = this.spriteAtlas[id].Texture;
                return (T)toReturn;
            }
            if (typeof(T) == typeof(Sprite))
            {
                object toReturn = this.spriteAtlas[id];
                return (T) toReturn;
            }
            if (typeof(T) == typeof(Animation))
            {
                object toReturn = this.animations[id];
                return (T) toReturn;
            }
            if (typeof(T) == typeof(SpriteAtlas))
            {
                object toReturn = this.spriteAtlas;
                return (T)toReturn;
            }
            if (typeof(T) == typeof(SpriteFont))
            {
                object toReturn = this.fonts[id];
                return (T)toReturn;
            }

            Debug.Error("error at Get<T> method: unsupported type ({0})", (typeof(T)));
            return obj1;
        }

        #region Add Sprites
        public void AddSprite(string filename, string id = "")
        {
            try
            {
                spriteAtlas.Add(id ?? filename, new Sprite(content.Load<Texture2D>(filename)));
            }
            catch (Exception e)
            {
                Debug.Warning("Exception when adding a Sprite from file : {0}", filename);
                Debug.Error(e.StackTrace);
            }
        }

        public void AddSprite(string filename, Rectangle sourceRectangle, string id)
        {
            try
            {
                spriteAtlas.Add(id, new Sprite(content.Load<Texture2D>(filename), sourceRectangle));
            }
            catch (Exception e)
            {
                Debug.Warning("Exception when adding a Sprite from file : {0}", filename);
                Debug.Error(e.StackTrace);
            }
        }
        #endregion

        #region Add Animation
        public void AddAnimation(Animation anim, string id)
        {
            try
            {
                animations.Add(id, anim);

            }
            catch (Exception e)
            {
                Debug.Warning("Exception when adding a Animation : {0}", id);
                Debug.Error(e.StackTrace);
            }
        }
        #endregion
        
    }
}
