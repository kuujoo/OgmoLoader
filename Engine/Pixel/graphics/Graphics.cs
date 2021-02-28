using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{

    public class Graphics
    {
        public GraphicsDeviceManager DeviceManager { get; private set; }
        public GraphicsDevice Device => DeviceManager.GraphicsDevice;
        public Texture2D Pixel { get; private set; }
        public Camera Camera { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Stack<Surface> _surfaceStack = new Stack<Surface>();
        public Stack<Matrix> _matrixStack = new Stack<Matrix>();
        public Stack<Effect> _effectStack = new Stack<Effect>();
        public Stack<SamplerState> _samplerStates = new Stack<SamplerState>();
        public Stack<BlendState> _blendingStates = new Stack<BlendState>();
        public Graphics(GraphicsDeviceManager devicemanager)
        {
            DeviceManager = devicemanager;
            SpriteBatch = new SpriteBatch(Device);
            if (Pixel == null)
            {
                Pixel = new Texture2D(Device, 1, 1);
                Color[] colors = { Color.White };
                Pixel.SetData<Color>(colors);
            }
        }
        public void SetRenderTargetToBackBuffer()
        {
            Device.SetRenderTarget(null);
        }
        public void PushSurface(Surface surface)
        {
            _surfaceStack.Push(surface);
        }
        public void PushSamplerState(SamplerState state)
        {
            _samplerStates.Push(state);
        }
        public void PopSamplerState()
        {
            _samplerStates.Pop();
        }
        public void PushBlendState(BlendState state)
        {
            _blendingStates.Push(state);
        }
        public void PopBLendState()
        {
            _blendingStates.Pop();
        }
        public void PushMatrix(Matrix matrix)
        {
            _matrixStack.Push(matrix);
        }
        public void PopMatrix(Matrix matrix)
        {
            var top = _matrixStack.Peek();
            if (top == matrix)
            {
                _matrixStack.Pop();
            }
        }
        public void PopSurface(Surface surface)
        {
            var top = _surfaceStack.Peek();
            if(top == surface)
            {
                _surfaceStack.Pop();
            }
        }
        public void PushEffect(Effect effect)
        {
            _effectStack.Push(effect);
        }
        public void PopEffect(Effect effect)
        {
            if (_effectStack.Peek() == effect)
            {
                _effectStack.Pop();
            }
        }
        public void Begin()
        {
            if (_surfaceStack.Count > 0)
            {
                Device.SetRenderTarget(_surfaceStack.Peek().Target);
            }
            else
            {
                Device.SetRenderTarget(null);
            }

            var effect = _effectStack.Count > 0 ? _effectStack.Peek() : null;
            var samplerState = _samplerStates.Peek();
            var blendState = _blendingStates.Peek();
            if (_matrixStack.Count > 0)
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, blendState, samplerState, DepthStencilState.None, RasterizerState.CullNone, effect, _matrixStack.Peek());
            }
            else
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, blendState, samplerState, DepthStencilState.None, RasterizerState.CullNone, effect);
            }
        }
        public void End()
        {
            SpriteBatch.End();
        }
        public void DrawPoint(Vector2 at, Color color)
        {
            SpriteBatch.Draw(Pixel, at, new Rectangle(0,0,1,1), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
        public void DrawRect(Rectangle rect, Color color)
        {
            SpriteBatch.Draw(Pixel, rect, color);
        }
        public void DrawHollowRect(Rectangle rect, Color color)
        {
            SpriteBatch.Draw(Pixel, new Rectangle(rect.Left, rect.Top, 1, rect.Height), color);
            SpriteBatch.Draw(Pixel, new Rectangle(rect.Right, rect.Top, 1, rect.Height), color);

            SpriteBatch.Draw(Pixel, new Rectangle(rect.Left, rect.Top, rect.Width, 1), color);
            SpriteBatch.Draw(Pixel, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), color);       
        }
        public void DrawSpriteFrame(Vector2 at, Vector2 pivot, Vector2 scale, Sprite.Frame frame, Color color, SpriteEffects effect )
        {
            SpriteBatch.Draw(frame.Texture, at, frame.Rect, color, 0.0f, pivot, scale, effect, 0.0f);
        }
        public void DrawTexture(Texture2D texture, Vector2 at)
        {
            SpriteBatch.Draw(texture, at, Color.White);
        }
        public void DrawTexture(Texture2D texture, Rectangle destination, Rectangle source, Color color)
        {
            SpriteBatch.Draw(texture, destination, source, color);
        }
        public void DrawText(BMFont font, string text, Vector2 position, Color color)
        {
            font.ProcessChars(text, (actual, drawPos, data, previous) => {
                var sourceRectangle = new Rectangle(data.X, data.Y, data.Width, data.Height);
                var destRectangle = new Rectangle((int)(position.X + drawPos.X), (int)(position.Y + drawPos.Y),
                    (int)(data.Width), (int)(data.Height));

                SpriteBatch.Draw(font.Texture, destRectangle, sourceRectangle, color);
            }, null);
        }
    }
}
