using kuujoo.Pixel;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    [Buildable]
    public class TestEntity : Entity
    {
        public override void Initialize()
        {
            base.Initialize();
            AddComponent(new SpriteComponent()
            {
                Sprite = Scene.GetSceneComponent<SpriteResources>().GetSprite("TestEntity")
            });
            var settings = GetComponent<OgmoSettingsComponent>();
            Position = settings.Position;
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
            var room = new Scene(384 * 2 , 216);
            room.AddCamera(new Camera(384 * 2, 216));
            var resources = room.AddSceneComponent(new SpriteResources(1024, 1024)) as SpriteResources;
            resources.AddAseSprite("tiles", "Content/Sprites/tiles.ase");
            resources.AddAseSprite("TestEntity", "Content/Sprites/TestEntity.ase");
            resources.Build();
            resources.InitTileset("tiles", 12, 12);
            OgmoSceneBuilder builder = new OgmoSceneBuilder(room, "Content/levels/levels");
            builder.AddTileset("tiles", resources.GetTileset("tiles"));
            builder.Build();
            Scene = room;
        }
    }
}
