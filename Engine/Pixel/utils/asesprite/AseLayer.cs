using System;
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
    public class AseLayer : IAseUserData
    {
        [Flags]
        public enum Flags
        {
            Visible = 1,
            Editable = 2,
            LockMovement = 4,
            Background = 8,
            PreferLinkedCels = 16,
            Collapsed = 32,
            Reference = 64
        }

        public enum Types
        {
            Normal = 0,
            Group = 1
        }

        public Flags Flag;
        public Types Type;
        public string Name;
        public int ChildLevel;
        public int BlendMode;
        public float Alpha;

        public string UserDataText { get; set; }
        public Color UserDataColor { get; set; }
    }
}