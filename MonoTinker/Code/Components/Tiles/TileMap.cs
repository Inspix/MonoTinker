using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;
using MonoTinker.Code.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTinker.Code.Components.Tiles
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Interfaces;

    public class TileMap
    {
        private List<StaticTile> staticTiles;
        private List<LightTile> lightSources;
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
        public List<LightTile> LightTiles
        {
            get
            {
                if (lightSources == null)
                    lightSources = new List<LightTile>();
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
            if (t == typeof(LightTile))
            {
                LightTiles.Add((LightTile)item);
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
                                    /*StaticTiles.Add(new StaticTile(atlas[tiles[col] - 1].Texture,
                                        atlas[tiles[col] - 1].Source, new Vector2(x, y)));*/
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
                                    /*CollisionTiles.Add(new CollisionTile(atlas[tiles[col] - 1].Texture, atlas[tiles[col] - 1].Source, new Vector2(x, y)));*/
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
                                   /* LightTiles.Add(new LightTile(atlas[tiles[col] - 1].Texture, atlas[tiles[col] - 1].Source,
                                        new Vector2(x, y),LightSimpleEffect.None));*/
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

        public void LoadFromTiledJsonFile(ref SpriteAtlas atlas, ContentManager content, string filename)
        {
            string input = File.ReadAllText(ScreenManager.Content.RootDirectory + filename);
            TileMapInfo map = JsonConvert.DeserializeObject<TileMapInfo>(input);

            foreach (var tileset in map.tilesets)
            {
                atlas.PopulateFromSpriteSheet(content.Load<Texture2D>(tileset.image.Replace(".png","")),
                    new Vector2(tileset.tilewidth, tileset.tileheight),tileset.name);
            }

            foreach (var tileLayer in map.layers)
            {
                switch (tileLayer.type)
                {
                    case "tilelayer":
                        LoadTileLayer(tileLayer, ref atlas, map.tilewidth, map.tileheight);
                        break;
                    case "objectgroup":
                        LoadObjectLayer(tileLayer);
                        break;
                }
            }

            totalHeight = map.height * map.tileheight;
            totalWidht = map.width * map.tilewidth;
        }


        private void LoadTileLayer(TileLayer layer, ref SpriteAtlas atlas, int tileW, int tileH)
        {
            int index = 0;
            int x = 0;
            int y = 0;
            for (int i = 0; i < layer.height; i++)
            {
                for (int j = 0; j < layer.width; j++)
                {
                    if (layer.data[index] == 0)
                    {
                        x += tileW;
                        index++;
                        continue;
                    }
                    this.StaticTiles.Add(new StaticTile(atlas[layer.data[index]-1],new Vector2(x,y)));
                    index++;
                    x += tileW;
                }
                x = 0;
                y += tileH;
            }
        }

        private void LoadObjectLayer(TileLayer layer)
        {
            foreach (var tileMapObject in layer.objects)
            {
                LightTile tile = new LightTile(AssetManager.Instance.Get<Sprite>("lighting"),new Vector2(tileMapObject.x,tileMapObject.y));
                tile.Origin = Origin.Center;
                LightTiles.Add(tile);
            }
        }
    }
}
