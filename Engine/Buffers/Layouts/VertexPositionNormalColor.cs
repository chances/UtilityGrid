using System.Numerics;
using Veldrid;

namespace Engine.Buffers.Layouts
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

        public static VertexPositionNormalColor FromVertexPositionNormal(VertexPositionNormal vertex) =>
            new VertexPositionNormalColor(vertex.Position, vertex.Normal, RgbaFloat.Grey.ToVector4());

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        // float is 4 bytes(?)
        public uint SizeInBytes => 40;

        // ReSharper disable once InconsistentNaming
        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position), VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Normal), VertexElementSemantic.Normal, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Color), VertexElementSemantic.Color, VertexElementFormat.Float4));
    }
}
