namespace kuujoo.Pixel
{
    public class Input
    {
        public bool Down => IsDown();
        public bool Pressed => IsPressed();
        public bool Released => IsReleased();
        public bool Buffered => IsBuffered();

        IInputNode[] _nodes;
        float _buffer_timer = 0.0f;
        float _buffer_time = 0.0f;
        public Input(IInputNode[] nodes, float buffer_time)
        {
            Engine.Instance.Inputs.RegisterInput(this);
            _nodes = nodes;
            _buffer_time = buffer_time;
        }
        public void Update()
        {
            _buffer_timer = MathExt.Approach(_buffer_timer, 0.0f, Time.DeltaTime);
            bool down = false;
            for (var i = 0; i < _nodes.Length; i++)
            {
                if(_nodes[i].Pressed)
                {
                    _buffer_timer = _buffer_time;
                    down = true;
                }
                else if(_nodes[i].Down)
                {
                    down = true;
                }
            }
            if(!down)
            {
                _buffer_timer = 0.0f;
            }
        }
        bool IsDown()
        {
            for(var i = 0; i < _nodes.Length; i++)
            {
                if(_nodes[i].Down)
                {
                    return true;
                }
            }
            return false;
        }
        bool IsPressed()
        {
            for (var i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i].Pressed)
                {
                    return true;
                }
            }
            return false;
        }
        bool IsReleased()
        {
            for (var i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i].Released)
                {
                    return true;
                }
            }
            return false;
        }
        bool IsBuffered()
        {
            return _buffer_timer > 0.0f;
        }
    }
}
