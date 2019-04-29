using System.Runtime.CompilerServices;
using Engine.Components;
using Veldrid;

namespace Engine.Buffers
{
    public class UniformBuffer<T> : Buffer, IBufferResource where T : struct
    {
        private GraphicsDevice _device;

        public UniformBuffer()
        {
            UniformData = new T();
        }

        public UniformBuffer(T uniformData)
        {
            UniformData = uniformData;
        }

        public T UniformData { private get; set; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            _buffer = factory.CreateBuffer(
                new BufferDescription((uint) Unsafe.SizeOf<T>(), BufferUsage.UniformBuffer | BufferUsage.Dynamic));
            _device = device;
            device.UpdateBuffer(_buffer, 0, UniformData);
        }

        public void Update()
        {
            _device.UpdateBuffer(_buffer, 0, UniformData);
        }

        public void Update(T uniformData)
        {
            UniformData = uniformData;
            Update();
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
