using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public class LdtkSceneBuilder : SceneBuilder
    {
        LdtkWorld _world;
        Rectangle _bounds;
        public LdtkSceneBuilder(Scene scene, string directory) : base(scene)
        {
            var ldtk_levels = Directory.GetFiles(directory, "*.ldtk");
            if (ldtk_levels.Length > 0)
            {
                _world = LdtkWorld.Load(ldtk_levels[0]);
            }
            _bounds = FindBounds(_world);
        }
        Rectangle FindBounds(LdtkWorld world)
        {
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            for (var i = 0; i < world.Levels.Length; i++)
            {
                minx = Math.Min(minx, _world.Levels[i].WorldX);
                maxx = Math.Max(maxx, _world.Levels[i].WorldX + _world.Levels[i].PxWid);
                miny = Math.Min(miny, _world.Levels[i].WorldY);
                maxy = Math.Max(maxy, _world.Levels[i].WorldY + _world.Levels[i].PxHei);
            }
            var w = maxx - minx;
            var h = maxy - miny;

            return new Rectangle(minx, miny, w, h);
        }
        public override void Build()
        {
            var levels = _world.Levels;
            var defs = _world.Defs;
   
            for(var l = 0; l < levels.Length; l++)
            {
                var level = levels[l];
                BeginRoom(level.WorldX, level.WorldY, level.PxWid, level.PxHei);
                for(int i = 0; i < level.LayerInstances.Length; i++)
                {
                    var depth = (level.LayerInstances.Length-1) - i;
                    var layer = level.LayerInstances[i];
                    var layer_define = defs.GetLayerDefineByUid(layer.LayerDefUid);               
                    if(layer_define.Type == LdtkLayerTypes.Tiles)
                    {
                        var tileset_define = defs.GetTilesetsDefineByUid(layer_define.TilesetDefUid.Value);
                        var tileset = GetTileset(tileset_define.Identifier);

                        BeginTileLayer(depth, layer_define.Identifier, _bounds.Width / layer_define.GridSize, _bounds.Height / layer_define.GridSize, _bounds.X, _bounds.Y, tileset);
                        for (var t = 0; t < layer.GridTiles.Length; t++)
                        {
                            var x = (level.WorldX + layer.GridTiles[t].Px[0] - _bounds.X) / layer_define.GridSize;
                            var y = (level.WorldY + layer.GridTiles[t].Px[1] - _bounds.Y) / layer_define.GridSize;
                            var value = layer.GridTiles[t].T;
                            SetTile(x, y, (byte)value);
                        }
                        EndLayer();
                    }
                    else if(layer_define.Type == LdtkLayerTypes.Entities)
                    {
                        BeginEntityLayer(depth, layer_define.Identifier);
                        for (var e = 0; e < layer.EntityInstances.Length; e++)
                        {
                            var entity = layer.EntityInstances[e];
                            var entity_define = defs.GetEntityDefineByUid(entity.DefUid);

                            var settings = new LdtkSettingsComponent();
                            if (settings != null)
                            {
                                settings.SetPoint("Position", new Point(level.WorldX + entity.Px[0], level.WorldY + entity.Px[1]));
                                settings.SetPoint("Pivot", new Point((int)(entity_define.PivotX * entity_define.Width), (int)(entity_define.PivotY * entity_define.Height)) );
                            }
                            CreateEntity(entity_define.Identifier, settings);
                        }
                        EndLayer();
                    }
                }
                EndRoom();
            }
        }
    }
}
