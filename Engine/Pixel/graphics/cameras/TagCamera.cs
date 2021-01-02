namespace kuujoo.Pixel
{
    public class TagCamera : Camera
    {
        public int RenderTag { get; set; }
        public TagCamera(int width, int height) : base(width, height)
        {
            RenderTag = 0;
        }
        public override bool CanSee(Entity entity)
        {
            return entity.Tag == RenderTag;
        }
    }
}
