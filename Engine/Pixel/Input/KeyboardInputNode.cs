using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class KeyboardInputNode : IInputNode
    {
        Keys _key;
        public KeyboardInputNode(Keys key)
        {
            _key = key;
        }
        public bool Down => Engine.Instance.Inputs.Keyboard.IsKeyDown(_key);
        public bool Pressed => Engine.Instance.Inputs.Keyboard.IsKeyPressed(_key);
        public bool Released => Engine.Instance.Inputs.Keyboard.IsKeyReleased(_key);
    }

    public class GamepadInputNode : IInputNode
    {
        Buttons _button;
        int _gamepadIndex;
        public GamepadInputNode(int _gamepadIndex, Buttons button)
        {
            _button = button;
        }
        public bool Down => Engine.Instance.Inputs.GamePads[_gamepadIndex].IsButtonDown(_button);

        public bool Pressed => Engine.Instance.Inputs.GamePads[_gamepadIndex].IsButtonPressed(_button);

        public bool Released => Engine.Instance.Inputs.GamePads[_gamepadIndex].IsButtonReleased(_button);
    }
}
