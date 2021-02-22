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
        List<int> _disabledInputs = new List<int>();
        bool _gamepadActive = false;
        public SceneInputs()
        {
            _gamePads = new GamepadState[GamePad.MaximumGamePadCount];
            for (var i = 0; i < GamePad.MaximumGamePadCount; i++)
            {
                _gamePads[i] = new GamepadState((PlayerIndex)i);
            }
        }
        public void disableInputTag(int tag)
        {
            if (_disabledInputs.Contains(tag)) return;

            _disabledInputs.Add(tag);
        }
        public void enableInputTag(int tag)
        {
            _disabledInputs.Remove(tag);
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
        public void Vibration(float strength, float time)
        {
            if (_gamepadActive)
            {
                _gamePads[0].SetVibration(strength, strength, time);
            }
        }
        public override void Update()
        {
            bool gamePadUsed = false;
            _keyboard.Update();
            for(var i = 0; i < _gamePads.Length; i++)
            {
                _gamePads[i].Update();
                gamePadUsed |= _gamePads[i].Used;
            }
            for(var i = 0; i < _inputs.Count; i++)
            {
                if (_disabledInputs.Contains(_inputs[i].Tag))
                {
                    _inputs[i].Reset();
                    continue;
                }

                _inputs[i].Update(this);
                if(_inputs[i].Pressed)
                {
                    if(gamePadUsed)
                    {
                        _gamepadActive = true;
                    }
                    else
                    {
                        _gamepadActive = false;
                    }
                }
            }
        }
    }
}
