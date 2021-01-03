using kuujoo.Pixel;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
            var room = new Scene(384 * 2 , 216);
            room.AddCamera(new Camera(384 * 2, 216));
            var resources = room.AddSceneComponent(new SpriteResources()) as SpriteResources;
            OgmoSceneBuilder builder = new OgmoSceneBuilder(room, new SpriteResourcesTilesetProvider(resources, "Content/Sprites/tiles.ase", 12, 12), "Content/levels/levels");
            builder.Build();
            Scene = room;
        }
    }
}
