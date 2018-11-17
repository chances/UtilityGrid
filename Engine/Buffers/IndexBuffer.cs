using System;
using Engine.ECS.Components;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;

namespace Engine.Buffers
{
    public class IndexBuffer : IResource
    {
        private readonly ushort[] _indexData;
        private DeviceBuffer _buffer;

        public IndexBuffer([NotNull] ushort[] indexData)
        {
            Guard.AgainstNullArgument(nameof(indexData), indexData);
            if (indexData.Length == 0)
            {
                throw new ArgumentException("Given index data must not be empty.", nameof(indexData));
            }
            _indexData = indexData;
        }

        public string Name
        {
            get => _buffer.Name;
            set => _buffer.Name = value;
        }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            var size = (uint) (_indexData.Length * sizeof(ushort));
            _buffer = factory.CreateBuffer(new BufferDescription(size, BufferUsage.IndexBuffer));
            device.UpdateBuffer(_buffer, 0, _indexData);
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
