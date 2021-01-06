/**
 * Mostly Cherry picked from the Nez Framework: https://github.com/prime31/Nez
 * ---------------------------------------------------------------------------------
		The MIT License (MIT)

		Copyright (c) 2016 Mike, 2020 Joonas Kuusela 

		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:

		The above copyright notice and this permission notice shall be included in all
		copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
		SOFTWARE.
* ------------------------------------------------------------------------------------
*/

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    class IntIntDictionary
    {
        Dictionary<long, List<Collider>> _store = new Dictionary<long, List<Collider>>();
        long GetKey(int x, int y)
        {
            return unchecked((long)x << 32 | (uint)y);
        }
        public void Add(int x, int y, List<Collider> list)
        {
            _store.Add(GetKey(x, y), list);
        }
        public void Remove(Collider obj)
        {
            foreach (var list in _store.Values)
            {
                if (list.Contains(obj))
                {
                    list.Remove(obj);
                }
            }
        }
        public bool TryGetValue(int x, int y, out List<Collider> list)
        {
            return _store.TryGetValue(GetKey(x, y), out list);
        }
        public HashSet<Collider> GetAllObjects()
        {
            var set = new HashSet<Collider>();

            foreach (var list in _store.Values)
            {
                set.UnionWith(list);
            }

            return set;
        }
        public void Clear()
        {
            _store.Clear();
        }
    }
    public class SpatialHash
    {
        public Rectangle Bounds = new Rectangle();
        int _cellSize;
        float _inverseCellSize;
        IntIntDictionary _cells = new IntIntDictionary();
        List<Collider> _tmpList = new List<Collider>(100);
        public SpatialHash(int cellsize = 100)
        {
            _cellSize = cellsize;
            _inverseCellSize = 1.0f / cellsize;
        }
        Point CellAt(int x, int y)
        {
            return new Point((int)(x * _inverseCellSize), (int)(y * _inverseCellSize));
        }
        List<Collider> CellAtPosition(int x, int y, bool createCellIfEmpty = false)
        {
            List<Collider> cell = null;
            if(!_cells.TryGetValue(x, y, out cell))
            {
                if(createCellIfEmpty)
                {
                    cell = new List<Collider>();
                    _cells.Add(x, y, cell);
                }
            }
            return cell;
        }
        public void Register(Collider collider)
        {
            var bounds = collider.Bounds;
            collider.PhysicsBounds = bounds;
            var p1 = CellAt(bounds.X, bounds.Y);
            var p2 = CellAt(bounds.Right, bounds.Bottom);

            if (!Bounds.Contains(p1))
            {
                var r = new Rectangle(p1.X, p1.Y, 0, 0);
                MathExt.UnionRects(ref Bounds, ref r, out Bounds);
            }

            if (!Bounds.Contains(p2))
            {
                var r = new Rectangle(p2.X, p2.Y, 0, 0);
                MathExt.UnionRects(ref Bounds, ref r, out Bounds);
            }

            for (var x = p1.X; x <= p2.X; x++)
            {
                for (var y = p1.Y; y <= p2.Y; y++)
                {
                    var c = CellAtPosition(x, y, true);
                    c.Add(collider);
                }
            }
        }
        public void Unregister(Collider collider)
        {
            var bounds = collider.PhysicsBounds;
            var p1 = CellAt(bounds.X, bounds.Y);
            var p2 = CellAt(bounds.Right, bounds.Bottom);

            for (var x = p1.X; x <= p2.X; x++)
            {
                for (var y = p1.Y; y <= p2.Y; y++)
                {
                    var cell = CellAtPosition(x, y);
                    if (cell != null)
                    {
                        cell.Remove(collider);
                    }
                }
            }
        }
        public List<Collider> Check(ref Rectangle bounds, int mask)
        {
            _tmpList.Clear();
            var p1 = CellAt(bounds.X, bounds.Y);
            var p2 = CellAt(bounds.Right, bounds.Bottom);
            for (var x = p1.X; x <= p2.X; x++)
            {
                for (var y = p1.Y; y <= p2.Y; y++)
                {
                    var cell = CellAtPosition(x, y);
                    if (cell == null)
                    {
                        continue;
                    }

                    for (var i = 0; i < cell.Count; i++)
                    {
                        var collider = cell[i];
                        if (!collider.Enabled || (collider.Mask & mask) == 0)
                        {
                            continue;
                        }
                        if (bounds.Intersects(collider.Bounds))
                        {
                            _tmpList.Add(collider);
                        }
                    }
                }
            }
            return _tmpList;
        }
        public void Clear()
        {
            _cells.Clear();
        }
    }
}
