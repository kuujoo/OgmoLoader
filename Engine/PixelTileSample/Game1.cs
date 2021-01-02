using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class Game1 : Engine
    {
        public static string SheetName = "Content/Sprites/sheet.ase";
        public Game1() : base()
        {

        }
        protected override void Initialize()
        {
            base.Initialize();

            var room = new Scene(384, 216);
            var res = room.AddSceneComponent(new SpriteResources()) as SpriteResources;
            room.AddCamera(new Camera(384, 216));
            var sp = res.GetSprite(SheetName, "colors_strip_4");
            var tileset = new Tileset(16, 16, sp.Texture, sp.Bounds);
            var layer = room.CreateTileLayer(0, 100, 100, tileset);
            for(var i = 0; i < layer.Width * layer.Height; i++)
            {
                layer.SetValueByIndex(i, (byte)(i % 3 + 1));
            }
            Scene = room;
        }
    }
}
