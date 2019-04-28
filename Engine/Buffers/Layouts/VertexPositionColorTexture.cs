using System.Numerics;
using Veldrid;

namespace Engine.Buffers.Layouts
{
    public struct VertexPositionColorTexture : IVertexBufferDescription
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector2 TexCoordinates;

        public VertexPositionColorTexture(Vector3 position, RgbaFloat color, Vector2 texCoordinates)
        {
            Position = position;
            Color = color.ToVector4();
            TexCoordinates = texCoordinates;
        }

        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position),
                VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Color),
                VertexElementSemantic.Color, VertexElementFormat.Float4),
            new VertexElementDescription(nameof(TexCoordinates),
                VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2)
        );

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        // float is 4 bytes(?), 4*9=36
        public uint SizeInBytes => 36;
    }
}
