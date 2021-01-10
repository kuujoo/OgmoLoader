using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class TexturePagesRenderer : Component
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
            e2.Transform.SetPosition(100, 0);
            e2.AddComponent(new TexturePagesRenderer());
            Scene = room;
        }
    }
}
