using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.Elements;

namespace MonoTinker.Code.GameScreens
{
    public sealed class CharacterCreationScreen : Screen
    {
        public CharacterCreationScreen(IServiceProvider service) : base(service, "CharacterCreation")
        {
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            
        }

        public override void UnloadContent()
        {
            this.content.Unload();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
