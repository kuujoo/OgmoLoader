using System;
using Microsoft.Xna.Framework;

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
        public string UserDataText { get; set; }
        public Color UserDataColor { get; set; }
        public Flags Flag;
        public Types Type;
        public string Name;
        public int ChildLevel;
        public int BlendMode;
        public float Alpha;
    }
}