using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace kuujoo.Pixel
{

    public class SceneInputs : SceneComponent, IInputState
    {
        List<Input> _inputs = new List<Input>();
        KeyboardState _keyboard = new KeyboardState();
        GamepadState[] _gamePads;
        public SceneInputs()
        {
            _gamePads = new GamepadState[GamePad.MaximumGamePadCount];
            for (var i = 0; i < GamePad.MaximumGamePadCount; i++)
            {
                _gamePads[i] = new GamepadState((PlayerIndex)i);
            }
        }
        public Input CreateInput(float buffer = 0.0f)
        {
            var input = new Input(buffer);
            _inputs.Add(input);
            return input;
        }
        public bool IsButtonDown(int index, Buttons key)
        {
            return _gamePads[index].IsButtonDown(key);
        }
        public bool IsButtonPressed(int index, Buttons key)
        {
            return _gamePads[index].IsButtonPressed(key);
        }
        public bool IsButtonReleased(int index, Buttons key)
        {
            return _gamePads[index].IsButtonReleased(key);
        }
        public bool IsKeyDown(Keys key)
        {
            return _keyboard.IsKeyDown(key);
        }
        public bool IsKeyPressed(Keys key)
        {
            return _keyboard.IsKeyPressed(key);
        }
        public bool IsKeyReleased(Keys key)
        {
            return _keyboard.IsKeyReleased(key);
        }
        public override void Update()
        {
            _keyboard.Update();
            for(var i = 0; i < _gamePads.Length; i++)
            {
                _gamePads[i].Update();
            }
            for(var i = 0; i < _inputs.Count; i++)
            {
                _inputs[i].Update(this);
            }
        }
    }
}
