﻿using Microsoft.Xna.Framework;
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
            _position.X = x;
            _position.Y = y;
            Changed?.Invoke(this);
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
    }
}