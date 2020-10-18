using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace kuujoo.Pixel
{
    public abstract class OgmoValueContainer
    {
        public Dictionary<string, JsonElement> Values { get; set; }
        public int GetIntValue(string name)
        {
            return Values[name].GetInt32();
        }
        public string GetStringValue(string name)
        {
            return Values[name].GetString();
        }
    }
}
