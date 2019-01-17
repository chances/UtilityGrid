using Engine.Components;
using Veldrid;

namespace Engine.Buffers
{
    public class UniformBuffer<T> : Buffer, IResource where T : struct, IUniformBufferDescription
    {
        private GraphicsDevice _device;

        public UniformBuffer(T uniformData)
        {
            UniformData = uniformData;
        }

        public T UniformData { private get; set; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            _buffer = factory.CreateBuffer(
                new BufferDescription(UniformData.SizeInBytes, BufferUsage.UniformBuffer | BufferUsage.Dynamic));
            _device = device;
            device.UpdateBuffer(_buffer, 0, UniformData);
        }

        public void Update()
        {
            _device.UpdateBuffer(_buffer, 0, UniformData);
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
