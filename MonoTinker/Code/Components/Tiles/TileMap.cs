using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Interfaces;

namespace MonoTinker.Code.Components.Tiles
{
    public class TileMap
    {
        private List<StaticTile> staticTiles;
        private List<Light> lightSources;
        private List<CollisionTile> collisionTiles;
        private int totalWidht;
        private int totalHeight;

        public TileMap()
        {
        }

        public int Widht
        {
            get { return this.totalWidht; }
        }

        public int Height
        {
            get { return this.totalHeight; }
        }

        public List<StaticTile> StaticTiles
        {
            get
            {
                if(staticTiles == null)
                    staticTiles = new List<StaticTile>();
                return this.staticTiles;
            }
        }
        public List<Light> LightTiles
        {
            get
            {
                if (lightSources == null)
                    lightSources = new List<Light>();
                return this.lightSources;
            }
        }
        public List<CollisionTile> CollisionTiles
        {
            get
            {
                if (collisionTiles == null)
                    collisionTiles = new List<CollisionTile>();
                return this.collisionTiles;
            }
        }

        public void Add(ISimpleDrawable item)
        {
            Type t = item.GetType();
            if (t == typeof(StaticTile))
            {
                StaticTiles.Add((StaticTile)item);
                return;
            }
            if (t == typeof(Light))
            {
                LightTiles.Add((Light)item);
                return;
            }
            if (t == typeof(CollisionTile))
            {
                CollisionTiles.Add((CollisionTile)item);
                return;
            }
            throw new InvalidOperationException("Tile map cant contain type: " + item.GetType());
        }

        public void Update(GameTime gameTime)
        {
            foreach (var lightTile in LightTiles)
            {
                lightTile.Update(gameTime);
            }
        }

        public void RemoveInactive()
        {
            for (int i = 0; i < LightTiles.Count; i++)
            {
                if (!LightTiles[i].Active)
                {
                    LightTiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void LoadFromTiledFile(ref SpriteAtlas atlas, string name)
        {
            int width = 0;
            int height = 0;
            int x = 0;
            int y = 0;
            using (StreamReader sm = new StreamReader(name))
            {
                string line = "";
                while ((line = sm.ReadLine()) != null)
                {
                    if (line.StartsWith("width"))
                    {

                        width = int.Parse(line.Substring(6));
                    }
                    if (line.StartsWith("height"))
                    {
                        height = int.Parse(line.Substring(7));
                    }
                    if (line.StartsWith("type=bg"))
                    {
                        sm.ReadLine();
                        for (int row = 0; row < height; row++)
                        {
                            int[] tiles = sm.ReadLine().Split(new char[] { ',', '\n' },StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                            for (int col = 0; col < width; col++)
                            {
                                if (tiles[col] != 0)
                                {
                                    StaticTiles.Add(new StaticTile(atlas[tiles[col] - 1].Texture,
                                        atlas[tiles[col] - 1].Source, new Vector2(x, y)));
                                }
                                x += 32;
                            }
                            x = 0;
                            y += 32;

                        }
                        x = 0;
                        y = 0;
                    }
                    if (line.StartsWith("type=object"))
                    {
                        sm.ReadLine();
                        for (int row = 0; row < height; row++)
                        {
                            int[] tiles = sm.ReadLine().Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                            for (int col = 0; col < width; col++)
                            {
                                if (tiles[col] != 0)
                                {
                                    CollisionTiles.Add(new CollisionTile(atlas[tiles[col] - 1].Texture, atlas[tiles[col] - 1].Source, new Vector2(x, y)));
                                }
                                x += 32;
                            }
                            x = 0;
                            y += 32;
                        }

                        x = 0;
                        y = 0;
                    }
                    if (line.StartsWith("type=light"))
                    {
                        sm.ReadLine();
                        for (int row = 0; row < height; row++)
                        {
                            int[] tiles = sm.ReadLine().Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                            for (int col = 0; col < width; col++)
                            {
                                if (tiles[col] != 0)
                                {
                                    LightTiles.Add(new Light(atlas[tiles[col] - 1].Texture, atlas[tiles[col] - 1].Source,
                                        new Vector2(x, y)));
                                }
                                x += 32;
                            }
                            x = 0;
                            y += 32;
                        }
                        x = 0;
                        y = 0;
                    }
                }
            }
            
            totalHeight = height*32;
            totalWidht = width*32;

        }
    }
}
