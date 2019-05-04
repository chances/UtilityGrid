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
        public FrontFace FrontFace { get; protected set; }
        public VertexBuffer VertexBuffer { get; protected set; }
        public IndexBuffer Indices => VertexBuffer.Indices;

        public MeshData([CanBeNull] string name,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList,
            FrontFace frontFace = FrontFace.Clockwise) : base(name)
        {
            PrimitiveTopology = primitiveTopology;
            FrontFace = frontFace;
        }
    }

    public class MeshData<T> : MeshData, IResource where T : struct, IVertexBufferDescription
    {
        public bool Initialized => VertexBuffer.Initialized;

        public MeshData(string name,
            VertexBuffer<T> vertexBuffer,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList,
            FrontFace frontFace = FrontFace.Clockwise) : base(name)
        {
            VertexBuffer = vertexBuffer;
            PrimitiveTopology = primitiveTopology;
            FrontFace = frontFace;
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
