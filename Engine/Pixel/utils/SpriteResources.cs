using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{ 
    public class SpriteResources : SceneComponent
    {
        public SpriteResources()
        {
        }
        public Sprite GetSprite(string sheetpath, string region)
        {
            var sheet = Scene.Content.LoadASESpriteSheet(sheetpath);
            return sheet.GetSprite(region);
        }
    }
}
