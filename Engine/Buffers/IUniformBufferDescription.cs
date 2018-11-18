using Veldrid;

namespace Engine.Buffers
{
    public interface IUniformBufferDescription
    {
        ResourceLayoutElementDescription LayoutDescription { get; }
        uint SizeInBytes { get; }
    }
}
