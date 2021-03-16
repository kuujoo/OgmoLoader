using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class Test : Component, IRenderable, IUpdateable
    {
        public int Layer { get; set; }

        public int UpdateOrder { get; set; }

        Vector2 _speed;
        BoxCollider _collider;
        Color _color = Color.White;
        public override void Initialize()
        {
            base.Initialize();
            _speed = Random.Range(new Vector2(-100, -100), new Vector2(100, 100));
            _collider = Entity.AddComponent(Scene.Get<BoxCollider>());
            _collider.Set(0, 0, 8, 8);
            _collider.Mask = 1;
        }
        public void Update()
        {
            Entity.Transform.Translate((int)(_speed.X * Time.DeltaTime), (int)(_speed.Y * Time.DeltaTime));
            if (_speed.X > 0 && Entity.Transform.Position.X > 384) _speed.X *= -1;
            if (_speed.X < 0 && Entity.Transform.Position.X < 0) _speed.X *= -1;

            if (_speed.Y > 0 && Entity.Transform.Position.Y > 216) _speed.Y *= -1;
            if (_speed.Y < 0 && Entity.Transform.Position.Y < 0) _speed.Y *= -1;

            if (true || _collider.Check(1, Point.Zero) != null)
            {
                _color = Color.Red;
            }
            else
            {
                _color = Color.White;
            }
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }
        public void Render(Graphics graphics)
        {
            graphics.DrawRect(_collider.Bounds, _color);
        }
        public void DebugRender(Graphics graphics)
        {
        }
    }
    public class Game1 : Engine
    {
        public Game1() : base()
        {

        }
        protected override void Initialize()
        {
            base.Initialize();

            var room = new Scene(384, 216);
            var cameraEntity = room.CreateEntity();
            var camera = cameraEntity.AddComponent(room.Get<Camera>());
            camera.BackgroundColor = Color.Aquamarine;
            camera.SetSize(384, 216);
            camera.SetViewport(384, 216);
            cameraEntity.Transform.SetPosition(384 / 2, 216 / 2);
            room.AddCamera(camera);
            room.DebugRender = true;
            for(var i = 0; i< 1000; i++)
            {
                var e = room.CreateEntity();
                e.AddComponent(room.Get<Test>());
                e.Transform.SetPosition(Random.Range(0, 384), Random.Range(0, 216));
            }
            Scene = room;
        }
    }
}
