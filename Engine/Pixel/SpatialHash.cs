// Mostly taken From the NEZ Framework: https://github.com/prime31/Nez by Prime31

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    class IntIntDictionary
    {
        Dictionary<long, List<ColliderComponent>> _store = new Dictionary<long, List<ColliderComponent>>();
        long GetKey(int x, int y)
        {
            return unchecked((long)x << 32 | (uint)y);
        }
        public void Add(int x, int y, List<ColliderComponent> list)
        {
            _store.Add(GetKey(x, y), list);
        }
        public void Remove(ColliderComponent obj)
        {
            foreach (var list in _store.Values)
            {
                if (list.Contains(obj))
                {
                    list.Remove(obj);
                }
            }
        }
        public bool TryGetValue(int x, int y, out List<ColliderComponent> list)
        {
            return _store.TryGetValue(GetKey(x, y), out list);
        }
        public HashSet<ColliderComponent> GetAllObjects()
        {
            var set = new HashSet<ColliderComponent>();

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
        HashSet<ColliderComponent> _tempHashset = new HashSet<ColliderComponent>();
        public SpatialHash(int cellsize = 100)
        {
            _cellSize = cellsize;
            _inverseCellSize = 1.0f / cellsize;
        }
        Point CellAt(int x, int y)
        {
            return new Point((int)(x * _inverseCellSize), (int)(y * _inverseCellSize));
        }
        List<ColliderComponent> CellAtPosition(int x, int y, bool createCellIfEmpty = false)
        {
            List<ColliderComponent> cell = null;
            if(!_cells.TryGetValue(x, y, out cell))
            {
                if(createCellIfEmpty)
                {
                    cell = new List<ColliderComponent>();
                    _cells.Add(x, y, cell);
                }
            }
            return cell;
        }
        public void Register(ColliderComponent collider)
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
        public void Unregister(ColliderComponent collider)
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
        public HashSet<ColliderComponent> Check(ref Rectangle bounds, int mask)
        {
            _tempHashset.Clear();
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
                            _tempHashset.Add(collider);
                        }
                    }
                }
            }
            return _tempHashset;
        }
        public void Clear()
        {
            _cells.Clear();
        }
    }
}
