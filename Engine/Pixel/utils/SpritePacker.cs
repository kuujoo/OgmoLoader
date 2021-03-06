﻿using Microsoft.Xna.Framework;
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

        public static TexturePage FromAtlas(GraphicsDevice graphicesDevice, Atlas atlas)
        {
            var page = new TexturePage();
            page.SubTextures = atlas.Subimages;

            kuujoo.Pixel.Packer.Color[] preMultiply = new kuujoo.Pixel.Packer.Color[atlas.Width * atlas.Height];
            for(var i = 0; i < atlas.Pixels.Length; i++)
            {
                preMultiply[i] = atlas.Pixels[i].ToPreMultiplied();
            }
            page.Texture = new Texture2D(graphicesDevice, atlas.Width, atlas.Height);
            page.Texture.SetData<kuujoo.Pixel.Packer.Color>(preMultiply);
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
