using Engine.Buffers;
using Engine.ECS.Components;
using Veldrid;

namespace Engine.Components
{
    public class MeshData<T> : ECS.Component, IResource where T : struct, IVertexBufferDescription
    {
        public MeshData(string name, VertexBuffer<T> vertexBuffer, IndexBuffer indexBuffer,
            FrontFace frontFace = FrontFace.Clockwise,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList) : base(name)
        {
            VertexBuffer = vertexBuffer;
            IndexBuffer = indexBuffer;
            FrontFace = frontFace;
            PrimitiveTopology = primitiveTopology;
        }

        public VertexBuffer<T> VertexBuffer { get; }
        public IndexBuffer IndexBuffer { get; }
        public PrimitiveTopology PrimitiveTopology { get; }
        public FrontFace FrontFace { get; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            VertexBuffer.Initialize(factory, device);
            VertexBuffer.Name = $"{Name} VBO";
            IndexBuffer.Initialize(factory, device);
            IndexBuffer.Name = $"{Name} IBO";
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
        }
    }
}
