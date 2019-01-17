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
        private readonly T[] _vertices;

        public VertexBuffer([NotNull] IVertexBufferDescription[] vertices, [NotNull] ushort[] indices)
        {
            Guard.AgainstNullArgument(nameof(vertices), vertices);
            Guard.AgainstNullArgument(nameof(indices), indices);
            if (vertices.Length == 0)
            {
                throw new ArgumentException("Given vertices must not be empty.", nameof(vertices));
            }

            _vertices = vertices.Cast<T>().ToArray();
            Indices = new IndexBuffer(indices);
        }

        public VertexLayoutDescription LayoutDescription => _vertices[0].LayoutDescription;

        public IndexBuffer Indices { get; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            var size = (uint) (_vertices.Length * _vertices[0].SizeInBytes);
            _buffer = factory.CreateBuffer(new BufferDescription(size, BufferUsage.VertexBuffer));
            device.UpdateBuffer(_buffer, 0, _vertices);

            Indices.Initialize(factory, device);
        }

        public void Dispose()
        {
            _buffer.Dispose();
            Indices.Dispose();
        }
    }
}
