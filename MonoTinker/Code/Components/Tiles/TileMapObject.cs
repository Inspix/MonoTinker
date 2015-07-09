using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Newtonsoft.Json.Linq;

namespace MonoTinker.Code.Components.Tiles
{
    public class TileMapObject
    {
        public bool ellipse;
        public float height;
        public int id;
        public string name;
        public JObject properties;
        public float rotation;
        public string type;
        public bool visible;
        public float width;
        public float x;
        public float y;
    }
}
