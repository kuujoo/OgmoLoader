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

                AddRoomBounds(_world.Levels[i].WorldX, _world.Levels[i].WorldY, _world.Levels[i].PxWid, _world.Levels[i].PxHei);
            }
            var w = maxx - minx;
            var h = maxy - miny;

            return new Rectangle(minx, miny, w, h);
        }
        public override void BuildRoomAt(int x, int y)
        {
            var levels = _world.Levels;
            for (var l = 0; l < levels.Length; l++)
            {
                var level = levels[l];
                var r = new Rectangle(level.WorldX, level.WorldY, level.PxWid, level.PxHei);
                if (r.Contains(x, y))
                {
                    BuildRoom(level);
                }
            }
        }
        void BuildRoom(LdtkLevel level)
        {
            var defs = _world.Defs;
            BeginRoom(level.WorldX, level.WorldY, level.PxWid, level.PxHei);
            BuildTiles(level);
            BuildEntities(level); 
            EndRoom();
        }
        void BuildTiles(LdtkLevel level)
        {
            var defs = _world.Defs;
            for (int i = 0; i < level.LayerInstances.Length; i++)
            {
                var depth = (level.LayerInstances.Length - 1) - i;
                var layer = level.LayerInstances[i];
                if (layer.__type == LdtkLayerTypes.Tiles)
                {
                    var tileset_define = defs.GetTilesetsDefineByUid(layer.__tilesetDefUid.Value);
                    var tileset = GetTileset(tileset_define.Identifier);

                    BeginTileLayer(depth, layer.__identifier, _bounds.Width / layer.__gridSize, _bounds.Height / layer.__gridSize, _bounds.X, _bounds.Y, tileset);
                    for (var t = 0; t < layer.GridTiles.Length; t++)
                    {
                        var x = (level.WorldX + layer.GridTiles[t].Px[0] - _bounds.X) / layer.__gridSize;
                        var y = (level.WorldY + layer.GridTiles[t].Px[1] - _bounds.Y) / layer.__gridSize;
                        var value = layer.GridTiles[t].T;
                        SetTile(x, y, (byte)value);
                    }
                    EndLayer();
                }
            }
        }
        void BuildEntities(LdtkLevel level)
        {
            var defs = _world.Defs;
            for (int i = 0; i < level.LayerInstances.Length; i++)
            {
                var depth = (level.LayerInstances.Length - 1) - i;
                var layer = level.LayerInstances[i];
                if (layer.__type == LdtkLayerTypes.Entities)
                {
                    BeginEntityLayer(depth, layer.__identifier);
                    for (var e = 0; e < layer.EntityInstances.Length; e++)
                    {
                        var entity = layer.EntityInstances[e];
                        var entity_define = defs.GetEntityDefineByUid(entity.DefUid);
                        var settings = new Settings();
                        if (settings != null)
                        {
                            settings.SetPoint("Position", new Point(level.WorldX + entity.Px[0], level.WorldY + entity.Px[1]));
                            settings.SetPoint("Pivot", new Point((int)(entity_define.PivotX * entity_define.Width), (int)(entity_define.PivotY * entity_define.Height)));
                        }

                        for (var ii = 0; ii < entity.FieldInstances.Length; ii++)
                        {
                            var field = entity.FieldInstances[ii];
                            SetField(field.__identifier, field.__type, field.__value, ref settings);
                        }
                        CreateEntity(entity.__identifier, settings);

                    }
                    EndLayer();
                }
            }
        }
        public void SetField(string identifier, string type, dynamic value, ref Settings settings)
        {
            var jsonElement = (System.Text.Json.JsonElement)value;
            if (type == "Bool")
            {
                bool bvalue = jsonElement.GetBoolean();
                settings.SetBool(identifier, bvalue);
            }
            else if(type == "String")
            {
                string svalue = jsonElement.GetString();
                settings.SetString(identifier, svalue);
            }
            else if(type == "Array<String>")
            {
                string[] svalues = new string[jsonElement.GetArrayLength()];
                for(var i = 0; i < jsonElement.GetArrayLength(); i++)
                {
                    svalues[i] = jsonElement[i].GetString();
                }
                settings.SetArrayString(identifier, svalues);
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
                var r = new Rectangle(level.WorldX, level.WorldY, level.PxWid, level.PxHei);
                if (r.Contains(x, y))
                {
                    BuildEntities(level);
                    return;
                }
            }
        }
    }
}
