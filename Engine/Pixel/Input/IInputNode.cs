namespace kuujoo.Pixel
{
    public interface IInputNode
    {
        public void Update(IInputState state);
        public bool Down { get; }
        public bool Pressed { get; }
        public bool Released { get; }
    }
}
