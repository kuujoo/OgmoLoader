namespace kuujoo.Pixel
{
    public class SceneComponent
    {
        public Scene Scene { get; set; }
        public virtual void Initialize()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void Destroy() { }
        public virtual void RemovedFromScene() { }
    }
}
