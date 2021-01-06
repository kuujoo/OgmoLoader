using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public class AseFrame
    {
        public AseSprite Sprite;
        public int Duration;
        public Color[] Pixels;
        public List<AseCel> Cels;
        public AseFrame(AseSprite sprite)
        {
            Sprite = sprite;
            Cels = new List<AseCel>();
        }
    }
}