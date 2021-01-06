using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class Input
    {
        public bool Down { get; private set; }
        public bool Pressed { get; private set; }
        public bool Released { get; private set; }
        public bool Buffered => _buffer_timer > 0.0f;
        float _buffer_timer = 0.0f;
        float _buffer_time = 0.0f;
        List<IInputNode> _nodes = new List<IInputNode>();
        public Input(float buffer_time)
        {
            _buffer_time = buffer_time;
        }
        public Input SetKey(Keys key)
        {
            _nodes.Add(new KeyboardInput(key));
            return this;
        }
        public Input SetButton(int _gamepad, Buttons button)
        {
            _nodes.Add(new GamepadInput(0, button));
            return this;
        }
        public void ConsumeBuffer()
        {
            _buffer_timer = 0.0f;
        }
        public void Update(IInputState state)
        {
            _buffer_timer = MathExt.Approach(_buffer_timer, 0.0f, Time.DeltaTime);
            Pressed = false;
            Down = false;
            Released = false;
            for (var i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Update(state);
                Pressed = _nodes[i].Pressed || Pressed;
                Down = _nodes[i].Down || Down;
                Released = _nodes[i].Released || Released;
            }
            if(Pressed)
            {
                _buffer_timer = _buffer_time;
            } else if (!Down)
            {
                _buffer_timer = 0.0f;
            }
        }
    }
}
