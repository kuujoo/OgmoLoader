namespace kuujoo.Pixel
{
    public interface IRenderable
    {
        int Depth { get; }
        bool IsVisibleFromCamera(Camera camera);
        void Render(Graphics graphics);
        void DebugRender(Graphics graphics);
    }
}
