using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components;

namespace MonoTinker.Code
{
    class Tile
    {
        private Sprite sprite;
        private Transform transform;
        public BoxCollider BoxCollider;

        public Tile(Sprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            transform = new Transform(position);
            this.BoxCollider = new BoxCollider(position,sprite.Size);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite.Texture,transform.Position,sprite.Source,Color.White);
        }
    }
}
