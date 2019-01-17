using Veldrid;

namespace Engine.Buffers
{
    public interface IVertexBufferDescription
    {
        VertexLayoutDescription LayoutDescription { get; }
        uint SizeInBytes { get; }
    }
}
