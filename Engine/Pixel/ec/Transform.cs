using Microsoft.Xna.Framework;
using System;

namespace kuujoo.Pixel
{
    public class Transform
    {
        public Action<Transform> Changed;
        public Point Position => _position;
        Point _position;

        public Transform()
        {
            _position = Point.Zero;
        }
        public void SetPosition(int x, int y)
        {
            if (x != _position.X || y != _position.Y)
            {
                _position.X = x;
                _position.Y = y;
                Changed?.Invoke(this);
            }
        }
        public void SetPosition(Vector2 position)
        {
            SetPosition((int)position.X, (int)position.Y);
        }
        public void TranslateX(int amount)
        {
            _position.X += amount;
            Changed?.Invoke(this);
        }
        public void TranslateY(int amount)
        {
            _position.Y += amount;
            Changed?.Invoke(this);
        }
        public void Translate(int x, int y)
        {
            _position.X += x;
            _position.Y += y;
            Changed?.Invoke(this);
        }
        public void SetPositionY(int y)
        {
            _position.Y = y;
            Changed?.Invoke(this);
        }
        public void SetPositionX(int x)
        {
            _position.X = x;
            Changed?.Invoke(this);
        }
    }
}
