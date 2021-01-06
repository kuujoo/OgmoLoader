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
        public void SetRenderTargetToBackBuffer()
        {
            Device.SetRenderTarget(null);
        }
        public Graphics()
        {
        }
        internal void Bind(GraphicsDeviceManager graphicDevice)
        {
            DeviceManager = graphicDevice;
            SpriteBatch = new SpriteBatch(Device);
            if(Pixel == null)
            {
                Pixel = new Texture2D(Device, 1, 1);
                Color[] colors = { Color.White };
                Pixel.SetData<Color>(colors);
            }
        }
        public void Begin(Camera camera = null)
        {
            Camera = camera;
            if (camera != null && camera.Surface != null)
            {
                Device.SetRenderTarget(camera.Surface.Target); 
            }
            else
            {
                Device.SetRenderTarget(null);
            }

            if (camera != null && camera.Type == CameraType.Base)
            {
                Device.Clear(camera.BackgroundColor);
            }
            if (camera != null)
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, camera.Matrix);
            }
            else
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            }
        }
        public void End()
        {
            SpriteBatch.End();
        }
        public void DrawPoint(Vector2 at, Color color)
        {
            SpriteBatch.Draw(Pixel, at, new Rectangle(0,0,1,1), color, 0, Microsoft.Xna.Framework.Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
        public void DrawRect(Rectangle rect, Color color)
        {
            SpriteBatch.Draw(Pixel, rect, color);
        }
        public void DrawSpriteFrame(Point at, Sprite.Frame frame, Color color)
        {
            SpriteBatch.Draw(frame.Texture, at.ToVector2(), frame.Rect, color);
        }
    }
}
