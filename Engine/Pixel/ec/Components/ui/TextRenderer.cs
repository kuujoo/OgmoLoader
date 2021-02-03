using kuujoo.Pixel;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public enum HorizontalTextAlign
    {
        Left,
        Right,
        Center
    }
    public enum VerticalTextAlign
    {
        Top,
        Bottom,
        Center
    }
    public class TextRenderer : Component, IRenderable
    {
        public Rectangle Bounds => new Rectangle(GetTextPosition().ToPoint(), _textDimensions.ToPoint());
        public Color Color { get; set; }
        public BMFont Font { get; set; }
        public string Text { get; private set; }
        public int Layer { get; set; }
        public HorizontalTextAlign HorizontalAlign { get; set; }
        public VerticalTextAlign VerticalAlign { get; set; }

        Vector2 _textDimensions;
        public TextRenderer()
        {
            Layer = 0;
            Text = "";
            HorizontalAlign = HorizontalTextAlign.Left;
            VerticalAlign = VerticalTextAlign.Top;
            Font = null;
            Color = Color.White;
        }
        public void DebugRender(Graphics graphics)
        {
        }
        public void SetText(string text)
        {
            _textDimensions = Font.MeasureString(text);
            Text = text;
        }

        public bool IsVisibleFromCamera(Camera camera)
        {
            if(Font == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        int GetHorizontalPosition()
        {
            switch (HorizontalAlign)
            {
                case HorizontalTextAlign.Left:
                    {
                        return Entity.Transform.Position.X;
                    }
                case HorizontalTextAlign.Right:
                    {
                        return (int)(Entity.Transform.Position.X - _textDimensions.X);
                    }
                case HorizontalTextAlign.Center:
                    {
                        return (int)(Entity.Transform.Position.X - _textDimensions.X / 2.0f);
                    }
            }
            return 0;
        }
        int GetVerticalPosition()
        {
            switch (VerticalAlign)
            {
                case VerticalTextAlign.Top:
                    {
                        return Entity.Transform.Position.Y;
                    }
                case VerticalTextAlign.Bottom:
                    {
                        return (int)(Entity.Transform.Position.Y - _textDimensions.Y);
                    }
                case VerticalTextAlign.Center:
                    {
                        return (int)(Entity.Transform.Position.Y - _textDimensions.Y / 2.0f);
                    }
            }
            return 0;
        }
        Vector2 GetTextPosition()
        {
            return new Vector2(GetHorizontalPosition(), GetVerticalPosition());
        }
        public void Render(Graphics graphics)
        {
            graphics.DrawText(Font, Text, GetTextPosition(), Color);
        }
    }
}
