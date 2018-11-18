using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Components;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;

namespace Engine.Buffers
{
    public class VertexBuffer<T> : Buffer, IResource where T : struct, IVertexBufferDescription
    {
        private readonly T[] _vertexData;

        public VertexBuffer([NotNull] IEnumerable<T> vertexData)
        {
            var vertexDataArray = vertexData as T[] ?? vertexData.ToArray();
            Guard.AgainstNullArgument(nameof(vertexData), vertexDataArray);
            if (vertexDataArray.Length == 0)
            {
                throw new ArgumentException("Given vertex data must not be empty.", nameof(vertexData));
            }
            _vertexData = vertexDataArray;
        }

        public VertexLayoutDescription LayoutDescription => _vertexData[0].LayoutDescription;

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            var size = (uint) (_vertexData.Length * _vertexData[0].SizeInBytes);
            _buffer = factory.CreateBuffer(new BufferDescription(size, BufferUsage.VertexBuffer));
            device.UpdateBuffer(_buffer, 0, _vertexData);
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
