using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class AnimatedSpriteComponent : Component
    {
        Sprite _sprite;
        float frame = 0;
        public override void Initialize()
        {
            _sprite = Scene.GetSceneComponent<SpriteResources>().GetSprite("ColorBox");
        }
        public override void Update()
        {
            base.Update();
            frame += 1.0f * Time.DeltaTime;
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            graphics.DrawSprite(Entity.Position, _sprite, (int)frame % _sprite.Bounds.Length, Color.White);
        }
    }

    public class TexturePagesComponent : Component
    {
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            var resources = Scene.GetSceneComponent<SpriteResources>();
            int y = 0;
            for (var i = 0; i < resources.TexturePages.Length; i++)
            {
                graphics.SpriteBatch.Draw(resources.TexturePages[i], new Vector2(Entity.Position.X, Entity.Position.Y + y), Color.White);
                y += 1 + resources.TexturePages[i].Height;
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

            var room = new Scene(384, 216);
            var resources = room.AddSceneComponent(new SpriteResources(64, 64)) as SpriteResources;
            resources.AddAseSprite("Checker", "Content/Sprites/checker.ase");
            resources.AddAseSprite("ColorBox", "Content/Sprites/animation.ase");
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
            animated_entity.AddComponent(new AnimatedSpriteComponent());
            animated_entity.Position = new Vector2(12, 12);
            room.AddEntity(animated_entity, 0);

            var texturepages_entity = room.CreateEntity(0);
            texturepages_entity.AddComponent(new TexturePagesComponent());
            texturepages_entity.Position = new Vector2(64, 12);
            room.AddEntity(texturepages_entity, 0);
            Scene = room;
        }
    }
}
