namespace kuujoo.Pixel
{
    public interface IGrid
    {
        public int Width { get; }
        public int Height { get; }
        byte GetValue(int x, int y);
        byte GetValueByIndex(int idx);
        void SetValue(int x, int y, byte value);
        void SetValueByIndex(int index, byte value);
    }

    public class ByteGrid : IGrid
    {
        byte[] _grid;
        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ByteGrid(int width, int height, int cellw, int cellh)
        {
            Width = width;
            Height = height;
            CellWidth = cellw;
            CellHeight = cellh;
            _grid = new byte[Width * Height];
        }
        public byte GetValue(int x, int y)
        {
            return GetValueByIndex(y * Width + x);
        }
        public byte GetValueByIndex(int idx)
        {
            return _grid[idx];
        }
        public void SetValue(int x, int y, byte value)
        {
            SetValueByIndex(y * Width + x, value);
        }
        public void SetValueByIndex(int index, byte value)
        {
            _grid[index] = value;
        }
    }
}