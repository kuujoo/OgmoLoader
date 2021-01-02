using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class KeyboardData
    {
        KeyboardState _previousKeyboardState;
        KeyboardState _currentKeyboardState;
        public KeyboardData()
        {
            _previousKeyboardState = new KeyboardState();
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
