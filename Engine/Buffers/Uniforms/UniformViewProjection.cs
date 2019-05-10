using System.Numerics;
using System.Runtime.CompilerServices;
using Veldrid;

namespace Engine.Buffers.Uniforms
{
    public class UniformViewProjection : UniformMatrix, IUniformBufferDescription<Matrix4x4>
    {
        public UniformViewProjection()
        {
        }

        public UniformViewProjection(Matrix4x4 value) : base(value)
        {
        }

        public ResourceLayoutElementDescription LayoutDescription => UniformViewProjection.ResourceLayout;

        public static ResourceLayoutElementDescription ResourceLayout =
            new ResourceLayoutElementDescription("ViewProj", ResourceKind.UniformBuffer, ShaderStages.Vertex);
    }
}
