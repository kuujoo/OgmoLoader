using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{       
    public interface IAseUserData
    {
        string UserDataText { get; set; }
        Color UserDataColor { get; set; }
    }
}