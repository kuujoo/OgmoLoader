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
            LeftInput = new Input(new IInputNode[] { new KeyboardInputNode(Keys.Left), new GamepadInputNode(0, Buttons.DPadLeft) }, 0.0f);
            RightInput = new Input(new IInputNode[] { new KeyboardInputNode(Keys.Right), new GamepadInputNode(0, Buttons.DPadRight) }, 0.0f);
            UpInput = new Input(new IInputNode[] { new KeyboardInputNode(Keys.Up), new GamepadInputNode(0, Buttons.DPadUp) }, 0.0f);
            DownInput = new Input(new IInputNode[] { new KeyboardInputNode(Keys.Down), new GamepadInputNode(0, Buttons.DPadDown) }, 0.0f);
            SwitchCameraInput = new Input(new IInputNode[] { new KeyboardInputNode(Keys.Space), new GamepadInputNode(0, Buttons.A)}, 0.0f);
            ZoomOutInput = new Input(new IInputNode[] { new KeyboardInputNode(Keys.Subtract), new GamepadInputNode(0, Buttons.LeftTrigger) }, 0.0f);
            ZoomInInput = new Input(new IInputNode[] { new KeyboardInputNode(Keys.Add), new GamepadInputNode(0, Buttons.RightTrigger) }, 0.0f);
        }
        public override void Initialize()
        {
            base.Initialize();

            InitInputs();

            _camera = new TagCamera(384, 216)
            {
                RenderTag = 1,
                Priority = 0
            };

            _camera2 = new TagCamera(384, 216)
            {
                RenderTag = 2,
                Priority = 1
            };
            _camera.SetCenterOrigin();
            _camera2.SetCenterOrigin();

            AddCamera(_camera);
            AddCamera(_camera2);
            CreateEntityLayer(0, "entities");

            _currentCamera = _camera2;


            for(var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity(0);
                    e.AddComponent(new RectangleRenderer(Color.White));
                    e.Position = new Vector2(i * 12, j * 12);
                    e.Tag = 1;
                }
            }

            for (var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity(0);
                    e.AddComponent(new RectangleRenderer(Color.Red));
                    e.Position = new Vector2(i * 12, j * 12);
                    e.Tag = 2;
                }
            }
        }
        public override void Update()
        {
            if(LeftInput.Down)
            {
                _currentCamera.Translate(-60 * Time.DeltaTime, 0.0f);
            }
            if (RightInput.Down)
            {
                _currentCamera.Translate(60 * Time.DeltaTime, 0.0f);
            }
            if (UpInput.Down)
            {
                _currentCamera.Translate(0.0f, -60 * Time.DeltaTime);
            }
            if (DownInput.Down)
            {
                _currentCamera.Translate(0.0f, +60 * Time.DeltaTime);
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
