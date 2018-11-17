using System.Numerics;
using Veldrid;

namespace Engine.Buffers
{
    public struct VertexPositionNormalColor : IVertexBufferDescription
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 Color;

        public VertexPositionNormalColor(Vector3 position, Vector3 normal, Vector4 color)
        {
            Position = position;
            Normal = normal;
            Color = color;
        }

        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position), VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Normal), VertexElementSemantic.Normal, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Color), VertexElementSemantic.Color, VertexElementFormat.Float4));

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        // float is 4 bytes(?)
        public uint SizeInBytes => 40;
    }
}
