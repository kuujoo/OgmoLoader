using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class SpriteComponent : Component
    {
        public int Frame { get; set; }
        public Sprite Sprite { get; set; }
        public SpriteComponent()
        {
            Frame = 0;
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            if (Sprite == null) return;

            graphics.DrawSprite(Entity.Position - Sprite.Pivot, Sprite, Frame, Color.White);
        }
    }
}
