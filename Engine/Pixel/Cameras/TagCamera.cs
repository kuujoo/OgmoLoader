namespace kuujoo.Pixel
{
    public class TagCamera : Camera
    {
        public int RenderTag { get; set; }
        public TagCamera(int width, int height) : base(width, height)
        {
            RenderTag = 0;
        }
        public override void Render(Scene scene)
        {
            var gfx = Engine.Instance.Graphics;
            gfx.Begin(this);
            for (var i = 0; i < scene.Entities.Count; i++)
            {
                if (scene.Entities[i].Tag == RenderTag)
                {
                    scene.Entities[i].Render(gfx);
                }
            }
            gfx.End();
        }
    }
}
