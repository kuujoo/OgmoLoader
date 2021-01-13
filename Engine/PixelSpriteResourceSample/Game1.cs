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
            var resources = room.AddSceneComponent(new SpriteResources(2048, 2048, "Content/Sprites")) as SpriteResources;
            var cameraEntity = room.CreateEntity(0);
            var camera = cameraEntity.AddComponent(new Camera(384, 216)
            {
                BackgroundColor = Color.Aquamarine
            });
            room.AddCamera(camera);

            var animated_entity = room.CreateEntity(0);
            animated_entity.Transform.SetPosition(12, 12);
            var sprite = animated_entity.AddComponent(new SpriteRenderer()
            {
                Sprite = resources.GetSprite("animation")
            });
            sprite.Play("ColorBox");


            var e0 = room.CreateEntity(0);
            e0.Transform.SetPosition(200, 100);
            e0.AddComponent(new SpriteRenderer()
            {
                Sprite = resources.GetSprite("checker")
            });

            var e1 = room.CreateEntity(0);
            e1.Transform.SetPosition(100, 100);
            e1.AddComponent(new SpriteRenderer()
            {
                Sprite = resources.GetSprite("circle")
            });

            var e2 = room.CreateEntity(0);
            e2.Transform.SetPosition(130, 140);
            e2.AddComponent(new SpriteRenderer()
            {
                Sprite = resources.GetSprite("png_sprite")
            });

            var e3 = room.CreateEntity(0);
            e3.Transform.SetPosition(130, 12);
            e3.AddComponent(new SpriteRenderer()
            {
                Sprite = resources.GetSprite("jpg_sprite")
            });

            var e4 = room.CreateEntity(0);
            e4.Transform.SetPosition(200, 12);
            e4.AddComponent(new SpriteRenderer()
            {
                Sprite = resources.GetSprite("bmp_sprite")
            });

            Scene = room;
        }
    }
}
