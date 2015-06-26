
namespace MonoTinker.Code.Utils
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public static class InputHandler
    {
        private static KeyboardState currentKeyboardState;
        private static KeyboardState lastKeyboardState;

        private static MouseState currentMouseState;
        private static MouseState lastMouseState;

        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        public static bool Down(this Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool DownOnce(this Keys key)
        {
            return IsKeyDown(key);
        }

        public static bool DownLast(this Keys key)
        {
            return lastKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        public static bool DirectionDownOnce(string direction)
        {
            switch (direction.ToLower())
            {
                case "up":
                    return IsKeyDown(Keys.Up) || IsKeyDown(Keys.W);
                case "down":
                    return IsKeyDown(Keys.Down) || IsKeyDown(Keys.S);
                case "left":
                    return IsKeyDown(Keys.Left) || IsKeyDown(Keys.A);
                case "right":
                    return IsKeyDown(Keys.Right) || IsKeyDown(Keys.D);
                default:
                    return false;
            }
        }

        public static bool DirectionDown(string direction)
        {
            switch (direction.ToLower())
            {
                case "up":
                    return currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W);
                case "down":
                    return currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S);
                case "left":
                    return currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A);
                case "right":
                    return currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D);
                default:
                    return false;
            }
        }

        public static bool MouseDownOnce(string button)
        {
            switch (button)
            {
                case "left":
                    return currentMouseState.LeftButton == ButtonState.Pressed &&
                           lastMouseState.LeftButton == ButtonState.Released;
                case "right":
                    return currentMouseState.RightButton == ButtonState.Pressed &&
                           lastMouseState.RightButton == ButtonState.Released;
                case "middle":
                    return currentMouseState.MiddleButton == ButtonState.Pressed &&
                           lastMouseState.MiddleButton == ButtonState.Released;
            }
            return false;
        }

        public static bool MouseDown(string button)
        {
            switch (button)
            {
                case "left":
                    return currentMouseState.LeftButton == ButtonState.Pressed;
                case "right":
                    return currentMouseState.RightButton == ButtonState.Pressed;
                case "middle":
                    return currentMouseState.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        public static Vector2 MousePos()
        {
            return currentMouseState.Position.ToVector2();
        }

        public static void Update(GameWindow win)
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState(win);
        }
    }
}
