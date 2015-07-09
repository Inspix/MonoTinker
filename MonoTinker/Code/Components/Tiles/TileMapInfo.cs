using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MonoTinker.Code.Components.Tiles
{
    public class TileMapInfo
    {
        public int height;
        public int width;
        public int tileheight;
        public int nextobjectid;
        public int tilewidth;
        public float version;
        public string orientation;
        public string renderorder;
        public JObject properties;
        public List<TileLayer> layers;
        public List<TileSet> tilesets;
    }

    public class TileSet
    {
        public int firstgid;
        public int imageheight;
        public int imagewidth;
        public int tileheight;
        public int tilewidth;
        public float spacing;
        public float margin;
        public string image;
        public string name;
        public JObject properties;
    }
}
