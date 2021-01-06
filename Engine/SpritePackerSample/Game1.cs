using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{

    public class TexturePagesComponent : Component
    {
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
                graphics.SpriteBatch.Draw(resources.TexturePages[i].Texture, new Vector2(Entity.Position.X + x, Entity.Position.Y + y), Color.White);
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
            var resources = room.AddSceneComponent(new SpriteResources(256, 256, "Content/Sprites")) as SpriteResources;

            room.AddCamera(new Camera(1920, 1080)
            {
                BackgroundColor = Color.Aquamarine
            });

            room.CreateEntityLayer(0, "entities");
            var texturepages_entity = room.CreateEntity(0);
            texturepages_entity.AddComponent(new TexturePagesComponent());
            texturepages_entity.Position = new Vector2(0, 0);
            room.AddEntity(texturepages_entity, 0);
            Scene = room;
        }
    }
}
