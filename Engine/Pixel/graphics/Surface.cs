using Microsoft.Xna.Framework.Graphics;
using System;

namespace kuujoo.Pixel
{
    public class Surface : IDisposable
    {
        public RenderTarget2D Target;
        public Surface(int width, int height)
        {
            Resize(width, height);
        }

        public void Resize(int width, int height)
        {
            if (Target != null)
            {
                Target.Dispose();
                Target = null;
            }
            Target = new RenderTarget2D(Engine.Instance.Graphics.Device, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }
        void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (Target != null)
                {
                    Target.Dispose();
                    Target = null;
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
