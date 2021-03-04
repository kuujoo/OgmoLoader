using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public abstract class PostProcessor
    {
        public Scene Scene { get; set; }
        public abstract void Initialize();
        public abstract Surface Process(Graphics gfx, Surface surface, Rectangle finalRectangle);
    }
}
