using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public partial class RuntimeContentManager : IDisposable
	{
		Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();
		List<IDisposable> _disposableAssets = new List<IDisposable>();
		public RuntimeContentManager()
		{
		}
		public Texture2D LoadTexture(string name)
		{
			if (_loadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is Texture2D tex)
					return tex;
			}
			using (var stream = Path.IsPathRooted(name) ? File.OpenRead(name) : TitleContainer.OpenStream(name))
			{
				var texture = Texture2D.FromStream(Engine.Instance.Graphics.Device, stream);
				texture.Name = name;
				_loadedAssets[name] = texture;
				_disposableAssets.Add(texture);

				return texture;
			}
		}
		void Dispose(bool dispose)
        {
			if(dispose)
            {
				for (var i = _disposableAssets.Count - 1; i >= 0; i--)
				{
					_disposableAssets[i].Dispose();
				}
				_disposableAssets.Clear();
				_loadedAssets.Clear();
			}
        }
        public void Dispose()
        {
			Dispose(true);
			GC.SuppressFinalize(this);
        }
    }
}