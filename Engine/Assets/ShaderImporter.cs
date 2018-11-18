using System.IO;

namespace Engine.Assets
{
    public class ShaderImporter : IAssetImporter<byte[]>
    {
        public static ShaderImporter Instance = new ShaderImporter();

        public byte[] Import(Stream assetData)
        {
            using (var stream = new MemoryStream())
            {
                // TODO: User CopyToAsync when we need "Loading..." screen
                assetData.CopyTo(stream);
                return stream.ToArray();
            }
        }
    }
}
