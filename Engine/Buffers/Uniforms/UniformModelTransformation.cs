using System.Numerics;
using Veldrid;

namespace Engine.Buffers.Uniforms
{
    public class UniformModelTransformation : UniformMatrix, IUniformBufferDescription<Matrix4x4>
    {
        public UniformModelTransformation()
        {
        }

        public UniformModelTransformation(Matrix4x4 value) : base(value)
        {
        }

        public ResourceLayoutElementDescription LayoutDescription => UniformModelTransformation.ResourceLayout;

        public static ResourceLayoutElementDescription ResourceLayout =
            new ResourceLayoutElementDescription("Model", ResourceKind.UniformBuffer, ShaderStages.Vertex);
    }
}
