using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public class Screen
    {
        GraphicsDeviceManager _graphics;
        public int ViewWidth { get; private set; }
        public int ViewHeight { get; private set; }
        public int Width => _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
        public int Height => _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
        public Screen(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }
        public  void SetSize(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }
    }
}
