using System;

namespace kuujoo.Pixel
{
    public class LifeCycleEntity : Entity
    {
        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("Entity initialized");
        }
        public override void Destroy()
        {
            base.Destroy();
            Console.WriteLine("Entity destroyed");
        }
    }

    public class LifeCycleComponent : Component
    {
        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("Component initialized");
        }
        public override void Destroy()
        {
            base.Destroy();
            Console.WriteLine("Component destroyed");
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
            var entity = room.CreateEntity<LifeCycleEntity>(); // Entity::Initialize
            entity.AddComponent(new LifeCycleComponent()); // Componoent::Initialize
            room.DestroyEntity(entity); // Entity::Destroy, Component::Destroy
            Scene = room;
        }
    }
}
