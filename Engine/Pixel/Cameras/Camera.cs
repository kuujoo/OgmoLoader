using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace kuujoo.Pixel
{
    public enum CameraType
    {
        Base,
        Overlay
    }
    public class Camera : IComparable<Camera>
    {
        public bool Enabled { get; set; }
        public Color BackgroundColor { get; set; }
        public CameraType Type { get; set; }
        public int Priority { get; set; }
        public Surface Surface { get; set; }
        public Viewport Viewport { get; private set; }
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _updateMatrices = true;
            }
        }
        public Matrix Matrix
        {
            get
            {
                if(_updateMatrices)
                {
                    UpdateMatrices();
                    _updateMatrices = true;
                }
                return _matrix;
            }
        }
        public Matrix InverseMatrix
        {
            get
            {
                if (_updateMatrices)
                {
                    UpdateMatrices();
                    _updateMatrices = false;
                }
                return _matrix;
            }
        }
        public float Zoom
        { 
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                _updateMatrices = true;
            }
        }
        private Matrix _matrix = Matrix.Identity;
        private Matrix _inverseMatrix = Matrix.Identity;
        Vector2 _position = Vector2.Zero;
        Vector2 _origin = Vector2.Zero;
        bool _updateMatrices = true;
        float _zoom = 1.0f;
        public Camera(int width, int height, CameraType type = CameraType.Base, int priority = 0)
        {
            Enabled = true;
            var port = default(Viewport);
            port.Width = width;
            port.Height = height;
            Viewport = port;
            Type = type;
            Priority = Priority;
            BackgroundColor = Color.Black;
        }
        public void Translate(float x, float y)
        {
            _position.X += x;
            _position.Y += y;
            _updateMatrices = true;
        }
        void UpdateMatrices()
        {
            var translation = Matrix.CreateTranslation(new Vector3(-(int)Math.Floor(Position.X), -(int)Math.Floor(Position.Y), 0f));
            var scale = Matrix.CreateScale(_zoom, _zoom, 1.0f);
            var origin_translation = Matrix.CreateTranslation(new Vector3((int)_origin.X, (int)_origin.Y, 0.0f));
            _matrix = Matrix.Identity * translation * scale * origin_translation;
            _inverseMatrix = Matrix.Invert(_matrix);
        }
        public void SetCenterOrigin()
        {
            _origin = new Vector2(Viewport.Width / 2.0f, Viewport.Height / 2.0f);
            _updateMatrices = true;
        }
        public virtual void Render(Scene scene)
        {
            var gfx = Engine.Instance.Graphics;
            gfx.Begin(this);
            for (var i = 0; i < scene.Entities.Count; i++)
            {
                scene.Entities[i].Render(gfx);
            }
            gfx.End();
        }

        public int CompareTo([AllowNull] Camera other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}
