using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public class OgmoSceneBuilder : SceneBuilder
    {
        List<OgmoLevel> _levels = new List<OgmoLevel>();
        Rectangle _bounds;
        public OgmoSceneBuilder(Scene scene, ITilesetProvider tilesetprovider, string directory) : base(scene, tilesetprovider)
        {
            var ogmo_levels = Directory.GetFiles(directory, "*.json");
            if (ogmo_levels.Length > 0)
            {
                for (var i = 0; i < ogmo_levels.Length; i++)
                {
                    var level = OgmoLevel.LoadLevel(ogmo_levels[i]);
                    _levels.Add(level);
                }
                _bounds = FindBounds(_levels);
            }
        }
        Rectangle FindBounds(List<OgmoLevel> levels)
        {
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            for (var i = 0; i < levels.Count; i++)
            {
                minx = Math.Min(minx, levels[i].OffsetX);
                maxx = Math.Max(maxx, levels[i].OffsetX + _levels[i].Width);
                miny = Math.Min(miny, levels[i].OffsetY);
                maxy = Math.Max(maxy, levels[i].OffsetY + _levels[i].Height);
            }
            var w = maxx - minx;
            var h = maxy - miny;

            return new Rectangle(minx, miny, w, h);
        }
        bool VerifyLayers(List<OgmoLevel> levels)
        {
            var reference = levels[0];
            for (var i = 0; i < levels.Count; i++)
            {
                if (reference.Layers.Length != levels[i].Layers.Length) return false;
                for (var j = 0; j < reference.Layers.Length; j++)
                {
                    if (reference.Layers[j].Type != levels[i].Layers[j].Type) return false;
                    if (reference.Layers[j].Tileset != levels[i].Layers[j].Tileset) return false;
                    if (reference.Layers[j].ArrayMode != levels[i].Layers[j].ArrayMode) return false;
                    if (reference.Layers[j].Name != levels[i].Layers[j].Name) return false;
                }
            }
            return true;
        }
        public override void Build()
        {
            if (!VerifyLayers(_levels)) return;
            var resources = _scene.GetSceneComponent<SpriteResources>();
            var sp = resources.GetSprite("Content/Sprites/tiles.ase", "tiles");
            for (var i = 0; i < _levels.Count; i++)
            {
                for (var j = 0; j < _levels[i].Layers.Length; j++)
                {

                    var layer = _levels[i].Layers[j];
                    if (layer.Type == OgmoLayerType.Tile)
                    {
                        var w = _bounds.Width / layer.GridCellWidth;
                        var h = _bounds.Height / layer.GridCellHeight;
                        var x = _levels[i].OffsetX / layer.GridCellWidth;
                        var y = _levels[i].OffsetY / layer.GridCellHeight;
                        var tileset = GetTileset(layer.Tileset);
                        BeginTileLayer(j, layer.Name, w, h, tileset);
                        for (var t = 0; t < layer.Data.Length; t++)
                        {
                            var tile = layer.Data[t];
                            if (tile > 0)
                            {
                                var xx = t % layer.GridCellsX;
                                var yy = t / layer.GridCellsX;
                                SetTile(x + xx, y + yy, (byte)tile);
                            }
                        }
                        EndLayer();
                    }
                    else if(layer.Type == OgmoLayerType.Entity)
                    {
                        // TODO
                    }
                }
            }
        }
    }
}
