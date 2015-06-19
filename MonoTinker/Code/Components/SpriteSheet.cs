using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Utils;

namespace MonoTinker.Code.Components
{
    public class SpriteSheet
    {
        private Texture2D spriteSheet;
        private Dictionary<string, Sprite> spritesDictionary;
        private readonly string baseName;

        public SpriteSheet(Texture2D texture, Vector2 size, string baseName = "")
        {
            this.spriteSheet = texture;
            this.baseName = baseName;
            this.spritesDictionary = new Dictionary<string, Sprite>();
            this.CreateSprites(size);
        }

        public SpriteSheet(Texture2D texture, Vector2 size, int missing, string baseName = "")
        {
            this.spriteSheet = texture;
            this.baseName = baseName;
            this.spritesDictionary = new Dictionary<string, Sprite>();
            this.CreateSprites(size, missing);
        }

        private void CreateSprites(Vector2 sizez,int missing = 0)
        {
            Point size = sizez.ToPoint();
            int perRow = spriteSheet.Width/size.X;
            int perCol = spriteSheet.Height/size.Y;
            int total = (perCol*perRow) - missing;
            int index = 0;
            for (int y = 0; y < perCol; y++)
            {
                for (int x = 0; x < perRow; x++)
                {
                    spritesDictionary.Add(baseName+index,new Sprite(spriteSheet,new Rectangle(x * size.X,y * size.Y ,size.X,size.Y)));
                    index++;
                    if (index  >= total)
                    {
                        break;
                    }
                }
            }
        }

        public Texture2D Texture
        {
            get { return this.spriteSheet; }
        }

        public int Count
        {
            get { return this.spritesDictionary.Count; }
        }

        public Sprite this[string key]
        {
            get
            {
                try
                {
                    return this.spritesDictionary[key];
                }
                catch (Exception)
                {
                    Debug.Error("Invalid sprite sheet key: {0}", key);
                }
                return null;
            }
        }

        public Sprite this[int key]
        {
            get
            {
                try
                {
                    return this.spritesDictionary[baseName+key];
                }
                catch (Exception)
                {
                    Debug.Error("Invalid sprite sheet index: {0}", key);
                }
                return null;
            }
        }

    }
}
