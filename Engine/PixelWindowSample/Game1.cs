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
            room.AddCamera(new Camera(384, 216));
            Scene = room;
        }
    }
}
