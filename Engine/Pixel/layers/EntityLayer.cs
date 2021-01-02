namespace kuujoo.Pixel
{
    public class EntityLayer : Layer
    {
        public EntityList Entities { get; private set; }
        Scene _scene;
        public EntityLayer(Scene scene, int layerid)
        {
            _scene = scene;
            Id = layerid;
            Entities = new EntityList(_scene, this);
        }
        public override void Update()
        {
            Entities.UpdateLists();
            Entities.AcceptVisitor(EntityListVisitor.UpdateVisitor, false);
        }
        public override void Render(Camera camera)
        {
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
