using Engine.Assets;

namespace Engine.Components
{
    public interface IAsset
    {
        void LoadAssets(AssetDataLoader assetDataLoader);
    }
}
