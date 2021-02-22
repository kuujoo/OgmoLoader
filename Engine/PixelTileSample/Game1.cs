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
            string[] sprites = { "Content/Sprites" };
            var res = room.AddSceneComponent(new SpriteResources(128,128, sprites)) as SpriteResources;
            var cameraEntity = room.CreateEntity();
            room.AddCamera(cameraEntity.AddComponent(new Camera(384, 216)));
            var sprite = res.GetSprite("sheet");
            var r = room.CreateEntity();
            var tilemap = r.AddComponent(new TilemapRenderer(100, 100, new Tileset(16, 16, sprite.DefaultFrame.Texture, sprite.DefaultFrame.Rect)));
            for(var i = 0; i < 100 * 100; i++)
            {
                tilemap.SetValueByIndex(i, (byte)(i % 3 + 1));
            }
            Scene = room;
        }
    }
}
