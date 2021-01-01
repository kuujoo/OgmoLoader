using Microsoft.Xna.Framework;
using kuujoo.Pixel;

namespace kuujoo.Pixel
{
    /*
    public class TilemapRenderer : RenderableComponent
    {
        public override RectangleF Bounds => GetBounds();
        public Tileset Tileset { get; set; }
        public Tilegrid Tilegrid { get; set; }
        public TilemapRenderer()
        {
        }
        RectangleF GetBounds()
        {
            return new RectangleF(Entity.Position, new Vector2(Tileset.CellWidth * Tilegrid.Width, Tileset.CellHeight * Tilegrid.Height));
        }
        public override void Render(Batcher batcher, Camera camera)
        {
            var bounds = camera.Bounds;
            bounds.Location -= Transform.Position;
            var left = MathHelper.Max(Mathf.FastFloorToInt(bounds.Left / Tileset.CellWidth), 0);
            var right = MathHelper.Min(Tilegrid.Width, Mathf.FastCeilToInt(bounds.Right / Tileset.CellHeight));
            var top = MathHelper.Max(Mathf.FastFloorToInt(bounds.Top / Tileset.CellWidth), 0);
            var bottom = MathHelper.Min(Tilegrid.Height, Mathf.FastCeilToInt(bounds.Bottom / Tileset.CellHeight));
            for (var j = top; j < bottom; j++)
            {
                for (var i = left; i < right; i++)
                {
                    var t = Tilegrid.getTileId(i, j);
                    if (t != -1)
                    {
                        var tile = Tileset.GetTile(t);
                        if (tile != null)
                        {

                            batcher.Draw(tile.Texture, new Rectangle((int)Transform.Position.X + i * Tileset.CellWidth, (int)Transform.Position.Y + j * Tileset.CellHeight, Tileset.CellWidth, Tileset.CellHeight), tile.Bounds, Color.White);
                        }
                    }
                }
            }
        }
    }*/
}