using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class OgmoWorld
    {
        public List<OgmoLevel> Levels { get; set; }
        public OgmoWorld()
        {
            Levels = new List<OgmoLevel>();
        }
    }
}
