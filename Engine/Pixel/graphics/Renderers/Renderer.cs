namespace kuujoo.Pixel
{
    public abstract class Renderer
    {
        public Surface Surface { get; set; }
        public virtual void BeginRender(Scene scene)
        {
        }
        public abstract void Render(Scene scene);
        public virtual void EndRender(Scene scene)
        {
        }
        protected Graphics GetGraphics()
        {
            return Engine.Instance.Graphics;
        }
    }
}
