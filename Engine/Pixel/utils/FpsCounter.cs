using System;

namespace kuujoo.Pixel
{
    public class FpsCounter
    {
        public int Fps { get; private set; }
        int _frame = 0;
        TimeSpan _elapsed;
        public bool Update(TimeSpan elapsedTime)
        {
            _frame++;
            _elapsed += elapsedTime;
            if (_elapsed >= TimeSpan.FromSeconds(1))
            {
                Fps = _frame;
                _frame = 0;
                _elapsed -= TimeSpan.FromSeconds(1);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
