using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public class DefaultRenderer : Renderer
    {
        public override void BeginRender(Scene scene)
        {
            base.BeginRender(scene);
        }
        public override void Render(Scene scene)
        {
            var gfx = GetGraphics();
            gfx.Begin(Surface, scene.ClearColor);
            for (var i = 0; i < scene.Entities.Count; i++)
            {
                scene.Entities[i].Render(gfx);
            }
            gfx.End();
        }
        public override void EndRender(Scene scene)
        {
            base.EndRender(scene);
        }
    }
}
