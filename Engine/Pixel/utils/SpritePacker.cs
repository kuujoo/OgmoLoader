using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using kuujoo.Pixel.Packer;

namespace kuujoo.Pixel
{
    public class TexturePage : IDisposable
    {
        public Texture2D Texture { get; set; }
        public List<Atlas.SubImage> SubTextures { get; private set; }

        public static TexturePage FromAtlas(GraphicsDevice graphicesDevice ,Atlas atlas)
        {
            var page = new TexturePage();
            page.SubTextures = atlas.Subimages;
            page.Texture = new Texture2D(graphicesDevice, atlas.Width, atlas.Height);
            page.Texture.SetData<kuujoo.Pixel.Packer.Color>(atlas.Pixels);
            return page;
        }
        public TexturePage()
        {
            SubTextures = new List<Atlas.SubImage>();
        }

        void Dispose(bool dispose)
        {
            if(dispose && Texture != null)
            {
                Texture.Dispose();
                Texture = null;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
