using Engine.Buffers;
using Veldrid;

namespace Engine.Components
{
    public class MeshData<T> : ECS.Component, IResource where T : struct, IVertexBufferDescription
    {
        public MeshData(string name, VertexBuffer<T> vertexBuffer,
            FrontFace frontFace = FrontFace.Clockwise,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList) : base(name)
        {
            VertexBuffer = vertexBuffer;
            FrontFace = frontFace;
            PrimitiveTopology = primitiveTopology;
        }

        public VertexBuffer<T> VertexBuffer { get; }
        public IndexBuffer Indices => VertexBuffer.Indices;
        public PrimitiveTopology PrimitiveTopology { get; }
        public FrontFace FrontFace { get; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            VertexBuffer.Initialize(factory, device);
            VertexBuffer.Name = $"{Name} VBO";
            Indices.Name = $"{Name} IBO";
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
        }
    }
}
