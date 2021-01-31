using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class TextComponent : Component
    {
        public BMFont Font { get; set; }
        public override void Initialize()
        {
        }
        public override bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            graphics.DrawText(Font, "Hello World\nthis is text", Entity.Transform.Position.ToVector2(), Color.White);
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
            var spriteResources = room.AddSceneComponent(new SpriteResources(2048, 2048, "Content/Sprites"));
            var fontResources = room.AddSceneComponent(new FontResources(2048, 2048, "Content/Fonts"));
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
                Sprite = spriteResources.GetSprite("animation")
            });
            sprite.Play("ColorBox");


            var e0 = room.CreateEntity(0);
            e0.Transform.SetPosition(200, 100);
            e0.AddComponent(new SpriteRenderer()
            {
                Sprite = spriteResources.GetSprite("checker")
            });

            var e1 = room.CreateEntity(0);
            e1.Transform.SetPosition(100, 100);
            e1.AddComponent(new SpriteRenderer()
            {
                Sprite = spriteResources.GetSprite("circle")
            });

            var e2 = room.CreateEntity(0);
            e2.Transform.SetPosition(130, 140);
            e2.AddComponent(new SpriteRenderer()
            {
                Sprite = spriteResources.GetSprite("png_sprite")
            });

            var e3 = room.CreateEntity(0);
            e3.Transform.SetPosition(130, 12);
            e3.AddComponent(new SpriteRenderer()
            {
                Sprite = spriteResources.GetSprite("jpg_sprite")
            });

            var e4 = room.CreateEntity(0);
            e4.Transform.SetPosition(200, 12);
            e4.AddComponent(new SpriteRenderer()
            {
                Sprite = spriteResources.GetSprite("bmp_sprite")
            });

            var e5 = room.CreateEntity(0);
            e5.Transform.SetPosition(10, 80);
            e5.AddComponent(new TextComponent()
            {
                Font = fontResources.GetFont("fnt")
            });
            Scene = room;
        }
    }
}
