using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public abstract class SettingsComponent : Component
    {
        public void SetBool(string key, bool value)
        {
            SetValueWithRecflection(key, value);
        }
        public void SetInt(string key, int value)
        {
            SetValueWithRecflection(key, value);
        }
        public void SetString(string key, string value)
        {
            SetValueWithRecflection(key, value);
        }
        public void SetPoint(string key, int x, int y)
        {
            var v = new Point(x, y);
            SetPoint(key, v);
        }
        public void SetPoint(string key, Point position)
        {
            SetValueWithRecflection(key, position);
        }
        private void SetValueWithRecflection(string key, object value)
        {
            var prop = GetType().GetProperty(key);
            if (prop != null)
            {
                prop.SetValue(this, value);
            }
        }
    }
}
