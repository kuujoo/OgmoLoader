using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class KeyboardState
    {
        Microsoft.Xna.Framework.Input.KeyboardState _previousKeyboardState;
        Microsoft.Xna.Framework.Input.KeyboardState _currentKeyboardState;
        public KeyboardState()
        {
            _previousKeyboardState = new Microsoft.Xna.Framework.Input.KeyboardState();
            _currentKeyboardState = Keyboard.GetState();
        }
        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }
        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }
        public bool IsKeyReleased(Keys key)
        {
            return !_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);
        }
        public void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }
    }
}
