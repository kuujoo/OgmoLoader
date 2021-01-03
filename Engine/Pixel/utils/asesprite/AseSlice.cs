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
    public struct AseSlice : IAseUserData
    {
        public int Frame;
        public string Name;
        public int OriginX;
        public int OriginY;
        public int Width;
        public int Height;
        public Vector2? Pivot;
        public string UserDataText { get; set; }
        public Color UserDataColor { get; set; }
    }
}