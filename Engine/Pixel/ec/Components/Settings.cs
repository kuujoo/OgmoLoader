using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class Settings : Component
    {
        public Action<string, int> IntChanged;
        public Action<string, bool> BoolChanged;
        Dictionary<string, int> _iValues = new Dictionary<string, int>();
        Dictionary<string, bool> _bValues = new Dictionary<string, bool>();
        Dictionary<string, string> _sValues = new Dictionary<string, string>();
        Dictionary<string, string[]> _sArrayValues = new Dictionary<string, string[]>();
        Dictionary<string, Point> _pointValues = new Dictionary<string, Point>();
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
            return _sValues[key];
        }
        public string[] GetArrayString(string key)
        {
            return _sArrayValues[key];
        }
        public Point GetPoint(string key)
        {
            return _pointValues[key];
        }
    }
}
