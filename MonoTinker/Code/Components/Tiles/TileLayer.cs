using System.Collections.Generic;

namespace MonoTinker.Code.Components.Tiles
{
    public class TileLayer
    {
        public int[] data;
        public int height;
        public string name;
        public float opacity;
        public string type;
        public bool visible;
        public int width;
        public int x;
        public int y;
        public string draworder;
        public List<TileMapObject> objects;
    }
}
