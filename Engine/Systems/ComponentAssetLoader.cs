using Engine.Assets;
using Engine.Components;
using Engine.ECS;

namespace Engine.Systems
{
    public class ComponentAssetLoader : System<IAsset>
    {
        private readonly AssetDataLoader _assetDataLoader;

        public ComponentAssetLoader(World world, AssetDataLoader assetDataLoader) : base(world)
        {
            _assetDataLoader = assetDataLoader;
        }

        public override void Operate()
        {
            foreach (var asset in OperableComponents)
            {
                asset.LoadAssets(_assetDataLoader);
            }
        }
    }
}
