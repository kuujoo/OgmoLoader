using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class SpriteComponent : Component
    {
        public Sprite Sprite { get; set; }
        public SpriteComponent()
        {

        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            if (Sprite == null) return;

            graphics.DrawSprite(Entity.Position - Sprite.Pivot, Sprite, Color.White);
        }
    }
}
