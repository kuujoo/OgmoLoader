using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public partial class PixelContentManager : IDisposable
	{
		Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();
		List<IDisposable> _disposableAssets = new List<IDisposable>();
		public PixelContentManager()
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
		public void UnloadAsset<T>(string assetName) where T : class, IDisposable
		{
			if (_loadedAssets.ContainsKey(assetName))
			{
				try
				{
					// first fetch the actual asset. we already know its loaded so we'll grab it directly
					var assetToRemove = _loadedAssets[assetName];
					for (var i = 0; i < _disposableAssets.Count; i++)
					{
						// see if the asset is disposeable. If so, find and dispose of it.
						var typedAsset = _disposableAssets[i] as T;
						if (typedAsset != null && typedAsset == assetToRemove)
						{
							typedAsset.Dispose();
							_disposableAssets.RemoveAt(i);
							break;
						}
					}

					_loadedAssets.Remove(assetName);
				}
				catch (Exception)
				{
				}
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