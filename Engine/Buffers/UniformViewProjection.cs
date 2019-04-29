using System.Numerics;
using System.Runtime.CompilerServices;
using Veldrid;

namespace Engine.Buffers
{
    public class UniformViewProjection : IUniformBufferDescription<Matrix4x4>
    {
        public UniformBuffer<Matrix4x4> Buffer { get; private set; }

        public UniformViewProjection()
        {
            Buffer = new UniformBuffer<Matrix4x4>();
        }

        public UniformViewProjection(Matrix4x4 viewProj)
        {
            Buffer = new UniformBuffer<Matrix4x4>(viewProj);
        }

        public ResourceLayoutElementDescription LayoutDescription => UniformViewProjection.ResourceLayout;

        public static ResourceLayoutElementDescription ResourceLayout =>
            new ResourceLayoutElementDescription("ViewProj", ResourceKind.UniformBuffer, ShaderStages.Vertex);
    }
}
