
namespace MonoTinker.Code.Components
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using Utils;


    public class SpriteSheet
    {
        private Texture2D spriteSheet;
        private SpriteAtlas atlas;
        private readonly string baseName;

        public SpriteSheet(Texture2D texture, Vector2 size, string baseName = "")
        {
            this.spriteSheet = texture;
            this.baseName = baseName;
            this.atlas = new SpriteAtlas();
            this.CreateSprites(size);

        }

        public SpriteSheet(Texture2D texture, Vector2 size, int missing, string baseName = "")
        {
            this.spriteSheet = texture;
            this.baseName = baseName;
            this.atlas = new SpriteAtlas();
            this.CreateSprites(size, missing);
        }

        private void CreateSprites(Vector2 framesize,int missing = 0, string baseName = "")
        {
            Point size = framesize.ToPoint();
            int perRow = spriteSheet.Width / size.X;
            int perCol = spriteSheet.Height / size.Y;
            int total = (perCol * perRow) - missing;
            int index = 0;
            for (int y = 0; y < perCol; y++)
            {
                for (int x = 0; x < perRow; x++)
                {
                    atlas.Add(baseName + index, new Sprite(spriteSheet, new Rectangle(x * size.X, y * size.Y, size.X, size.Y)));
                    index++;
                    if (index >= total)
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
            get { return this.atlas.Count; }
        }

        public Sprite this[string key]
        {
            get
            {
                try
                {
                    return atlas[key];
                }
                catch (Exception)
                {
                    Debug.Error("Invalid sprite sheet key: {0}", key);
                }
                return null;
            }
        }

        public Sprite this[int index]
        {
            get
            {
                try
                {
                    return (this.atlas[index]);
                }
                catch (Exception)
                {
                    Debug.Error("Invalid sprite sheet index: {0}", index);
                }
                return null;
            }
        }

    }
}
