using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class Settings : Component
    {
        public int Layer { get; set; }
        public Action<string, int> IntChanged;
        public Action<string, bool> BoolChanged;
        Dictionary<string, int> _iValues = new Dictionary<string, int>();
        Dictionary<string, bool> _bValues = new Dictionary<string, bool>();
        Dictionary<string, string> _sValues = new Dictionary<string, string>();
        Dictionary<string, string[]> _sArrayValues = new Dictionary<string, string[]>();
        Dictionary<string, Point> _pointValues = new Dictionary<string, Point>();
        Dictionary<string, Vector2> _vectorValues = new Dictionary<string, Vector2>();
        public void SetBool(string key, bool value)
        {
            _bValues[key] = value;
            BoolChanged?.Invoke(key, value);
        }
        public void SetInt(string key, int value)
        {
            _iValues[key] = value;
            IntChanged?.Invoke(key, value);
        }
        public void SetString(string key, string value)
        {
            _sValues[key] = value;
        }
        public void SetArrayString(string key, string[] values)
        {
            _sArrayValues[key] = values;
        }
        public void SetPoint(string key, int x, int y)
        {
            var v = new Point(x, y);
            SetPoint(key, v);
        }
        public void SetPoint(string key, Point point)
        {
            _pointValues[key] = point;
        }
        public void SetVector(string key, Vector2 vector)
        {
            _vectorValues[key] = vector;
        }
        public bool GetBool(string key)
        {
            return _bValues[key];
        }
        public int getInt(string key)
        {
            return _iValues[key];
        }
        public string GetString(string key)
        {
            if (_sValues.ContainsKey(key))
            {
                return _sValues[key];
            }
            else
            {
                return null;
            }
        }
        public string[] GetArrayString(string key)
        {
            if (_sArrayValues.ContainsKey(key))
            {
                return _sArrayValues[key];
            }
            else
            {
                return null;
            }
        }
        public Point GetPoint(string key)
        {
            return _pointValues[key];
        }
    }
}
