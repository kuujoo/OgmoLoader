using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class GamepadInput : IInputNode
    {
        Buttons _button;
        int _gamepadIndex;
        public GamepadInput(int gamepadIndex, Buttons button)
        {

            _button = button;
            _gamepadIndex = gamepadIndex;
        }
        public void Update(IInputState state)
        {
            Down = state.IsButtonDown(_gamepadIndex, _button);
            Pressed = state.IsButtonPressed(_gamepadIndex, _button);
            Released = state.IsButtonReleased(_gamepadIndex, _button);
        }
        public bool Down { get; private set; }
        public bool Pressed { get; private set; }
        public bool Released { get; private set; }
    }

    public class GamePadStick
    {
        public Vector2 Value { get; private set; }
        Buttons _button;
        int _gamepadIndex;
        public GamePadStick(int gamepadIndex, Buttons button)
        {
            _button = button;
            _gamepadIndex = gamepadIndex;
        }
        public void Update(IInputState state)
        {
            if(_button == Buttons.LeftStick)
            {
                Value = state.GetLeftStickValue(_gamepadIndex);
            }
            else if(_button == Buttons.RightStick)
            {
                Value = state.GetRightStickValue(_gamepadIndex);
            }
        }
    }
}
