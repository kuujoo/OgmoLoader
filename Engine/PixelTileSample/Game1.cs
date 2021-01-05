using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class Game1 : Engine
    {
        public Game1() : base()
        {

        }
        protected override void Initialize()
        {
            base.Initialize();

            var room = new Scene(384, 216);
            var res = room.AddSceneComponent(new SpriteResources(128,128)) as SpriteResources;
            res.AddAseTileset("tiles", "Content/Sprites/sheet.ase", 16, 16);
            res.Build();
            room.AddCamera(new Camera(384, 216));
            var tileset = res.GetTileset("tiles");
            var layer = room.CreateTileLayer(0, "tiles", 100, 100, tileset);
            for(var i = 0; i < layer.Width * layer.Height; i++)
            {
                layer.SetValueByIndex(i, (byte)(i % 3 + 1));
            }
            Scene = room;
        }
    }
}
