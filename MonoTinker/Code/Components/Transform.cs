using Microsoft.Xna.Framework;

namespace MonoTinker.Code.Components
{
    public class Transform
    {

        #region Private fields
        private Vector2 _position; 
        #endregion

        #region Public fields

        public Vector2 Scale;
        public float Rotation; 

        #endregion

        #region Constructors

        public Transform()
        {
            _position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0.0f;
        }

        public Transform(Vector2 position)
        {
            this._position = position;
            Scale = Vector2.One;
            Rotation = 0.0f;
        }

        public Transform(Vector2 position, Vector2 scale)
        {
            this._position = position;
            Scale = scale;
            Rotation = 0.0f;
        }

        #endregion

        #region Properties

        public float PosX
        {
            get { return this._position.X; }
            set { this._position.X = value; }
        }

        public float PosY
        {
            get { return this._position.Y; }
            set { this._position.Y = value; }
        }

        #endregion
    }
}
