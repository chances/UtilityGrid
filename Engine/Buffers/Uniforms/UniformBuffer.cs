using System.Runtime.CompilerServices;
using Engine.Components;
using Veldrid;

namespace Engine.Buffers.Uniforms
{
    public class UniformBuffer<T> : Buffer where T : struct
    {
        private GraphicsDevice _device;
        private T _uniformData;

        public UniformBuffer()
        {
            UniformData = new T();
        }

        public UniformBuffer(T uniformData)
        {
            UniformData = uniformData;
        }

        public T UniformData
        {
            private get => _uniformData;
            set
            {
                _uniformData = value;
                if (Initialized)
                {
                    Update();
                }
            }
        }

        public override void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            _buffer = factory.CreateBuffer(
                new BufferDescription((uint)Unsafe.SizeOf<T>(), BufferUsage.UniformBuffer | BufferUsage.Dynamic));
            _device = device;
            Update();
        }

        private void Update()
        {
            _device.UpdateBuffer(_buffer, 0, UniformData);
        }
    }
}
