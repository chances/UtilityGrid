using System.Numerics;
using Veldrid;

namespace Engine.ECS.Components
{
    public class MeshData : Component, IResource
    {
        public MeshData(FrontFace frontFace = FrontFace.Clockwise,
            PrimitiveTopology primitiveTopology = PrimitiveTopology.TriangleList)
        {
            FrontFace = frontFace;
            PrimitiveTopology = primitiveTopology;
        }

        public DeviceBuffer VertexBuffer { get; }
        public DeviceBuffer IndexBuffer { get; }
        public PrimitiveTopology PrimitiveTopology { get; }
        public FrontFace FrontFace { get; }

        public void Initialize(ResourceFactory factory)
        {
            throw new global::System.NotImplementedException();
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
        }
    }

    public struct VertexPositionNormal
    {
        public Vector3 Position;
        public Vector3 Normal;

        public const uint SizeInBytes = 26;
    }
}
