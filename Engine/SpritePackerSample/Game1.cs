﻿using Microsoft.Xna.Framework;
using System.Collections;

namespace kuujoo.Pixel
{

    public class TexturePagesRenderer : Component, IRenderable
    {
        public int Layer { get; set; }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }
        public void Render(Graphics graphics)
        {
            var resources = Scene.GetSceneComponent<RuntimeSpriteLibrary>();
            int y = 0;
            int x = 0;
            for (var i = 0; i < resources.TexturePages.Length; i++)
            { 
                x = (i % 4) * 280;
                if (i != 0 && i % 4 == 0)
                {
                    y += 280;
                }
                graphics.DrawTexture(resources.TexturePages[i].Texture, new Vector2(Entity.Transform.Position.X + x, Entity.Transform.Position.Y + y));
            }
        }
        public void DebugRender(Graphics graphics)
        {

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
            Screen.SetSize(1920, 1080);
            var room = new Scene(1920, 1080);
            string[] sprites = { "Content/Sprites" };
            var resources = room.AddSceneComponent(new RuntimeSpriteLibrary(256, 256, sprites));
            var cameraEntity = room.CreateEntity();
            var camera = cameraEntity.AddComponent(room.Get<Camera>());

            camera.SetViewport(1920, 1080);
            camera.SetSize(1920, 1080);
            camera.BackgroundColor = Color.Aquamarine;

            room.AddCamera(camera);
            var texturepages_entity = room.CreateEntity();
            texturepages_entity.AddComponent(room.Get<TexturePagesRenderer>());
            texturepages_entity.Transform.SetPosition(12, 12);
            room.AddEntity(texturepages_entity);
            Scene = room;
        }
    }
}
