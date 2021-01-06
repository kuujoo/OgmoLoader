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
            graphics.SpriteBatch.Draw(resources.TexturePages[0].Texture, new Vector2(Entity.Transform.Position.X, Entity.Transform.Position.Y), Color.White);
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

            var room = new Scene(384, 216);
            var resources = room.AddSceneComponent(new SpriteResources(2048, 2048, "Content/Sprites")) as SpriteResources;
            var w = new System.Diagnostics.Stopwatch();
            room.AddCamera(new Camera(384, 216)
            {
                BackgroundColor = Color.Aquamarine
            });

            room.CreateEntityLayer(0, "entities");
            var animated_entity = room.CreateEntity(0);
            animated_entity.AddComponent(new SpriteComponent()
            {
                Sprite = resources.GetSprite("animation")
            });
            animated_entity.Transform.SetPosition(12, 12);
            animated_entity.GetComponent<SpriteComponent>().Play("ColorBox");
            room.AddEntity(animated_entity, 0);

            var e0 = room.CreateEntity(0);
            e0.AddComponent(new SpriteComponent()
            {
                Sprite = resources.GetSprite("checker")
            });
            e0.Transform.SetPosition(200, 100);
            var e1 = room.CreateEntity(0);     
            e1.AddComponent(new SpriteComponent()
            {
                Sprite = resources.GetSprite("circle")
            });
            e1.Transform.SetPosition(100, 100);

            var e2 = room.CreateEntity(0);
            e2.Transform.SetPosition(100, 0);
            e2.AddComponent(new TexturePagesComponent());
            Scene = room;
        }
    }
}
