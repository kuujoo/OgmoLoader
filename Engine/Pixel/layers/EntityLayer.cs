﻿namespace kuujoo.Pixel
{
    public class EntityLayer : Layer
    {
        public Scene  Scene { get; private set; }
        public SortedList<Entity> Entities { get; private set; }
        public EntityLayer(Scene scene, int layerid)
        {
            Scene = scene;
            Id = layerid;
            Entities = new SortedList<Entity>();
        }
        public Entity AddEntity(Entity entity)
        {
            entity.Layer = this;
            entity.Initialize();
            Entities.Add(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            entity.Destroy();
            entity.Components.AcceptVisitor(ComponentListVisitor.DestroyVisitor, true);

            entity.CleanUp();
            entity.Components.AcceptVisitor(ComponentListVisitor.CleanUpVisitor, true);
            entity.Components.Clear();
            Entities.Remove(entity);
        }
        public override void Update()
        {
            Entities.UpdateLists();
            Entities.AcceptVisitor(EntityListVisitor.UpdateVisitor, false);
        }
        public override void Render(Camera camera)
        {
            if (!camera.CanRenderLayer(this)) return;
            
             var gfx = Engine.Instance.Graphics;
            var visitor = EntityListVisitor.RenderVisitor;
            visitor.Camera = camera;
            visitor.Graphics = gfx;
            Entities.AcceptVisitor(visitor, false);
        }
        public override void CleanUp()
        {
            Entities.AcceptVisitor(EntityListVisitor.CleanUpVisitor, true);
            Entities.Clear();
        }
        public override void OnGraphicsDeviceReset()
        {
            Entities.AcceptVisitor(EntityListVisitor.GraphicsDeviceResetVisitor, true);
        }
    }
}
