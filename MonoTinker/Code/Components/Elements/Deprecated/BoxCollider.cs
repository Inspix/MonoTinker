using Microsoft.Xna.Framework;

namespace MonoTinker.Code.Components.Elements
{
    public class BoxCollider
    {
        private Vector2 position;

        public float Height;
        public float Width;

        #region Properties
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public float PosX
        {
            get { return this.position.X; }
            set { this.position.X = value; }
        }

        public float PosY
        {
            get { return this.position.Y; }
            set { this.position.Y = value; }
        }

        public float Top
        {
            get { return this.PosY; }
        }

        public float Bottom
        {
            get { return this.PosY + Height; }
        }

        public float Left
        {
            get { return this.PosX; }
        }

        public float Right
        {
            get { return this.PosX + Width; }
        }

        #endregion

        public BoxCollider(float x, float y, float width, float height)
        {
            this.position = new Vector2(x,y);
            this.Width = width;
            this.Height = height;
        }

        public BoxCollider(Vector2 position, Vector2 size) : this(position.X, position.Y, size.X, size.Y) { }

    }
}
