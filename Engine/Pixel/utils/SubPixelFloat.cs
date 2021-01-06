using System;

namespace kuujoo.Pixel
{
    public struct SubpixelFloat
    {
        float _remainder;
        public float Update(float amount)
        {
            _remainder += amount;
            var t = (int)Math.Truncate(_remainder);
            _remainder -= t;
            return t;
        }
        public void Reset()
        {
            _remainder = 0;
        }
    }
}