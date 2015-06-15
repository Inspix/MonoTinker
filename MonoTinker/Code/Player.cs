using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTinker.Code.Components;

namespace MonoTinker.Code
{
    class Player
    {
        public Transform Transform;
        public Sprite Sprite;
        public BoxCollider Collider;

        public Player(Sprite sprite)
        {
            Sprite = sprite;
            this.Transform = new Transform(Vector2.Zero);
            Collider = new BoxCollider(Transform.Position,sprite.Size);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                Move(new Vector2(-1f,0));
            }
            if (ks.IsKeyDown(Keys.D))
            {
                Move(new Vector2(1f,0));
            }
            if (ks.IsKeyDown(Keys.W))
            {
                Move(new Vector2(0, -1));
            }
            if (ks.IsKeyDown(Keys.S))
            {
                Move(new Vector2(0, 1));
            }
        }

        public void Move(Vector2 move)
        {
            this.Transform.Position += move;
            this.Collider.Position += move;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Texture,Transform.Position,Sprite.Source,Color.White);
        }
    }
}
