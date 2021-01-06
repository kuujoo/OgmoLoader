using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class KeyboardInput : IInputNode
    {
        public bool Down { get; private set; }
        public bool Pressed { get; private set; }
        public bool Released { get; private set; }
        Keys _key;
        public KeyboardInput(Keys key)
        {
            _key = key;
        }
        public void Update(IInputState state)
        {
            Down = state.IsKeyDown(_key);
            Pressed = state.IsKeyPressed(_key);
            Released = state.IsKeyReleased(_key);
        }
    }
}
