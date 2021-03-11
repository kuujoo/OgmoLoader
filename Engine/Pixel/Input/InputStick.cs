using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class InputStick
    {
        public Vector2 Direction => Vector2.Normalize(Value);
        public Vector2 Value { get; private set; }
        List<GamePadStick> _nodes = new List<GamePadStick>();
        bool _invertY = true;
        public InputStick SetStick(int _gamepad, Buttons button)
        {
            _nodes.Add(new GamePadStick(_gamepad, button));
            return this;
        }
        public void Update(IInputState state)
        {
            Vector2 value = Vector2.Zero;
            for(var i = 0; i< _nodes.Count; i++)
            {
                _nodes[i].Update(state);
                if (_invertY)
                {
                    var v = _nodes[i].Value;
                    Value = v * new Vector2(1.0f, -1.0f);
                }
                else
                {
                    Value = _nodes[i].Value;
                }
            }
        }
    }
}
