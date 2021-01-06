using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class Sprite
    {
        public class Frame
        {
            public Texture2D Texture { get; set; }
            public Rectangle Rect {get;set;}
            public float Duration { get; set; }
        }
        public class Animation
        {
            public string Name { get; set; }
            public List<Frame> Frames { get; protected set; }
            public Animation()
            {
                Frames = new List<Frame>();
            }
        }
        public Sprite.Frame DefaultFrame => Animations[0].Frames[0];
        public Vector2 Pivot { get; protected set; }
        public List<Animation> Animations { get; protected set; }
        public Sprite()
        {
            Animations = new List<Animation>();
        }
    }
}
