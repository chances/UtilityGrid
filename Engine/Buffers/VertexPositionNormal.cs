using System.Numerics;
using Veldrid;

namespace Engine.Buffers
{
    public struct VertexPositionNormal : IVertexBufferDescription
    {
        public Vector3 Position;
        public Vector3 Normal;

        public VertexPositionNormal(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position), VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Normal), VertexElementSemantic.Normal, VertexElementFormat.Float3));

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        // float is 4 bytes(?), 4*6=24
        public uint SizeInBytes => 24;
    }
}
