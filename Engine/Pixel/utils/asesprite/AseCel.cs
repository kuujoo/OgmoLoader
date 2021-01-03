using Microsoft.Xna.Framework;

// Originally from gist by Noel Berry
// https://gist.github.com/NoelFB/778d190e5d17f1b86ebf39325346fcc5/revisions
// Added few little modifications

// File Format:
// https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md

// Note: I didn't test with with Indexed or Grayscale colors
// Only implemented the stuff I needed / wanted, other stuff is ignored

namespace kuujoo.Pixel
{
    public class AseCel : IAseUserData
    {
        public AseLayer Layer;
        public Color[] Pixels;

        public int X;
        public int Y;
        public int Width;
        public int Height;
        public float Alpha;

        public string UserDataText { get; set; }
        public Color UserDataColor { get; set; }
    }
}