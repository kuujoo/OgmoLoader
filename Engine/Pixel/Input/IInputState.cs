using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public interface IInputState
    {
        public bool IsKeyPressed(Keys key);
        public bool IsKeyDown(Keys key);
        public bool IsKeyReleased(Keys key);
        public bool IsButtonPressed(int index, Buttons key);
        public bool IsButtonDown(int index, Buttons key);
        public bool IsButtonReleased(int index, Buttons key);
        public Vector2 GetLeftStickValue(int index);
        public Vector2 GetRightStickValue(int index);
    }
}
