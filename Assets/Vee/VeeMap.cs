using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vee
{
    public class VeeMap<T> where T : class
    {

        public static VeeMap<T> Create(Coordinate mapSize)
        {
            VeeMap<T> map = new VeeMap<T>();
            map.Init(mapSize);
            return map;
        }



        // map size
        public int Width { get { return mapSize.x; } }
        public int Height { get { return mapSize.y; } }
        // tile size
        public float TileWidth { get { return tileSize.x; } }
        public float TileHeight { get { return tileSize.y; } }

        public int Length { get; set; }
        public Vector2 BasePosition { get; set; }
        public Vector2 ContentSize { get; set; }

        public void SetMapSize(Coordinate size)
        {
            mapSize = new Coordinate(size);
            Length = mapSize.x * mapSize.y;
            UpdateContentSize();
        }

        public void SetTileSize(Vector2 size)
        {
            tileSize = size;
            UpdateContentSize();
        }


        public int Grid2Index(Coordinate p)
        {
            if (!LawGrid(p)) return -1;
            return p.ToIDX(Width);
        }

        public Coordinate Index2Grid (int idx)
        {
            return new Coordinate(idx % Width, idx / Width);
        }

        public Vector2 Grid2Position (Coordinate p)
        {
            return new Vector2(BasePosition.x + (p.x + 0.5f) * TileWidth, BasePosition.y + (p.y + 0.5f) * TileHeight);
        }

        public Coordinate Position2Grid(Vector2 p)
        {
            var gridX = Mathf.FloorToInt((p.x - BasePosition.x) / TileWidth);
            var gridY = Mathf.FloorToInt((p.y - BasePosition.y) / TileHeight);

            Coordinate retGrid = new Coordinate(gridX, gridY);
            if (LawGrid(retGrid))
            {
                return retGrid;
            }
            else
            {
                return null;
            }
        }

        public bool LawGrid(Coordinate p)
        {
            if (p == null)
                return false;
            if (p.x < 0 || p.y < 0 || p.x >= Width || p.y >= Height)
            {
                return false;
            }
            return true;
        }

        public Coordinate ForceLawGrid (Coordinate p)
        {
            var x = Mathf.Max(0, Mathf.Min(Width - 1, p.x));
            var y = Mathf.Max(0, Mathf.Min(Height - 1, p.y));
            return new Coordinate((int)x, (int)y);
        }

        public void SetObject(T obj, Coordinate p)
        {
            if (p == null) return;
            var idx = Grid2Index(p);

            SetObjectByIndex(obj, idx);
        }

        public void SetObjectByIndex(T obj, int index)
        {
            if (index >= 0)
            {
                _objs[index] = obj;
            }
        }

        public T GetObjectByIndex(int index)
        {
            if (_objs.ContainsKey(index))
            {
                return _objs[index];
            }
            else
            {
                SetObjectByIndex(null, index);
                return null;
            }
        }

        public T GetObject(Coordinate p)
        {
            if (p == null) return null;

            return GetObjectByIndex(Grid2Index(p));
        }

        public void RemoveObject(Coordinate p)
        {
            SetObject(null, p);
        }

        public List<T> GetObjects(bool ignoreNull = false)
        {
            if (ignoreNull) {
                List<T> result = new List<T> ();
                foreach (var obj in _objs.Values) {
                    if (obj != null) {
                        result.Add(obj);
                    }
                }
                return result;
            } else {
                return new List<T>(_objs.Values);
            }
        }

        public void ForEachObject(Callback<T> callback)
        {
            foreach (var obj in _objs.Values)
            {
                if (obj != null)
                    callback(obj);
            }
        }

        public void RemoveAllObjects()
        {
            _objs.Clear();
        }



        public Coordinate GridByDirection (Coordinate p, int dir)
        {
            var newGrid = p + Direction2D.Direction2PointInTiled(dir);
            if (LawGrid(newGrid))
            {
                return newGrid;
            }
            else
            {
                return ForceLawGrid(newGrid);
            }
        }





        Coordinate mapSize = new Coordinate(10, 10);
        Vector2 tileSize = new Vector2(1f, 1f);
        Dictionary<int, T> _objs = null;
        void Init(Coordinate size)
        {
            tileSize = new Vector2(1f, 1f);
            SetMapSize(size);
            //UpdateContentSize();
            _objs = new Dictionary<int, T>();

            BasePosition = Vector2.zero;
        }
        void UpdateContentSize()
        {
            ContentSize = new Vector2(mapSize.x * tileSize.x, mapSize.y * tileSize.y);
        }
    }
}