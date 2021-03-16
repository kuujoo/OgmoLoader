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
    public enum ScalePolicy
    {
        None,
        Min,
        Max,
        Stretch
    }

    public class Camera : Component, IComparable<Camera>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Effect Effect { get; set; }
        public List<int> ExcludeLayers { get; private set; }
        public List<int> IncludeLayers { get; private set; }
        public Color BackgroundColor { get; set; }
        public SamplerState SamplerState { get; set; }
        public BlendState BlendState { get; set; }
        public CameraType Type { get; set; }
        public int Priority { get; set; }
        public Surface Surface { get; set; }
        public Viewport Viewport { get; private set; }
        public ScalePolicy ScalePolicy { get; private set; }
        public Vector2 Position
        {
            get
            {
                return Entity.Transform.Position.ToVector2();
            }
            set
            {
                Entity.Transform.SetPosition((int)value.X, (int)value.Y);
                _updateMatrices = true;
            }
        }
        public Matrix Matrix
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
        public Rectangle Bounds
        {
            get
            {
                if (_updateMatrices)
                {
                    UpdateMatrices();
                    _updateMatrices = false;
                }
                return _bounds;
            }
        }
        private Rectangle _bounds;
        private Matrix _matrix = Matrix.Identity;
        private Matrix _inverseMatrix = Matrix.Identity;
        Vector2 _origin = Vector2.Zero;
        bool _updateMatrices = true;
        Vector2 _scale = Vector2.One;
        public Camera()
        {
            Priority = 0;
            BackgroundColor = Color.Black;
            ExcludeLayers = new List<int>();
            IncludeLayers = new List<int>();
            SamplerState = SamplerState.PointClamp;
            BlendState = BlendState.AlphaBlend;
            ScalePolicy = ScalePolicy.Max;
        }
        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            _updateMatrices = true;
        }
        public override void TransformChanged(Transform transform)
        {
            _updateMatrices = true;
        }
        public void SetViewport(int width, int height)
        {
            var port = default(Viewport);
            port.Width = width;
            port.Height = height;
            Viewport = port;
            _updateMatrices = true;
        }
        void UpdateMatrices()
        {
            _origin = new Vector2(Viewport.Width / 2.0f, Viewport.Height / 2.0f);
            if (ScalePolicy == ScalePolicy.Max)
            {
                var v = Math.Max(Viewport.Width / Width, Viewport.Height / Height);
                _scale = Vector2.One * v;
            }
            else if(ScalePolicy == ScalePolicy.Min)
            {
                var v = Math.Min(Viewport.Width / Width, Viewport.Height / Height);
                _scale = Vector2.One * v;
            }
            else if(ScalePolicy == ScalePolicy.Stretch)
            {
                var w = (float)Viewport.Width / Width;
                var h = (float)Viewport.Height / Height;
                _scale = new Vector2(w, h);
            }
            else
            {
                _scale = Vector2.One;
            }
            var translation = Matrix.CreateTranslation(new Vector3(-(int)Math.Floor(Position.X), -(int)Math.Floor(Position.Y), 0f));
            var scale = Matrix.CreateScale(_scale.X, _scale.Y, 1.0f);
            var origin_translation = Matrix.CreateTranslation(new Vector3((int)_origin.X, (int)_origin.Y, 0.0f));
            _matrix = Matrix.Identity * translation * scale * origin_translation;
            _inverseMatrix = Matrix.Invert(_matrix);

            var tl = Vector2.Transform(Vector2.Zero, _inverseMatrix);
            var br = Vector2.Transform(new Vector2(Viewport.Width, Viewport.Height), _inverseMatrix);
            var size = br - tl;
            _bounds = new Rectangle(Vector2.Transform(Vector2.Zero, _inverseMatrix).ToPoint(), size.ToPoint());
        }
        public int CompareTo([AllowNull] Camera other)
        {
            return Priority.CompareTo(other.Priority);
        }
        public void BeginRender(Graphics graphics)
        {
            graphics.PushSurface(Surface);
            graphics.PushMatrix(Matrix);
            graphics.PushEffect(Effect);
            graphics.PushSamplerState(SamplerState);
            graphics.PushBlendState(BlendState);
            graphics.Begin();
            if (Type == CameraType.Base)
            {
                graphics.Device.Clear(BackgroundColor);
            }
            graphics.Camera = this;
            if (Effect != null)
            {
                Effect.CurrentTechnique.Passes[0].Apply();
            }
        }
        public virtual void Render(Graphics graphics, IRenderable renderable)
        {
            if (ExcludeLayers.Contains(renderable.Layer)) return;
            if (IncludeLayers.Count > 0 && !IncludeLayers.Contains(renderable.Layer)) return;

            if (renderable.IsVisibleFromCamera(this)) {
                renderable.Render(graphics);
            }
        }
        public void DebugRender(Graphics graphics, IRenderable renderable)
        {
            if (ExcludeLayers.Contains(renderable.Layer)) return;
            if (IncludeLayers.Count > 0 && !IncludeLayers.Contains(renderable.Layer)) return;

            if (renderable.IsVisibleFromCamera(this))
            {
                renderable.DebugRender(graphics);
            }
        }
        public void EndRender(Graphics graphics)
        {
            graphics.End();
            graphics.PopBLendState();
            graphics.PopSamplerState();
            graphics.PopEffect(Effect);
            graphics.PopMatrix(Matrix);
            graphics.PopSurface(Surface);
        }
        public Vector2 WorldToScreen(Vector2 p)
        {
            return Vector2.Transform(p, Matrix);
        }
        public Vector2 ScreenToWorld(Vector2 p)
        {
            return Vector2.Transform(p, InverseMatrix);
        }
    }
}
