namespace kuujoo.Pixel
{
    public interface IInputNode
    {
        public bool Down { get; }
        public bool Pressed { get; }
        public bool Released { get; }
    }
}
