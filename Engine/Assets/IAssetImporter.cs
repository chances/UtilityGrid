using System.IO;

namespace Engine.Assets
{
    public interface IAssetImporter<out T>
    {
        T Import(Stream assetData);
    }
}
