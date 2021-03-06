﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace kuujoo.Pixel
{
    public class SpriteRenderer : Component, IRenderable, kuujoo.Pixel.IUpdateable
    {
        public int Layer { get; set; }
        public enum PlayMode
        {
            Loop,
            Once,
        }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
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
        public int UpdateOrder { get; set; }
        public Color Color { get; set; }
        public Sprite.Frame CurrentFrame => Sprite.Animations[_animationIndex].Frames[_frameIndex];
        public int Frame { get; set; }
        public Sprite Sprite { get; set; }

        int _animationIndex = 0;
        int _frameIndex = 0;
        bool _play = false;
        bool _paused = false;
        float _frameTimer = 0.0f;
        PlayMode _mode;
        public SpriteRenderer()
        {
            Scale = Vector2.One;
            Frame = 0;
            FlipX = false;
            Color = Color.White;
        }
        public void Set(Sprite sprite)
        {
            Sprite = sprite;
        }
        public override void CleanUp()
        {
            base.CleanUp();
            _animationIndex = 0;
            _frameIndex = 0;
            _play = false;
            _paused = false;
            _frameTimer = 0.0f;
            Sprite = null;
            Scale = Vector2.One;
            Frame = 0;
            FlipX = false;
            Color = Color.White;
        }
        public void Update()
        {
            if (Sprite == null || _paused) return;

            if(_play)
            {
                _frameTimer = Calc.Approach(_frameTimer, 0.0f, Time.DeltaTime);
                if(_frameTimer <= 0)
                {
                    FrameIndex += 1;
                    _frameTimer = CurrentFrame.Duration;
                }
                if(_mode == PlayMode.Once && FrameIndex >= Sprite.Animations[_animationIndex].Frames.Count - 1)
                {
                    Stop();
                }
            }
        }
        public void DebugRender(Graphics graphics)
        {
            graphics.DrawPoint(Entity.Transform.Position.ToVector2(), Color.Pink);
        }
        public void Render(Graphics graphics)
        {
            if (Sprite == null) return;

            graphics.DrawSpriteFrame(Entity.Transform.Position.ToVector2(), Sprite.Pivot, Scale, Rotation, CurrentFrame, Color, FlipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None);  
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            var bounds = camera.Bounds;
            var b = CurrentFrame.Rect;
            b.Location = Entity.Transform.Position;
            b.Location -= Sprite.Pivot.ToPoint();
            return bounds.Intersects(b);
        }
        public void Play(string name, PlayMode mode = PlayMode.Loop)
        {
            _mode = mode;
            for (var i = 0; i < Sprite.Animations.Count;i++)
            {
                if(Sprite.Animations[i].Name == name)
                {
                    if (_play == true && AnimationIndex == i) return;

                    AnimationIndex = i;
                    FrameIndex = 0;
                    _frameTimer = Sprite.Animations[_animationIndex].Frames[_frameIndex].Duration;
                    _play = true;
                    return;
                }
            }
            Debug.Assert(false, "Animation not found");
        }
        public void SetFrame(string name, int frame)
        {
            for (var i = 0; i < Sprite.Animations.Count; i++)
            {
                if (Sprite.Animations[i].Name == name)
                {
                    AnimationIndex = i;
                    FrameIndex = frame;
                    return;
                }
            }
            Debug.Assert(false, "Animation not found");
        }
        public void Pause()
        {
            _paused = true;
        }
        public void UnPause()
        {
            _paused = false;
        }
        public void Stop()
        {
            _play = false;
        }
    }
}
