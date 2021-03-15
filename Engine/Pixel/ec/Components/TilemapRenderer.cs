using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class TilemapRenderer : Component, IRenderable
    {
        public int Layer { get; set; }
        public static byte EmptyTile = 0;
        public int Width => Grid.Width;
        public int Height => Grid.Height;
        public Tileset Tileset { get; set; }
        public ByteGrid Grid { get; set; }
        public TilemapRenderer()
        {

        }
        public void Set(ByteGrid grid, Tileset tileset)
        {
            Grid = grid;
            Tileset = tileset;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void CleanUp()
        {
            Grid = null;
            Tileset = null;
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return Grid != null && Tileset != null;
        }

        public void Render(Graphics graphics)
        {
            if (Tileset == null) return;
            if (Grid == null) return;
            var gfx = Engine.Instance.Graphics;
            var bounds = graphics.Camera.Bounds;
            bounds.Location -= Entity.Transform.Position;
            int left = Math.Clamp( (int)Math.Floor((float)bounds.Left / Tileset.TileWidth), 0, Width - 1);
            int right = Math.Clamp( (int)Math.Floor((float)bounds.Right / Tileset.TileWidth), 0, Width - 1);
            int top = Math.Clamp((int)Math.Floor((float)bounds.Top / Tileset.TileHeight), 0, Height - 1);
            int bottom = Math.Clamp((int)Math.Floor((float)bounds.Bottom / Tileset.TileHeight), 0, Height - 1);
            for (var j = top; j <= bottom; j++)
            { 
                for (var i = left; i <= right; i++)
                {
                    var tile_id = Grid.GetValue(i, j);
                    if (tile_id != EmptyTile)
                    {
                        var tile = Tileset.GetTile(tile_id);
                        gfx.SpriteBatch.Draw(tile.Texture, Entity.Transform.Position.ToVector2() + new Vector2(i * Tileset.TileWidth, j * Tileset.TileHeight), tile.Bounds, Color.White);
                    }
                }
            }
        }
        public void DebugRender(Graphics graphics)
        {
        }
    }
}
