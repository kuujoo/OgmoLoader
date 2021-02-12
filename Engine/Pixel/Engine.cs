using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Diagnostics;

namespace kuujoo.Pixel
{

    public class Engine : Game
    {
        public Screen Screen { get; private set; }
        public Color ClearColor = Color.Black;
        public static Engine Instance => _instance;
        public Graphics Graphics { get; set; }
        public Scene Scene { get { return _scene; } set { _nextScene = value; } }
        public bool PauseOnFocusLost { get; set; }
        FpsCounter _frameCounter = new FpsCounter();
        internal static Engine _instance;
        bool _resizing = false;
        string _windowTitle;
        Scene _scene;
        Scene _nextScene;
        CoroutineManager _coroutineManager;
        int _width;
        int _height;
        public Engine(int width = 1920, int height = 1080, bool fullscreen = false, string title = "Pixel", string contentDirectory = "Content")
        {
            _windowTitle = title;
            _instance = this;
            var graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = fullscreen,
                SynchronizeWithVerticalRetrace = true,
                PreferHalfPixelOffset = true,
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height
            };
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            graphics.DeviceReset += OnGraphicsDeviceReset;
            graphics.DeviceCreated += OnGraphicsCreate;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            PauseOnFocusLost = true;
            Window.AllowUserResizing = true;
            Graphics = new Graphics(graphics);
            Screen = new Screen(graphics);
            _coroutineManager = new CoroutineManager();
            //  Monogame 3.8 bug workaround: Window size does not change to "preferred backbuffer size" if changed inside Game()-constructor. Do change inside Engine::Initialize()
            _width = width;
            _height = height;
        }
        public ICoroutine StartCoroutine(IEnumerator enumerator)
        {
            return _coroutineManager.StartCoroutine(enumerator);
        }
        protected override void Initialize()
        {
            base.Initialize();
            Screen.SetSize(_width, _height);
            Window.ClientSizeChanged += OnClientSizeChanged; 
        }
        [Conditional("DEBUG")]
        private void StartDebugUpdate(GameTime gameTime)
        {
        }
        [Conditional("DEBUG")]
        private void EndDebugUpdate(GameTime gameTime)
        {
        }
        protected override void Update(GameTime gameTime)
        {    
            if (PauseOnFocusLost && !IsActive)
            {
                SuppressDraw();
                return;
            }
            StartDebugUpdate(gameTime);
            {
                Time.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                _coroutineManager.Update();
                if (_nextScene != null)
                {
                    if(_scene != null)
                    {
                        _scene.EndScene();
                        _scene.Dispose();
                    }
                    _scene = _nextScene;
                    if(_scene != null)
                    {
                        GC.Collect();
                    }
                    _nextScene = null;
                }
                if(_scene != null)
                {
                    _scene.Update();
                }
                base.Update(gameTime);
            }
            EndDebugUpdate(gameTime);
         
        }
        Stopwatch _stopwatch = new Stopwatch();
        [Conditional("DEBUG")]
        private void StartDebugDraw(GameTime gametime)
        {
            _stopwatch.Start();
        }
        [Conditional("DEBUG")]
        private void EndDebugDraw(GameTime gameTime)
        {
            if (_frameCounter.Update(gameTime.ElapsedGameTime))
            {
                
                Window.Title = $"{_windowTitle} - {_frameCounter.Fps} fps";
            }
            var elapsed = _stopwatch.Elapsed;
           // Console.WriteLine(elapsed.TotalMilliseconds);
            _stopwatch.Reset();
        }
        protected override void Draw(GameTime gameTime)
        {       
            if (PauseOnFocusLost && !IsActive) return;
            StartDebugDraw(gameTime);
            {
                if(_scene != null)
                {
                    _scene.Render();
                }
                base.Draw(gameTime);
            }
            EndDebugDraw(gameTime);
         
        }
        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }
        private void OnGraphicsDeviceReset(object sender, EventArgs e)
        {
            if (_scene != null)
            {
                _scene.OnGraphicsDeviceReset();
            }
            if (_nextScene != null)
            {
                _scene.OnGraphicsDeviceReset();
            }
        }
        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0 && !_resizing)
            {
                _resizing = true;
                Screen.SetSize(Window.ClientBounds.Width, Window.ClientBounds.Height);
                _resizing = false;
            }
        }
        private void OnGraphicsCreate(object sender, EventArgs e)
        {
        }
    }
}
