using Veldrid;

namespace Engine.Buffers
{
    public interface IUniformBufferDescription<T> where T : struct
    {
        UniformBuffer<T> Buffer { get; }
        ResourceLayoutElementDescription LayoutDescription { get; }
    }
}
