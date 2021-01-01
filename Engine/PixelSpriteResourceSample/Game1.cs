﻿using Microsoft.Xna.Framework;
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
    public class SpriteComponent : Component
    {
        Sprite _sprite;

        public SpriteComponent()
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            _sprite = Entity.Scene.GetSceneComponent<SpriteResources>().GetSprite(Game1.SheetName, "Checker");
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            graphics.DrawSprite(Entity.Position, _sprite, Color.White);
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
            room.AddRenderer(new DefaultRenderer());

            // Entity & Component - preferred
            var entity = room.CreateEntity();
            entity.AddComponent(new SpriteComponent());
            entity.Position = new Microsoft.Xna.Framework.Vector2(50, 50);


            // Subclassed entity, fast and easy
            var entity2 = room.AddEntity(new SpriteEntity());
            entity2.Position = new Microsoft.Xna.Framework.Vector2(100, 100);
            Scene = room;
        }
    }
}