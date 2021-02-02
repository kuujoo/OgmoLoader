﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class RectangleRenderer : Component, IRenderable
    {
        public int Layer { get; set; }
        Color _color;
        public RectangleRenderer(Color color)
        {
            _color = color;
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }
        public void Render(Graphics graphics)
        {
            graphics.DrawRect(new Rectangle(Entity.Transform.Position, new Point(10, 10)), _color);
        }
        public void DebugRender(Graphics graphics)
        {
        }
    }

    public class RectangleScene : Scene
    {
        Input UpInput;
        Input DownInput;
        Input LeftInput;
        Input RightInput;
        Input SwitchCameraInput;
        Input ZoomInInput;
        Input ZoomOutInput;

        Camera _camera;
        Camera _camera2;
        Camera _currentCamera;
 
        public RectangleScene() : base(384, 216)
        {
        }
        void InitInputs()
        {
            var inputs = AddSceneComponent(new SceneInputs());
            LeftInput = inputs.CreateInput().SetKey(Keys.Left).SetButton(0, Buttons.DPadLeft);
            RightInput = inputs.CreateInput().SetKey(Keys.Right).SetButton(0, Buttons.DPadRight);
            UpInput = inputs.CreateInput().SetKey(Keys.Up).SetButton(0, Buttons.DPadUp);
            DownInput = inputs.CreateInput().SetKey(Keys.Down).SetButton(0, Buttons.DPadDown);
            SwitchCameraInput = inputs.CreateInput().SetKey(Keys.Space).SetButton(0, Buttons.A);
            ZoomOutInput = inputs.CreateInput().SetKey(Keys.Subtract).SetButton(0, Buttons.LeftTrigger);
            ZoomInInput = inputs.CreateInput().SetKey(Keys.Add).SetButton(0, Buttons.RightTrigger);
        }
        public override void Initialize()
        {
            base.Initialize();

            InitInputs();

            var cameraEntity = CreateEntity();
            _camera = cameraEntity.AddComponent(new LayerCamera(384, 216)
            {
                RenderLayer= 1,
                Priority = 0
            });

            var cameraEntity2 = CreateEntity();
            _camera2 = cameraEntity2.AddComponent(new LayerCamera(384, 216)
            {
                RenderLayer = 2,
                Priority = 1
            });
            _camera.SetCenterOrigin();
            _camera2.SetCenterOrigin();

            AddCamera(_camera);
            AddCamera(_camera2);
            _currentCamera = _camera2;
            for(var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity();
                    e.AddComponent(new RectangleRenderer(Color.White)).Layer = 1;
                    e.Transform.SetPosition(i * 12, j * 12);
                }
            }

            for (var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity();
                    e.AddComponent(new RectangleRenderer(Color.Red)).Layer = 2;
                    e.Transform.SetPosition(i * 12, j * 12);
                }
            }
        }
        public override void Update()
        {
            if(LeftInput.Down)
            {
                _currentCamera.Entity.Transform.Translate((int)(-60 * Time.DeltaTime), 0);
            }
            if (RightInput.Down)
            {
                _currentCamera.Entity.Transform.Translate((int)(60 * Time.DeltaTime), 0);
            }
            if (UpInput.Down)
            {
                _currentCamera.Entity.Transform.Translate(0, (int)(-60 * Time.DeltaTime));
            }
            if (DownInput.Down)
            {
                _currentCamera.Entity.Transform.Translate(0, (int)(60 * Time.DeltaTime));
            }
            if (ZoomInInput.Down)
            {
                _currentCamera.Zoom = MathHelper.Clamp(_currentCamera.Zoom + 0.5f * Time.DeltaTime, 1.0f, 3.0f);
            }
            if (ZoomOutInput.Down)
            {
                _currentCamera.Zoom = MathHelper.Clamp(_currentCamera.Zoom - 0.5f * Time.DeltaTime, 1.0f, 3.0f);
            }
            if (SwitchCameraInput.Pressed)
            {
                if (_currentCamera == _camera)
                {
                    _camera.Priority = 0;
                    _camera2.Priority = 1;
                    _currentCamera = _camera2;
                }
                else if (_currentCamera == _camera2)
                {
                    _camera.Priority = 1;
                    _camera2.Priority = 0;
                    _currentCamera = _camera;
                }
            }
            base.Update();
        }
    }

    public class Game1 : Engine
    {        
        public Game1()
        {

        }
        protected override void Initialize()
        {
            base.Initialize();
            Screen.SetSize(1920, 1080);
            Scene = new RectangleScene();
        }
    }
}
