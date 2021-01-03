using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public class AseSpriteAtlas : SpriteAtlas
    {
        AseSprite _asesprite;
        public AseSpriteAtlas(AseSprite ase)
        {
            Texture = Texture;
            _asesprite = ase;
            Build(_asesprite);
        }
        void Build(AseSprite ase)
        {
            for (var i = 0; i < ase.Frames.Count; i++)
            {
                if (i == 0)
                {
                    var texture = new Texture2D(Engine.Instance.Graphics.Device, ase.Width, ase.Height);
                    texture.SetData<Color>(ase.Frames[i].Pixels);
                    Texture = texture;
                    for (var j = 0; j < ase.Slices.Count; j++)
                    {
                        var slice = ase.Slices[j];
                        if (slice.Frame == i)
                        {
                            var userdata = (slice.UserDataText != null ? slice.UserDataText.Split('_') : null);
                            if (userdata!= null && userdata[0] == "tileset")
                            {
                                var w = int.Parse(userdata[1]);
                                var h = int.Parse(userdata[2]);
                                AddTilesetRegion(slice.Name, new Rectangle(slice.OriginX, slice.OriginY, slice.Width, slice.Height), w, h);
                            }
                            else
                            {
                                var pivot = slice.Pivot.GetValueOrDefault(Vector2.Zero);
                                AddSpriteRegion(slice.Name, new Rectangle(slice.OriginX, slice.OriginY, slice.Width, slice.Height), pivot);
                            }
                        }
                    }
                }
            }
        }       
    }

    public partial class RuntimeContentManager
    {
        public SpriteAtlas LoadASESpriteSheet(string name)
        {
            if (_loadedAssets.TryGetValue(name, out var asset))
            {
                if (asset is SpriteAtlas s)
                {
                    return s;
                }
            }
            var sheet = new AseSpriteAtlas(new AseSprite(name, false));
            _loadedAssets[name] = sheet;
            _disposableAssets.Add(sheet);
            return sheet;
        }
    }
}
