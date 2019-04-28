using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Engine.Buffers;
using Engine.Buffers.Layouts;
using Engine.Components;
using JetBrains.Annotations;
using LiteGuard;

namespace Engine.Primitives
{
    public class MeshBuilder
    {
        private IVertexBufferDescription[] _vertices;
        private ushort[] _indices = new ushort[0];

        public MeshBuilder()
        {
            _vertices = null;
        }

        public MeshBuilder WithVertex([NotNull] IVertexBufferDescription vertex)
        {
            Guard.AgainstNullArgument(nameof(vertex), vertex);

            return new MeshBuilder()
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

            return new MeshBuilder()
            {
                _vertices = verticesArray,
                _indices = _indices
            };
        }

        public MeshBuilder WithIndices(ushort[] indices)
        {
            return new MeshBuilder()
            {
                _vertices = _vertices,
                _indices = indices
            };
        }

        public MeshBuilder WithTexturedUnitQuad()
        {
            return WithVertices(new IVertexBufferDescription[]
            {
                new VertexPositionTexture(new Vector3(-1, -1, 0), Vector2.Zero),
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(1, 1, 0), Vector2.One),
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 1)),
            }).WithIndices(new ushort[] {0, 1, 2, 0, 2, 3});
        }

        /// <summary>
        /// Instantiates a <see cref="MeshData{T}"/> with stored vertices and indices
        /// </summary>
        /// <param name="name">Name to give the instantiated <see cref="Engine.ECS.Component"/></param>
        /// <returns>Guaranteed to return an instance of <see cref="MeshData{T}"/></returns>
        /// <exception cref="InvalidOperationException">Zero vertices are stored</exception>
        public MeshData<T> Build<T>(string name) where T : struct, IVertexBufferDescription
        {
            if (_vertices == null || _vertices.Length == 0)
            {
                throw new InvalidOperationException("This builder contains zero vertices");
            }

            return new MeshData<T>(name, new VertexBuffer<T>(_vertices, _indices));
        }
    }
}
