using System.Numerics;
using System.Runtime.CompilerServices;
using Veldrid;

namespace Engine.Buffers.Uniforms
{
    public class UniformColor : IUniformBufferDescription<RgbaFloat>
    {
        public UniformBuffer<RgbaFloat> Buffer { get; private set; }

        public UniformColor()
        {
            Buffer = new UniformBuffer<RgbaFloat>();
        }

        public UniformColor(RgbaFloat color)
        {
            Buffer = new UniformBuffer<RgbaFloat>(color);
        }

        public ResourceLayoutElementDescription LayoutDescription => UniformColor.ResourceLayout;

        public static ResourceLayoutElementDescription ResourceLayout =>
            new ResourceLayoutElementDescription("Color", ResourceKind.UniformBuffer, ShaderStages.Fragment);
    }
}
