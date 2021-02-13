using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public class TextWithBackgroundRenderer : TextRenderer
    {
        public Color BackgroundColor { get; set; }
        public int Margin { get; set; }
        public TextWithBackgroundRenderer()
        {
            Margin = 10;
            BackgroundColor = Color.Black;
        }
        public override void Render(Graphics graphics)
        {
            var textDims = TextDimensions;
            var tp = GetTextPosition();
            graphics.DrawRect(new Rectangle((int)tp.X - Margin, (int)tp.Y - Margin, (int)textDims.X + Margin * 2, (int)textDims.Y + Margin * 2), BackgroundColor);
            graphics.DrawText(Font, Text, tp, Color);
        }
    }
}
