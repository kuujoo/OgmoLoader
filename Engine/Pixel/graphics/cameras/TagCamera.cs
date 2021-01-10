namespace kuujoo.Pixel
{
    public class TagCamera : Camera
    {
        public int RenderTag { get; set; }
        public TagCamera(int width, int height) : base(width, height)
        {
            RenderTag = 0;
        }
        public override void Render(Graphics graphics, EntityList entities, bool debugrender)
        {


            graphics.PushSurface(Surface);
            graphics.PushMatrix(Matrix);
            graphics.Begin();
            graphics.Device.Clear(BackgroundColor);
            graphics.Camera = this;
            for (var i = 0; i < entities.Count; i++)
            {
                if (entities[i].Tag == RenderTag)
                {
                    entities[i].Components.Render(graphics);
                }
            }
            if (debugrender)
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    if (entities[i].Tag == RenderTag)
                    {
                        entities[i].Components.DebugRender(graphics);
                    }
                }
            }
            graphics.PopMatrix(Matrix);
            graphics.PopSurface(Surface);
            graphics.End();

        }
    }
}
