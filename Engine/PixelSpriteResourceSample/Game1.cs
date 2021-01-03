using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{
    public class SpriteEntity : Entity
    {
        Sprite _sprite;
        public SpriteEntity()
        {
        }
        public override void Initialize()
        {
            _sprite = Scene.GetSceneComponent<SpriteResources>().GetSprite(Game1.SheetName, "Circle");
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            graphics.DrawSprite(Position, _sprite, Color.White);
        }
    }
    public class Game1 : Engine
    {
        public static string SheetName = "Content/Sprites/sheet.ase";
        public Game1() : base()
        {

        }
        protected override void Initialize()
        {
            base.Initialize();

            var room = new Scene(384, 216);
            room.AddSceneComponent(new SpriteResources());
            room.AddCamera(new Camera(384, 216)
            {
                BackgroundColor = Color.Aquamarine
            });

            room.CreateEntityLayer(0, "entities");
            // Entity & Component - preferred
            var entity = room.CreateEntity(0);
            entity.AddComponent(new SpriteComponent() 
            {
                Sprite = room.GetSceneComponent<SpriteResources>().GetSprite(Game1.SheetName, "Checker")
            });
            entity.Position = new Vector2(50, 50);
            // Subclassed entity, fast and easy
            var entity2 = room.AddEntity(0, new SpriteEntity());
            entity2.Position = new Vector2(100, 100);

            Scene = room;
        }
    }
}
