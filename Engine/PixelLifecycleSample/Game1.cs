using System;

namespace kuujoo.Pixel
{
    public class LifeCyclePrinter : Component
    {
        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("Component initialized");
        }
        public override void CleanUp()
        {
            base.CleanUp();
            Console.WriteLine("Component Cleanup");
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
            var entity = room.CreateEntity();
            entity.AddComponent(new LifeCyclePrinter()); // Componoent::Initialize
            room.DestroyEntity(entity); // Component::Destroy, Component::Cleanup
            Scene = room;
        }
    }
}
