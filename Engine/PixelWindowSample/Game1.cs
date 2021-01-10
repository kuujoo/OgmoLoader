using kuujoo.Pixel;
using Microsoft.Xna.Framework;
using System;
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
            var cameraEntity = room.CreateEntity(0);
            var camera = cameraEntity.AddComponent(new Camera(384, 216));
            camera.BackgroundColor = Color.Aquamarine;
            room.AddCamera(camera);
            Scene = room;
        }
    }
}
