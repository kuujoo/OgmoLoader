using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public static class InputTag
    {
        public static int Default = 0;
        public static int Player = 1;
        public static int Menu = 2;
    }
    public interface IInputNode
    {
        public void Update(IInputState state);
        public bool Down { get; }
        public bool Pressed { get; }
        public bool Released { get; }
    }

    public class InputButton
    {
        public int Tag { get; set; }
        public bool Down { get; private set; }
        public bool Pressed { get; private set; }
        public bool Released { get; private set; }
        public bool Buffered => Pressed || _buffer_timer > 0.0f;
        float _buffer_timer = 0.0f;
        float _buffer_time = 0.0f;
        List<IInputNode> _nodes = new List<IInputNode>();
        public InputButton(float buffer_time)
        {
            _buffer_time = buffer_time;
        }
        public void Reset()
        {
            Pressed = false;
            Down = false;
            Released = false;
            _buffer_timer = 0.0f;
        }
        public InputButton SetKey(Keys key)
        {
            _nodes.Add(new KeyboardInput(key));
            return this;
        }
        public InputButton SetButton(int _gamepad, Buttons button)
        {
            _nodes.Add(new GamepadInput(0, button));
            return this;
        }
        public InputButton SetTag(int tag)
        {
            Tag = tag;
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
