using System;
using Engine.Components;
using Veldrid;

namespace Engine.Buffers
{
    public abstract class Buffer : IBufferResource, IDisposable
    {
        protected DeviceBuffer _buffer;

        public string Name
        {
            get => _buffer.Name;
            set => _buffer.Name = value;
        }

        public DeviceBuffer DeviceBuffer => _buffer;

        public bool Initialized => _buffer != null;

        public void Dispose()
        {
            _buffer?.Dispose();
            _buffer = null;
        }

        public abstract void Initialize(ResourceFactory factory, GraphicsDevice device);
    }
}
