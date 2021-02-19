using kuujoo.Pixel;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public enum HorizontalAlign
    {
        Left,
        Right,
        Center
    }
    public enum VerticalALign
    {
        Top,
        Bottom,
        Center
    }
    public class Label : MenuComponent, IRenderable
    {
        public override Vector2 Size => _size;
        public Color Color { get; set; }
        public BMFont Font { get; set; }
        public string Text { get; private set; }
        public int Layer { get; set; }
        Vector2 _size = Vector2.Zero;
        public Label()
        {
            Layer = 0;
            Text = "";
            HorizontalAlign = HorizontalAlign.Left;
            VerticalAlign = VerticalALign.Top;
            Font = null;
            Color = Color.White;
        }
        public void DebugRender(Graphics graphics)
        {
        }
        public void SetText(string text)
        {
            _size = Font.MeasureString(text);
            Text = text;
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return camera.Bounds.Intersects(new Rectangle(GetRenderPosition().ToPoint(), Size.ToPoint()));
        }
        public virtual void Render(Graphics graphics)
        {
            graphics.DrawText(Font, Text, GetRenderPosition(), Color);
        }
    }
}
