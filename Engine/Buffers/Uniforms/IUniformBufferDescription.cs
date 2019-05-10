using Veldrid;

namespace Engine.Buffers.Uniforms
{
    public interface IUniformBufferDescription<T> where T : struct
    {
        UniformBuffer<T> Buffer { get; }
        ResourceLayoutElementDescription LayoutDescription { get; }
    }
}
