using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public static class Screen
    {
        internal static GraphicsDeviceManager _graphics;
        public static int ViewWidth { get; private set; }
        public static int ViewHeight { get; private set; }
        public static int Width => _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
        public static int Height => _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
        public static void Bind(GraphicsDeviceManager graphicsManager)
        {
            _graphics = graphicsManager;

        }
        public static void SetSize(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }
    }
}
