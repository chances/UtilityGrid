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

        public static MeshData FromSolid(Csg.Solid solid, string name)
        {
            var vertices = solid.Polygons
                .SelectMany(polygon => polygon.Vertices)
                .Select(vertex =>
                    new Vector3((float) vertex.Pos.X, (float) vertex.Pos.Y, (float) vertex.Pos.Z)
                ).ToArray();

            var normals = solid.Polygons.SelectMany(polygon => {
                var faceNormal = polygon.Plane.Normal;
                var normal = new Vector3(
                    (float) faceNormal.X,
                    (float) faceNormal.Y,
                    (float) faceNormal.Z
                );

                return polygon.Vertices.Select(_ => normal);
            }).ToArray();

            var vi = 0;
            var indices = solid.Polygons.SelectMany(face => {
                var faceIndices = new List<ushort>();
                for (var v = 2; v < face.Vertices.Count; v++)
                {
                    faceIndices.Add((ushort) (vi));
                    faceIndices.Add((ushort) (vi + v - 1));
                    faceIndices.Add((ushort) (vi + v));
                }
                vi += face.Vertices.Count;
                return faceIndices;
            }).ToArray();

            var builder = new MeshBuilder().WithFrontFaceClockwise(false);
            for (int i = 0; i < vertices.Length; i++)
            {
                builder.WithVertex(new VertexPositionNormal(vertices[i], normals[i]));
            }
            builder.WithIndices(indices);

            return builder.Build<VertexPositionNormal>(name);
        }
    }
}
