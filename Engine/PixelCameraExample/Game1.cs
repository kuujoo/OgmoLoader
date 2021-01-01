using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class RectangleRenderer : Component
    {
        Color _color;
        public RectangleRenderer(Color color)
        {
            _color = color;
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            graphics.DrawRect(new Rectangle(Entity.Position.ToPoint(), new Point(10, 10)), _color);
        }
    }

    public class RectangleScene : Scene
    {
        Camera _camera;
        Camera _camera2;
        Camera _currentCamera;
        bool _spaceReleased = true;
        public RectangleScene() : base(384, 216)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            _camera = new TagCamera(384, 216)
            {
                RenderTag = 1
            };

            _camera2 = new TagCamera(384, 216)
            {
                RenderTag = 2
            };

            AddCamera(_camera);
            AddCamera(_camera2);

            _camera.SetCenterOrigin();
            _camera.Priority = 0;
            _camera2.SetCenterOrigin();
            _camera2.Priority = 1;
            _currentCamera = _camera2;


            for(var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity();
                    e.AddComponent(new RectangleRenderer(Color.White));
                    e.Position = new Vector2(i * 12, j * 12);
                    e.Tag = 1;
                }
            }

            for (var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity();
                    e.AddComponent(new RectangleRenderer(Color.Red));
                    e.Position = new Vector2(i * 12, j * 12);
                    e.Tag = 2;
                }
            }
        }
        public override void Update()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _currentCamera.Translate(-60 * Time.DeltaTime, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _currentCamera.Translate(60 * Time.DeltaTime, 0.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _currentCamera.Translate(0.0f, -60 * Time.DeltaTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _currentCamera.Translate(0.0f, +60 * Time.DeltaTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Add))
            {
                _currentCamera.Zoom = MathHelper.Clamp(_camera.Zoom + 0.5f * Time.DeltaTime, 1.0f, 3.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
            {
                _currentCamera.Zoom = MathHelper.Clamp(_camera.Zoom - 0.5f * Time.DeltaTime, 1.0f, 3.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && _spaceReleased)
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
                _spaceReleased = false;
            }
            if(Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                _spaceReleased = true;
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
