using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Buffers;
using Engine.Components;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;

namespace Engine.Primitives
{
    public class MeshBuilder
    {
        private IVertexBufferDescription[] _vertices;
        private ushort[] _indices = new ushort[0];
        private readonly RgbaFloat? _color;

        public MeshBuilder(RgbaFloat? color = null)
        {
            _vertices = null;
            _color = color;
        }

        public MeshBuilder WithVertex([NotNull] IVertexBufferDescription vertex)
        {
            Guard.AgainstNullArgument(nameof(vertex), vertex);

            return new MeshBuilder(_color)
            {
                _vertices = _vertices.Append(vertex).ToArray(),
                _indices = _indices
            };
        }

        public MeshBuilder WithVertices([NotNull] IEnumerable<IVertexBufferDescription> vertices)
        {
            var verticesArray = vertices as IVertexBufferDescription[] ?? vertices.ToArray();
            Guard.AgainstNullArgument(nameof(vertices), verticesArray);
            if (verticesArray.Length == 0)
            {
                throw new ArgumentException("Given vertex data must not be empty.", nameof(vertices));
            }

            return new MeshBuilder(_color)
            {
                _vertices = _vertices,
                _indices = _indices
            };
        }

        /// <summary>
        /// Instantiates a <see cref="MeshData{T}"/> with stored vertices and indices
        /// </summary>
        /// <param name="name">Name to give the instantiated <see cref="Engine.ECS.Component"/></param>
        /// <returns>Guaranteed to return an instance of <see cref="MeshData{T}"/></returns>
        /// <exception cref="InvalidOperationException">Zero vertices are stored</exception>
        public object Build(string name)
        {
            if (_vertices == null || _vertices.Length == 0)
            {
                throw new InvalidOperationException("This builder contains zero vertices");
            }

            if (_color == null)
                return new MeshData<VertexPositionNormal>(name,
                    new VertexBuffer<VertexPositionNormal>(_vertices, _indices));

            return new MeshData<VertexPositionNormalColor>(name,
                new VertexBuffer<VertexPositionNormalColor>(_vertices, _indices));
        }
    }
}
