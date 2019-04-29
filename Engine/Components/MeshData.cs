using System.Numerics;
using Engine.Buffers;
using Engine.ECS;
using JetBrains.Annotations;
using Veldrid;
using Veldrid.Utilities;

namespace Engine.Components
{
    public class MeshData : Component
    {
        public Vector3 Center { get; }
        public BoundingBox BoundingBox { get; }
        public PrimitiveTopology PrimitiveTopology { get; protected set; }
        public FrontFace FrontFace { get; }
        public VertexBuffer VertexBuffer { get; protected set; }
        public IndexBuffer Indices => VertexBuffer.Indices;

        public MeshData([CanBeNull] string name,
            FrontFace frontFace = FrontFace.Clockwise,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList) : base(name)
        {
            FrontFace = frontFace;
            PrimitiveTopology = primitiveTopology;
        }
    }

    public class MeshData<T> : MeshData, IBufferResource where T : struct, IVertexBufferDescription
    {
        public MeshData(string name,
            VertexBuffer<T> vertexBuffer,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList) : base(name)
        {
            VertexBuffer = vertexBuffer;
            PrimitiveTopology = primitiveTopology;
        }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            (VertexBuffer as VertexBuffer<T>).Initialize(factory, device);
            VertexBuffer.Name = $"{Name} VBO";
            Indices.Name = $"{Name} IBO";
        }

        public void Dispose()
        {
            (VertexBuffer as VertexBuffer<T>).Dispose();
        }
    }
}
