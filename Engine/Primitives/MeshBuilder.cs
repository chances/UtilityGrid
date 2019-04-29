using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Engine.Buffers;
using Engine.Buffers.Layouts;
using Engine.Components;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;

namespace Engine.Primitives
{
    public class MeshBuilder
    {
        private IVertexBufferDescription[] _vertices = new IVertexBufferDescription[0];
        private ushort[] _indices = new ushort[0];
        PrimitiveTopology _primitiveTopology = PrimitiveTopology.TriangleList;
        FrontFace _frontFace = FrontFace.Clockwise;

        public MeshBuilder WithVertex([NotNull] IVertexBufferDescription vertex)
        {
            Guard.AgainstNullArgument(nameof(vertex), vertex);

            _vertices = _vertices.Append(vertex).ToArray();
            return this;
        }

        public MeshBuilder WithVertices([NotNull] IEnumerable<IVertexBufferDescription> vertices)
        {
            var verticesArray = vertices as IVertexBufferDescription[] ?? vertices.ToArray();
            Guard.AgainstNullArgument(nameof(vertices), verticesArray);
            if (verticesArray.Length == 0)
            {
                throw new ArgumentException("Given vertex data must not be empty.", nameof(vertices));
            }

            _vertices = verticesArray;
            return this;
        }

        public MeshBuilder WithIndices(ushort[] indices)
        {
            _indices = indices;
            return this;
        }

        public MeshBuilder WithPrimitiveTopology(PrimitiveTopology primitiveTopology)
        {
            _primitiveTopology = primitiveTopology;
            return this;
        }

        public MeshBuilder WithFrontFaceClockwise(bool isClockwise)
        {
            _frontFace = isClockwise ? FrontFace.Clockwise : FrontFace.CounterClockwise;
            return this;
        }

        public static MeshData TexturedUnitQuad(string name)
        {
            return new MeshBuilder().WithVertices(new IVertexBufferDescription[]
            {
                new VertexPositionTexture(new Vector3(-1, 1, 0), Vector2.Zero),
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(-1, -1, 0), Vector2.One),
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(0, 1)),
            }).WithIndices(new ushort[] {0, 1, 2, 2, 1, 3})
            .Build<VertexPositionTexture>(name);
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

            return new MeshData<T>(
                name,
                new VertexBuffer<T>(_vertices, _indices),
                _primitiveTopology, _frontFace
            );
        }
    }
}
