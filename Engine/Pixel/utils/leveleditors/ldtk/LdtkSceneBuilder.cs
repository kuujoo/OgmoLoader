using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public static class LdtkLayerTypes
    {
        public static string Entities = "Entities";
        public static string Tiles = "Tiles";

    }
    public class LdtkSceneBuilder : SceneBuilder
    {
        ldtk.LdtkJson _world;
        Rectangle _bounds;
        public LdtkSceneBuilder(Scene scene, string directory) : base(scene)
        {
            var ldtk_levels = Directory.GetFiles(directory, "*.ldtk");
            if (ldtk_levels.Length > 0)
            {
                var json = File.ReadAllText(ldtk_levels[0]);
                _world = ldtk.LdtkJson.FromJson(json);
            }
            _bounds = FindBounds(_world);
        }
        Rectangle FindBounds(ldtk.LdtkJson world)
        {
            long minx = int.MaxValue;
            long maxx = int.MinValue;
            long miny = int.MaxValue;
            long maxy = int.MinValue;
            for (var i = 0; i < world.Levels.Length; i++)
            {
                minx = Math.Min(minx, _world.Levels[i].WorldX);
                maxx = Math.Max(maxx, _world.Levels[i].WorldX + _world.Levels[i].PxWid);
                miny = Math.Min(miny, _world.Levels[i].WorldY);
                maxy = Math.Max(maxy, _world.Levels[i].WorldY + _world.Levels[i].PxHei);

                AddRoomBounds((int)_world.Levels[i].WorldX, (int)_world.Levels[i].WorldY, (int)_world.Levels[i].PxWid, (int)_world.Levels[i].PxHei);
            }
            var w = maxx - minx;
            var h = maxy - miny;

            return new Rectangle((int)minx, (int)miny, (int)w, (int)h);
        }
        public override void BuildRoomAt(int x, int y)
        {
            var levels = _world.Levels;
            for (var l = 0; l < levels.Length; l++)
            {
                var level = levels[l];
                var r = new Rectangle((int)level.WorldX, (int)level.WorldY, (int)level.PxWid, (int)level.PxHei);
                if (r.Contains(x, y))
                {
                    BuildRoom(level);
                }
            }
        }
        void BuildRoom(ldtk.Level level)
        {
            var defs = _world.Defs;
            BeginRoom((int)level.WorldX, (int)level.WorldY, (int)level.PxWid, (int)level.PxHei);
            BuildTiles(level);
            BuildEntities(level); 
            EndRoom();
        }
        void BuildTiles(ldtk.Level level)
        {
            var defs = _world.Defs;
            for (int i = 0; i < level.LayerInstances.Length; i++)
            {
                var depth = (level.LayerInstances.Length - 1) - i;
                var layer = level.LayerInstances[i];
                if (layer.Type == LdtkLayerTypes.Tiles)
                {                  
                    var tileset = GetTileset(layer.Identifier);
                    BeginTileLayer(depth, layer.Identifier, _bounds.Width / (int)layer.GridSize, _bounds.Height / (int)layer.GridSize, _bounds.X, _bounds.Y, tileset);
                    for (var t = 0; t < layer.GridTiles.Length; t++)
                    {
                        var x = (level.WorldX + layer.GridTiles[t].Px[0] - _bounds.X) / layer.GridSize;
                        var y = (level.WorldY + layer.GridTiles[t].Px[1] - _bounds.Y) / layer.GridSize;
                        var value = layer.GridTiles[t].T;
                        SetTile((int)x, (int)y, (byte)value);
                    }
                    EndLayer();
                }
            }
        }
        void BuildEntities(ldtk.Level level)
        {
            for (int i = 0; i < level.LayerInstances.Length; i++)
            {
                var depth = (level.LayerInstances.Length - 1) - i;
                var layer = level.LayerInstances[i];
                if (layer.Type == LdtkLayerTypes.Entities)
                {
                    BeginEntityLayer(depth, layer.Identifier);
                    for (var e = 0; e < layer.EntityInstances.Length; e++)
                    {
                        var entity = layer.EntityInstances[e];
                        var settings = new Settings();
                        if (settings != null)
                        {
                            settings.SetPoint("Position", new Point((int)(level.WorldX + entity.Px[0]), (int)(level.WorldY + entity.Px[1])));
                            settings.SetVector("Pivot", new Vector2((float)entity.Pivot[0], (float)entity.Pivot[1]));
                        }

                        for (var ii = 0; ii < entity.FieldInstances.Length; ii++)
                        {
                            var field = entity.FieldInstances[ii];
                            SetField(field.Identifier, field.Type, field.Value, ref settings);
                        }
                        CreateEntity(entity.Identifier, settings);

                    }
                    EndLayer();
                }
            }
        }
        public void SetField(string identifier, string type, dynamic value, ref Settings settings)
        {
            if (type == "Bool")
            {
                settings.SetBool(identifier, (bool)value);
            }
            else if(type == "String")
            {
                settings.SetString(identifier, (string)value);
            }
            else if(type == "Array<String>")
            {
                settings.SetArrayString(identifier, (string[])value);
            }
            else if(type.StartsWith("LocalEnum"))
            {
                settings.SetString(identifier, (string)value);
            }
        }
        public override void Build()
        {
            var levels = _world.Levels;   
            for(var l = 0; l < levels.Length; l++)
            {
                var level = levels[l];
                BuildRoom(level);
            }
        }
        public override void BuildTiles()
        {
            var levels = _world.Levels;
            for (var l = 0; l < levels.Length; l++)
            {
                var level = levels[l];
                BuildTiles(level);
            }
        }
        public override void BuildEntitiesInRoomAt(int x, int y)
        {
            var levels = _world.Levels;
            for (var l = 0; l < levels.Length; l++)
            {
                var level = levels[l];
                var r = new Rectangle((int)level.WorldX, (int)level.WorldY, (int)level.PxWid, (int)level.PxHei);
                if (r.Contains(x, y))
                {
                    BuildEntities(level);
                    return;
                }
            }
        }
    }
}
