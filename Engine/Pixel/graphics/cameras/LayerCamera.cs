namespace kuujoo.Pixel
{
    public class LayerCamera : Camera
    {
        public int RenderLayer { get; set; }
        public LayerCamera(int width, int height) : base(width, height)
        {
            RenderLayer = 0;
        }

        public override void Render(Graphics graphics, IRenderable renderable)
        {
            if (renderable.Layer == RenderLayer)
            {
                base.Render(graphics, renderable);
            }
        }
    }

}
