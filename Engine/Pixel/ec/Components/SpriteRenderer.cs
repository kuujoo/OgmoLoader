using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class SpriteRenderer : Component
    {
        public Vector2 Scale { get; set; }
        public bool FlipX { get; set; }
        public int AnimationIndex { 
            get { 
                return _animationIndex;
            }
            set
            {
                _animationIndex = value % Sprite.Animations.Count;
            }
        }
        public int FrameIndex
        {
            get
            {
                return _frameIndex;
            }
            set
            {
                _frameIndex = value % Sprite.Animations[AnimationIndex].Frames.Count;
            }
        }
        public Sprite.Frame CurrentFrame => Sprite.Animations[_animationIndex].Frames[_frameIndex];
        public int Frame { get; set; }
        public Sprite Sprite { get; set; }
        int _animationIndex = 0;
        int _frameIndex = 0;
        bool _play = false;
        float _frameTimer = 0.0f;
        public SpriteRenderer()
        {
            Scale = Vector2.One;
            Frame = 0;
            FlipX = false;
        }
        public override void Update()
        {
            if (Sprite == null) return;

            if(_play)
            {
                _frameTimer = MathExt.Approach(_frameTimer, 0.0f, Time.DeltaTime);
                if(_frameTimer <= 0)
                {
                    FrameIndex += 1;
                    _frameTimer = CurrentFrame.Duration;
                }
            }
        }
        public override void DebugRender(Graphics graphics)
        {
            base.DebugRender(graphics);
            graphics.DrawPoint(Entity.Transform.Position.ToVector2(), Color.Pink);
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            if (Sprite == null) return;

            graphics.DrawSpriteFrame(Entity.Transform.Position.ToVector2(), Sprite.Pivot, Scale, CurrentFrame, Color.White, FlipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None);  
        }
        public void Play(string name)
        {
            for(var i = 0; i < Sprite.Animations.Count;i++)
            {
                if(Sprite.Animations[i].Name == name)
                {
                    if (_play == true && AnimationIndex == i) return;

                    AnimationIndex = i;
                    FrameIndex = 0;
                    _frameTimer = Sprite.Animations[_animationIndex].Frames[_frameIndex].Duration;
                    _play = true;
                    break;
                }
            }
        }
        public void Stop()
        {
            _play = false;
        }
    }
}
