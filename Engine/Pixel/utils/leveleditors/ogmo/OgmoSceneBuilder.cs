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
        public OgmoSceneBuilder(Scene scene, string directory) : base(scene)
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

                AddRoomBounds(levels[i].OffsetX, levels[i].OffsetY, levels[i].Width, levels[i].Height);
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
        void BuildRoom(OgmoLevel level)
        {
            BeginRoom(level.OffsetX, level.OffsetY, level.Width, level.Height);
            BuildTiles(level);
            BuildEntities(level);
            EndRoom();
        }
        public void BuildTiles(OgmoLevel level)
        {
            for (var j = 0; j < level.Layers.Length; j++)
            {
                var layer = level.Layers[j];
                if (layer.Type == OgmoLayerType.Tile)
                {
                    var w = _bounds.Width / layer.GridCellWidth;
                    var h = _bounds.Height / layer.GridCellHeight;
                    var x = level.OffsetX / layer.GridCellWidth - _bounds.X;
                    var y = level.OffsetY / layer.GridCellHeight - _bounds.Y;
                    var tileset = GetTileset(layer.Tileset);
                    BeginTileLayer(j, layer.Name, w, h, _bounds.X, _bounds.Y, tileset);
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
            }
        }
        public void BuildEntities(OgmoLevel level)
        {
            for (var j = 0; j < level.Layers.Length; j++)
            {
                var layer = level.Layers[j];
                if (layer.Type == OgmoLayerType.Entity)
                {
                    BeginEntityLayer(j, layer.Name);
                    for (var e = 0; e < layer.Entities.Length; e++)
                    {
                        var settings = new Settings();
                        if (settings != null)
                        {
                            settings.SetPoint("Position", new Point(level.OffsetX + layer.Entities[e].X, level.OffsetY + layer.Entities[e].Y));
                            settings.SetPoint("Origin", new Point(layer.Entities[e].OriginX, layer.Entities[e].OriginY));
                        }
                        CreateEntity(layer.Entities[e].Name, settings);
                    }
                    EndLayer();
                }
            }
        }
        public override void Build()
        {
            if (!VerifyLayers(_levels)) return;
            for (var i = 0; i < _levels.Count; i++)
            {
                BuildRoom(_levels[i]);
            }
        }
        public override void BuildRoomAt(int x, int y)
        {
            for (var i = 0; i < _levels.Count; i++)
            {
                var r = new Rectangle(_levels[i].OffsetX, _levels[i].OffsetY, _levels[i].Width, _levels[i].Height);
                if(r.Contains(x, y))
                {
                    BuildRoom(_levels[i]);
                }
            }
        }
        public override void BuildEntitiesInRoomAt(int x, int y)
        {
            for (var i = 0; i < _levels.Count; i++)
            {
                var r = new Rectangle(_levels[i].OffsetX, _levels[i].OffsetY, _levels[i].Width, _levels[i].Height);
                if (r.Contains(x, y))
                {
                    BuildEntities(_levels[i]);
                }
            }
        }
        public override void BuildTiles()
        {
            if (!VerifyLayers(_levels)) return;
            for (var i = 0; i < _levels.Count; i++)
            {
                BuildTiles(_levels[i]);
            }
        }
    }
}
