using System.Numerics;
using Engine.Buffers;
using Engine.Buffers.Layouts;
using Engine.Components;

namespace Engine.Primitives
{
    public class Triangle : IPrimitive
    {
        public Triangle(string name)
        {
            var builder = new MeshBuilder()
                .WithVertices(vertices).WithIndices(new ushort[] { 0, 1, 2 });

            MeshData = builder.Build<VertexPositionNormal>(name);
        }
        public MeshData MeshData { get; private set; }

        private readonly IVertexBufferDescription[] vertices = new IVertexBufferDescription[]
        {
            new VertexPositionNormal(new Vector3(-0.5f, -0.5f, -0f), new Vector3(0, 0, 0)),
            new VertexPositionNormal(new Vector3(+0f, +0.5f, -0f), new Vector3(0.5f, 0, 0)),
            new VertexPositionNormal(new Vector3(+0.5f, -0.5f, +0f), new Vector3(0.5f, 0.5f, 0))
        };
    }
}
