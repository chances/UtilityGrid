using System.Numerics;
using Veldrid;

namespace Engine.Buffers.Layouts
{
    public struct VertexPositionNormalTexture : IVertexBufferDescription
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoordinates;

        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 texCoordinates)
        {
            Position = position;
            Normal = normal;
            TexCoordinates = texCoordinates;
        }

        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position), VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Normal), VertexElementSemantic.Normal, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(TexCoordinates), VertexElementSemantic.TextureCoordinate,
                VertexElementFormat.Float2));

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        // float is 4 bytes(?), 4*8=32
        public uint SizeInBytes => 32;
    }
}
