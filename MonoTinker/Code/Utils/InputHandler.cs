using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoTinker.Code.Utils
{
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

        public static void Update(GameWindow win)
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState(win);
        }
    }
}
