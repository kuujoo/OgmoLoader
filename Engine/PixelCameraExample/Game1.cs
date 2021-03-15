using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace kuujoo.Pixel
{
    public class RectangleRenderer : Component, IRenderable
    {
        public int Layer { get; set; }
        Color _color;
        public void Set(Color color)
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
        InputButton UpInput;
        InputButton DownInput;
        InputButton LeftInput;
        InputButton RightInput;
        InputButton SwitchCameraInput;

        LayerCamera _camera;
        LayerCamera _camera2;
        LayerCamera _currentCamera;
 
        public RectangleScene() : base(384, 216)
        {
        }
        void InitInputs()
        {
            var inputs = AddSceneComponent(new SceneInputs());
            LeftInput = inputs.CreateInputButton().SetKey(Keys.Left).SetButton(0, Buttons.DPadLeft);
            RightInput = inputs.CreateInputButton().SetKey(Keys.Right).SetButton(0, Buttons.DPadRight);
            UpInput = inputs.CreateInputButton().SetKey(Keys.Up).SetButton(0, Buttons.DPadUp);
            DownInput = inputs.CreateInputButton().SetKey(Keys.Down).SetButton(0, Buttons.DPadDown);
            SwitchCameraInput = inputs.CreateInputButton().SetKey(Keys.Space).SetButton(0, Buttons.A);
        }
        public override void Initialize()
        {
            base.Initialize();

            InitInputs();

            var cameraEntity = CreateEntity();
            _camera = cameraEntity.AddComponent(Get<LayerCamera>());
            _camera.SetSize(384, 216);
            _camera.SetViewport(384, 216);
            _camera.RenderLayer = 1;
            _camera.Priority = 0;


            var cameraEntity2 = CreateEntity();
            _camera2 = cameraEntity2.AddComponent(Get<LayerCamera>());
            _camera2.SetSize(384, 216);
            _camera2.SetViewport(384, 216);
            _camera2.RenderLayer = 2;
            _camera2.Priority = 0;

            AddCamera(_camera);
            AddCamera(_camera2);
            _currentCamera = _camera2;

            for(var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity();
                    var rect = e.AddComponent(Get<RectangleRenderer>());
                    rect.Layer = 1;
                    rect.Set(Color.White);
                    e.Transform.SetPosition(i * 12, j * 12);
                }
            }

            for (var i = 0; i < 384 / 12; i++)
            {
                for (var j = 0; j < 216 / 12; j++)
                {
                    var e = CreateEntity();
                    var rect = e.AddComponent(Get<RectangleRenderer>());
                    rect.Layer = 2;
                    rect.Set(Color.Red);
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
