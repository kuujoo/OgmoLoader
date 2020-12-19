using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public struct SubpixelVector2
    {
        SubpixelFloat _x;
        SubpixelFloat _y;
        public Vector2 Update(Vector2 amount)
        {
            return new Vector2(_x.Update(amount.X), _y.Update(amount.Y));
        }       
        public void Reset()
        {
            _x.Reset();
            _y.Reset();
        }
    }
}
