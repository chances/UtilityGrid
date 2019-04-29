using System;
using Engine.Components;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;

namespace Engine.Buffers
{
    public class IndexBuffer : Buffer, IBufferResource
    {
        private readonly ushort[] _indices;

        public IndexBuffer([NotNull] ushort[] indices)
        {
            Guard.AgainstNullArgument(nameof(indices), indices);
            if (indices.Length == 0)
            {
                throw new ArgumentException("Given index data must not be empty.", nameof(indices));
            }
            _indices = indices;
        }

        public int Count => _indices.Length;

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            var size = (uint) (_indices.Length * sizeof(ushort));
            _buffer = factory.CreateBuffer(new BufferDescription(size, BufferUsage.IndexBuffer));
            device.UpdateBuffer(_buffer, 0, _indices);
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
