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
            graphics.SpriteBatch.Draw(resources.TexturePages[0], new Vector2(Entity.Position.X, Entity.Position.Y), Color.White);
        }
    }

    public class AnimatedSpriteComponent : Component
    {
        public Sprite Sprite;
        float frame = 0;
    
        public override void Update()
        {
            base.Update();
            frame += 1.0f * Time.DeltaTime;
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            graphics.DrawSprite(Entity.Position, Sprite, (int)frame % Sprite.Bounds.Length, Color.White);
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
            var resources = room.AddSceneComponent(new SpriteResources(2048, 2048)) as SpriteResources;
            resources.AddAseSprite("Checker", "Content/Sprites/checker.ase");
            resources.AddAseSprite("Animated","Content/Sprites/animation.ase");
            resources.AddAseSprite("Circle", "Content/Sprites/circle.ase");

            var w = new System.Diagnostics.Stopwatch();
            w.Start();
            resources.Build();
            var t = w.Elapsed.TotalSeconds;
            room.AddCamera(new Camera(384, 216)
            {
                BackgroundColor = Color.Aquamarine
            });

            room.CreateEntityLayer(0, "entities");
            var animated_entity = room.CreateEntity(0);
            animated_entity.AddComponent(new AnimatedSpriteComponent()
            {
                Sprite = resources.GetSprite("Animated", "ColorBox")
            });
            animated_entity.Position = new Vector2(12, 12);
            room.AddEntity(animated_entity, 0);

            var e0 = room.CreateEntity(0);
            e0.AddComponent(new SpriteComponent()
            {
                Sprite = resources.GetSprite("Checker")
            });
            e0.Position = new Vector2(200, 100);

            var e1 = room.CreateEntity(0);     
            e1.AddComponent(new SpriteComponent()
            {
                Sprite = resources.GetSprite("Circle")
            });
            e1.Position = new Vector2(100, 100);

            var e2 = room.CreateEntity(0);
            e2.Position = new Vector2(100, 0);
            e2.AddComponent(new TexturePagesComponent());
            Scene = room;
        }
    }
}
