
using System.IO;
using System.Threading;
using MonoTinker.Code.Components.Extensions;

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
        private GraphicsDevice device;

        private SpriteAtlas spriteAtlas;          // To Store our sprite sheets
        private Dictionary<string, Effect> shaders; 
        private Dictionary<string, Animation> animations;
        private Dictionary<string, AnimationV2> animationV2s;

        private Dictionary<string, SpriteFont> fonts;
        // TODO: dictionary for sounds  

        private AssetManager()
        {
            this.spriteAtlas = new SpriteAtlas();
            this.animations = new Dictionary<string, Animation>();
            this.animationV2s = new Dictionary<string, AnimationV2>();
            this.fonts = new Dictionary<string, SpriteFont>();
            this.shaders = new Dictionary<string, Effect>();
        }

        public static AssetManager Instance                         // Instance Getter
        {
            get { return instance ?? (instance = new AssetManager()); }
        }

        public void LoadContent(ContentManager content,GraphicsDevice device)             // Preload Content
        {
            this.content = content;
            this.device = device;
            this.shaders.Add("Grayscale",content.Load<Effect>("Shaders/SpriteGrayscale"));
            this.fonts.Add("UIFont",content.Load<SpriteFont>("UI/InterfaceFont"));
            this.fonts.Add("Standart", content.Load<SpriteFont>("UI/Standart"));
            this.fonts.Add("SplashScreenFont",content.Load<SpriteFont>("SplashScreen/SplashFont"));
            this.LoadSprites();
            this.LoadAnimationFrames();
        }

        private void LoadSprites()
        {
            this.spriteAtlas.Add("BasicArrow",new Sprite(TextureMaker.Arrow(device,32,Color.Transparent,Color.White,Color.Transparent)));
            this.spriteAtlas.Add("BigArrow", new Sprite(TextureMaker.Arrow(device, 128, Color.Transparent, Color.White, Color.Transparent)));
            this.spriteAtlas.Add("BasicCheckbox", new Sprite(TextureMaker.CheckBox(device, 32, Color.Transparent, Color.White, Color.White)));
            this.spriteAtlas.Add("BigCheckbox", new Sprite(TextureMaker.Arrow(device, 128, Color.Transparent, Color.White, Color.White)));
            this.AddSprite("UI/frame","UIFrame");
            this.AddSprite("Other/Lighting","lighting");
            this.spriteAtlas.PopulateFromSpriteSheetAlt(content, "UI/hud");
            this.spriteAtlas.PopulateFromSpriteSheetAlt(content, "Items/weapons");
        }

        private void LoadAnimationFrames()
        {
            LoadSpriteSheets(content.RootDirectory + "/Sprites/");
        }

        private void LoadSpriteSheets(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                LoadSheet(file);
            }
            string[] subdirs = Directory.GetDirectories(path);
            foreach (var subdir in subdirs)
            {
                LoadSpriteSheets(subdir);
            }
        }

        private void LoadSheet(string filename)
        {
            int sub = filename.IndexOf("/")+1;
            filename = filename.Replace("\\", "/");
            filename = filename.Substring(sub);
            int rem = filename.LastIndexOf(".");
            filename = filename.Remove(rem);
            string[] tags = filename.Split('/');
            string tag = tags[tags.Length - 1] + tags[tags.Length - 2] + tags[tags.Length - 3];

            Texture2D texture = content.Load<Texture2D>(filename);
            float x, y;
            switch (tags[tags.Length - 2])
            {

                case "walk":
                    AddWalking(spriteAtlas.PopulateFromSpriteSheet(texture, new Vector2(64), tag), tag);
                    break;

                case "slash":
                    x = texture.Width / 6f;
                    y = texture.Height / 4f;
                    AddAnimationUDLR(spriteAtlas.PopulateFromSpriteSheet(texture, new Vector2(x, y), tag), tag, 6, x > 64);
                    break;

                case "bow":
                    x = texture.Width / 13f;
                    y = texture.Height / 4f;
                    AddAnimationUDLR(spriteAtlas.PopulateFromSpriteSheet(texture, new Vector2(x, y), tag), tag, 13, x > 64);
                    break;

                case "spellcast":
                    Console.WriteLine(tag);
                    x = texture.Width / 7f;
                    y = texture.Height / 4f;
                    AddAnimationUDLR(spriteAtlas.PopulateFromSpriteSheet(texture, new Vector2(x, y), tag), tag, 7, x > 64);
                    break;
                case "thrust":
                    x = texture.Width / 8f;
                    y = texture.Height / 4f;
                    AddAnimationUDLR(spriteAtlas.PopulateFromSpriteSheet(texture, new Vector2(x, y), tag), tag, 8, x > 64);
                    break;
                case "hurt":
                    x = texture.Width / 6f;
                    y = texture.Height;
                    AddHurt(spriteAtlas.PopulateFromSpriteSheet(texture, new Vector2(x, y), tag), tag);
                    break;

            }
            //todo All other states and animation layers
        }


        public void UnloadContent()
        {
            this.content.Unload();
        }

        private void AddWalking(string[] framenames,string tag)
        { 
            Animation[] result = Factory.CreateWalking(framenames, ref spriteAtlas);
            this.animations.Add(tag + "idleUp", result[0]);
            this.animations.Add(tag + "idleDown", result[1]);
            this.animations.Add(tag + "idleLeft", result[2]);
            this.animations.Add(tag + "idleRight", result[3]);
            this.animations.Add(tag + "Up", result[4]);
            this.animations.Add(tag + "Down", result[5]);
            this.animations.Add(tag + "Left", result[6]);
            this.animations.Add(tag + "Right", result[7]);
        }

        private void AddHurt(string[] framenames, string tag)
        {
            this.animations.Add(tag + "Hurt",Factory.CreateAnimation(ref spriteAtlas,framenames,0,6));
        }

        private void AddAnimationUDLR(string[] framenames, string tag, int perRow,bool offset = false)
        {
            Animation[] result = Factory.CreateUDLR(framenames, ref spriteAtlas, perRow);
            if (offset)
            {
                foreach (var animation in result)
                {
                    animation.Offset = new Vector2(-64);
                }
            }
            this.animations.Add(tag + "Up", result[0]);
            this.animations.Add(tag + "Left", result[1]);
            this.animations.Add(tag + "Down", result[2]);
            this.animations.Add(tag + "Right", result[3]);
        }
        

        public AnimationController GetBaseWalkingController(string tag)
        {
            if (!this.animations.ContainsKey(tag + "Up"))
            {
                Debug.Error("Theres no loaded walking animation with tag: {0}",tag);
                return null;
            }
            AnimationController result = new AnimationController();
            result.AddState("idleUp",new AnimationV2(this.animations[tag + "idleUp"].Copy()));
            result.AddState("idleDown", new AnimationV2(this.animations[tag + "idleDown"].Copy()));
            result.AddState("idleLeft", new AnimationV2(this.animations[tag + "idleLeft"].Copy()));
            result.AddState("idleRight", new AnimationV2(this.animations[tag + "idleRight"].Copy()));
            result.AddState("Up", new AnimationV2(this.animations[tag + "Up"].Copy()));
            result.AddState("Down", new AnimationV2(this.animations[tag + "Down"].Copy()));
            result.AddState("Left", new AnimationV2(this.animations[tag + "Left"].Copy()));
            result.AddState("Right", new AnimationV2(this.animations[tag + "Right"].Copy()));
            return result;
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
            if (typeof(T) == typeof(Animation))
            {
                object toReturn = this.animations[id];
                return (T) toReturn;
            }
            if (typeof(T) == typeof(AnimationV2))
            {
                object toReturn = this.animationV2s[id];
                return (T) toReturn;
            }
            if (typeof(T) == typeof(Effect))
            {
                object toReturn = this.shaders[id];
                return (T) toReturn;
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
