using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class SurfaceLibrary : SceneComponent
    {
        List<Surface> _surfaces = new List<Surface>();
        public Surface CreateSurface(int width, int height, Color initialColor)
        {
            var surface = new Surface(width, height, initialColor);
            _surfaces.Add(surface);
            return surface;
        }
        public override void CleanUp()
        {
           for(var i = 0; i < _surfaces.Count; i++)
            {
                _surfaces[i].Dispose();
            }
        }
        public void FreeSurface(Surface surface)
        {
            _surfaces.Remove(surface);
            surface.Dispose();
        }
    }
}
