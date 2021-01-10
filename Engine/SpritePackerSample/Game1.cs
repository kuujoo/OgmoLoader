using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{

    public class TexturePagesRenderer : Component
    {
        public override bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            var resources = Scene.GetSceneComponent<SpriteResources>();
            int y = 0;
            int x = 0;
            for (var i = 0; i < resources.TexturePages.Length; i++)
            {
                x = (i % 4) * 280;
                if (i != 0 && i % 4 == 0)
                {
                    y += 280;
                }
                graphics.SpriteBatch.Draw(resources.TexturePages[i].Texture, new Vector2(Entity.Transform.Position.X + x, Entity.Transform.Position.Y + y), Color.White);
            }

        }
    }
    public class Game1 : Engine
    {
        public Game1() : base()
        {

        }
        protected override void Initialize()
        {
            base.Initialize();
            Screen.SetSize(1920, 1080);
            var room = new Scene(1920, 1080);
            var resources = room.AddSceneComponent(new SpriteResources(256, 256, "Content/Sprites"));
            var cameraEntity = room.CreateEntity(0);
            var camera = cameraEntity.AddComponent(new Camera(1920, 1080)
            {
                BackgroundColor = Color.Aquamarine
            });
            room.AddCamera(camera);
            var texturepages_entity = room.CreateEntity(0);
            texturepages_entity.AddComponent(new TexturePagesRenderer());
            texturepages_entity.Transform.SetPosition(12, 12);
            room.AddEntity(texturepages_entity, 0);
            Scene = room;
        }
    }
}
