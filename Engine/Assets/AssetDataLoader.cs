using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using LiteGuard;

namespace Engine.Assets
{
    public class AssetDataLoader
    {
        private readonly Assembly _gameAssembly;
        private readonly Dictionary<AssetType, string> _assetDirectoryPaths;
        private readonly string[] _assetFilenames;

        public AssetDataLoader(Assembly gameAssembly, Dictionary<AssetType, string> assetDirectoryPaths)
        {
            _gameAssembly = gameAssembly;
            _assetDirectoryPaths = assetDirectoryPaths;
            _assetFilenames = gameAssembly.GetManifestResourceNames();
        }

        /// <summary>
        /// Load an asset from the asset library given a <paramref name="type"/> and a <paramref name="filename"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filename"></param>
        /// <returns><see cref="Stream"/> of loaded asset's data</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filename"/> is null</exception>
        /// <exception cref="InvalidOperationException">Asset type doesn't exist in asset directory dictionary</exception>
        /// <exception cref="FileNotFoundException">Given <paramref name="filename"/> doesn't exist in asset library</exception>
        public Stream Load(AssetType type, [NotNull] string filename)
        {
            Guard.AgainstNullArgument(nameof(filename), filename);
            if (!_assetDirectoryPaths.ContainsKey(type))
            {
                throw new InvalidOperationException($"{type} does not exist in the asset directory path dictionary");
            }

            var assetFilePath = $"{_assetDirectoryPaths[type]}.{filename}";

            if (!_assetFilenames.Contains(assetFilePath))
            {
                throw new FileNotFoundException($"{filename} not found in asset library");
            }

            return _gameAssembly.GetManifestResourceStream(assetFilePath);
        }
    }
}
