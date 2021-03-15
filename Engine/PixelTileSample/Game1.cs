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
            var res = room.AddSceneComponent(new RuntimeSpriteLibrary(128,128, sprites));
            var cameraEntity = room.CreateEntity();
            var camera = cameraEntity.AddComponent(room.Get<Camera>());
            camera.SetSize(384, 216);
            camera.SetViewport(384, 216);
            cameraEntity.Transform.SetPosition(384 / 2, 216 / 2);
            room.AddCamera(camera);

            var sprite = res.GetSprite("sheet");
            var r = room.CreateEntity();
            var tilemap = r.AddComponent(room.Get<TilemapRenderer>());

            ByteGrid grid = new ByteGrid(100, 100);
            tilemap.Set(grid, new Tileset(16, 16, sprite.DefaultFrame.Texture, sprite.DefaultFrame.Rect));
            for(var i = 0; i < 100 * 100; i++)
            {
                grid.SetValueByIndex(i, (byte)(i % 3 + 1));
            }
            Scene = room;
        }
    }
}
