using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components
{
    class Sprite
    {
        private Texture2D _texture2D;
        public Vector2 Size { get; }
        public Vector2 Center { get; }
        
        public Sprite(Texture2D texture2D)
        {
            this._texture2D = texture2D;
            this.Size = new Vector2(_texture2D.Width,_texture2D.Height);
            this.Center = new Vector2(_texture2D.Width/2f, _texture2D.Height/2f);
        }

        public Sprite(string path, ContentManager content): this(content.Load<Texture2D>(path))
        {
        }
    }
}
