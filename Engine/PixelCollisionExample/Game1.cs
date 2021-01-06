using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class Test : Component
    {
        Vector2 _speed;
        BoxCollider _collider;
        Color _color = Color.White;
        public override void Initialize()
        {
            base.Initialize();
            _speed = Random.Range(new Vector2(-100, -100), new Vector2(100, 100));
            Entity.AddComponent(new BoxCollider(0, 0, 32, 32));
            _collider = Entity.GetComponent<BoxCollider>();
            _collider.Mask = CollisionMask.Solid;
        }
        public override void Update()
        {
            Entity.Transform.Translate((int)(_speed.X * Time.DeltaTime), (int)(_speed.Y * Time.DeltaTime));
            if (_speed.X > 0 && Entity.Transform.Position.X > 384) _speed.X *= -1;
            if (_speed.X < 0 && Entity.Transform.Position.X < 0) _speed.X *= -1;

            if (_speed.Y > 0 && Entity.Transform.Position.Y > 216) _speed.Y*= -1;
            if (_speed.Y < 0 && Entity.Transform.Position.Y < 0) _speed.Y *= -1;

            if(_collider.Check( 1<<0, Entity.Transform.Position) != null)
            {
                _color = Color.Red;
            }
            else
            {
                _color = Color.White;
            }
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            graphics.DrawRect(_collider.Bounds, _color);
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
            room.AddCamera(new Camera(384, 216)
            {
                BackgroundColor = Color.Aquamarine
            });
            for(var i = 0; i< 10; i++)
            {
                var e = room.CreateEntity(0);
                e.AddComponent(new Test());
                e.Transform.SetPosition(Random.Range(0, 384), Random.Range(0, 216));
            }
            Scene = room;
        }
    }
}
