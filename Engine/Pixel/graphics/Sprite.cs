using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class Sprite
    {
        public class Frame : IComparable<Frame>
        {
            public int Id { get; set; }
            public Texture2D Texture { get; set; }
            public Rectangle Rect {get;set;}
            public float Duration { get; set; }

            public int CompareTo([AllowNull] Frame other)
            {
                return Id.CompareTo(other.Id);
            }
        }
        public class Animation
        {
            public string Name { get; set; }
            public List<Frame> Frames { get; protected set; }
            public Animation()
            {
                Frames = new List<Frame>();
            }
            public void AddFrame(Frame frame)
            {
                Frames.Add(frame);
                Frames.Sort();
            }
        }
        public Sprite.Frame DefaultFrame => Animations[0].Frames[0];
        public Vector2 Pivot { get; protected set; }
        public List<Animation> Animations { get; protected set; }
        public Sprite()
        {
            Animations = new List<Animation>();
        }
        public void SetPivot(Vector2 pivot)
        {
            Pivot = pivot;
        }
        public Animation GetAnimation(string name)
        {
            for (var i = 0; i < Animations.Count; i++)
            {
                if (Animations[i].Name == name)
                {
                    return Animations[i];
                }
            }
            return null;
        }
        public int GetAnimationIndex(string name)
        {
            for (var i = 0; i < Animations.Count; i++)
            {
                if (Animations[i].Name == name)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
