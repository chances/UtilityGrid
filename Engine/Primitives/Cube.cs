using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Csg;
using Engine.Buffers;
using Engine.Buffers.Layouts;
using Engine.Components;

namespace Engine.Primitives
{
    public class Cube : IPrimitive
    {
        private MeshData _mesh;

        public Cube(string name)
        {
            var cube = Solids.Cube(1, true);

            // TODO: Extract this to a MeshBuilder static method, From(Solid solid)
            var vertices = cube.Polygons.SelectMany(polygon =>
                polygon.Vertices.Select(vertex =>
                    new Vector3((float) vertex.Pos.X, (float) vertex.Pos.Y, (float) vertex.Pos.Z)
                )).ToArray();

            var normals = cube.Polygons.SelectMany(polygon => {
                var faceNormal = polygon.Plane.Normal;
                var normal = new Vector3(
                    (float) faceNormal.X,
                    (float) faceNormal.Y,
                    (float) faceNormal.Z
                );

                return polygon.Vertices.Select(vertex => normal);
            }).ToArray();

            var indices = new ushort[vertices.Length + 2 * vertices.Length];
            for (int i = 0; i < indices.Length; i += 6)
            {
                indices[i + 0] = (ushort) (i + 0);
                indices[i + 1] = (ushort) (i + 1);
                indices[i + 2] = (ushort) (i + 2);
                indices[i + 3] = (ushort) (i + 2);
                indices[i + 4] = (ushort) (i + 1);
                indices[i + 5] = (ushort) (i + 3);
            }

            var builder = new MeshBuilder();
            for (int i = 0; i < vertices.Length; i++)
            {
                builder.WithVertex(new VertexPositionNormal(vertices[i], normals[i]));
            }
            builder.WithIndices(indices);

            _mesh = builder.Build<VertexPositionNormal>(name);
        }

        public MeshData MeshData => _mesh;
    }
}
