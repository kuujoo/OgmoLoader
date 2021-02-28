using System;
using System.Diagnostics;

namespace kuujoo.Pixel
{
    public class PerformanceTimer
    {
        public float Average { get;  private set; }
        public float Max { get; private set; }
        public float Value { get; private set; }
        Stopwatch _stopWatch = new Stopwatch();
        int _samples = 0;
        float _accumulator = 0.0f;
        float _maxNow = 0.0f;
        public void Start()
        {
            _stopWatch.Restart();
            _maxNow = 0.0f;
        }
        public void Stop()
        {
            _stopWatch.Stop();
            var elapsedMs = _stopWatch.Elapsed.TotalMilliseconds;
            Value = (float)elapsedMs;
            _accumulator += (float)elapsedMs;
            _samples++;
            _maxNow = Math.Max(Value, _maxNow);
            if(_samples >= 100)
            {
                Average = _accumulator / 100.0f;
                _accumulator = 0.0f;
                _samples = 0;
                Max = _maxNow;
            }
        }

    }
}
