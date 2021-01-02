using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class Inputs
    {   
        List<Input> _inputs = new List<Input>();
        public GamePadData[] GamePads { get; private set; }
        public KeyboardData Keyboard;
        public Inputs()
        {
            Keyboard = new KeyboardData();
            GamePads = new GamePadData[GamePad.MaximumGamePadCount];
            for(var i = 0; i < GamePad.MaximumGamePadCount; i++)
            {
                GamePads[i] = new GamePadData((PlayerIndex)i);
            }
        }
        public void Update()
        {
            Keyboard.Update();
            for (var i = 0; i < GamePads.Length; i++)
            {
                GamePads[i].Update();
            }
            for(var i = 0; i < _inputs.Count; i++)
            {
                _inputs[i].Update();
            }
        }
        public void RegisterInput(Input input)
        {
            _inputs.Add(input);
        }
    }
}
