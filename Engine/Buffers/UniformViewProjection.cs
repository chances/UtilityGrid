using System.Numerics;
using System.Runtime.CompilerServices;
using Veldrid;

namespace Engine.Buffers
{
    public struct UniformViewProjection : IUniformBufferDescription
    {
        public Matrix4x4 ViewProj;

        public UniformViewProjection(Matrix4x4 viewProj)
        {
            ViewProj = viewProj;
        }

        public ResourceLayoutElementDescription LayoutDescription =>
            new ResourceLayoutElementDescription("ProjView", ResourceKind.UniformBuffer, ShaderStages.Vertex);

        public uint SizeInBytes => (uint) Unsafe.SizeOf<Matrix4x4>();
    }
}
