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
}
