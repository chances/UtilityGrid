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
        public Cube(string name)
        {
            // var cube = Solids.Cube(1, true);

            // // TODO: Extract this to a MeshBuilder static method, From(Solid solid)
            // var vertices = cube.Polygons.SelectMany(polygon =>
            //     polygon.Vertices.Select(vertex =>
            //         new Vector3((float) vertex.Pos.X, (float) vertex.Pos.Y, (float) vertex.Pos.Z)
            //     )).ToArray();

            // var normals = cube.Polygons.SelectMany(polygon => {
            //     var faceNormal = polygon.Plane.Normal;
            //     var normal = new Vector3(
            //         (float) faceNormal.X,
            //         (float) faceNormal.Y,
            //         (float) faceNormal.Z
            //     );

            //     return polygon.Vertices.Select(vertex => normal);
            // }).ToArray();

            // var indices = new ushort[cube.Polygons.Count * 6];
            // for (int i = 0; i < indices.Length; i += 6)
            // {
            //     indices[i + 0] = (ushort) (i + 0);
            //     indices[i + 1] = (ushort) (i + 1);
            //     indices[i + 2] = (ushort) (i + 2);
            //     indices[i + 3] = (ushort) (i + 0);
            //     indices[i + 4] = (ushort) (i + 2);
            //     indices[i + 5] = (ushort) (i + 3);
            // }

            // var builder = new MeshBuilder();
            // for (int i = 0; i < vertices.Length; i++)
            // {
            //     builder.WithVertex(new VertexPositionNormal(vertices[i], normals[i]));
            // }
            // builder.WithIndices(indices);

            // _mesh = builder.Build<VertexPositionNormal>(name);

            var builder = new MeshBuilder();

            builder.WithVertices(vertices).WithFrontFaceClockwise(true);

            builder.WithIndices(new ushort[]
            {
                0,1,2, 0,2,3,
                4,5,6, 4,6,7,
                8,9,10, 8,10,11,
                12,13,14, 12,14,15,
                16,17,18, 16,18,19,
                20,21,22, 20,22,23,
            });

            MeshData = builder.Build<VertexPositionNormal>(name);
        }

        public MeshData MeshData { get; private set; }

        private readonly IVertexBufferDescription[] vertices = new IVertexBufferDescription[]
        {
                // Top
                new VertexPositionNormal(new Vector3(-0.5f, +0.5f, -0.5f), new Vector3(0, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, +0.5f, -0.5f), new Vector3(0.5f, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, +0.5f, +0.5f), new Vector3(0.5f, 0.5f, 0)),
                new VertexPositionNormal(new Vector3(-0.5f, +0.5f, +0.5f), new Vector3(0, 0.5f, 0)),
                // Bottom
                new VertexPositionNormal(new Vector3(-0.5f,-0.5f, +0.5f),  new Vector3(0, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f,-0.5f, +0.5f),  new Vector3(0.5f, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f,-0.5f, -0.5f),  new Vector3(0.5f, 0.5f, 0)),
                new VertexPositionNormal(new Vector3(-0.5f,-0.5f, -0.5f),  new Vector3(0, 0.5f, 0)),
                // Left
                new VertexPositionNormal(new Vector3(-0.5f, +0.5f, -0.5f), new Vector3(0, 0, 0)),
                new VertexPositionNormal(new Vector3(-0.5f, +0.5f, +0.5f), new Vector3(0.5f, 0, 0)),
                new VertexPositionNormal(new Vector3(-0.5f, -0.5f, +0.5f), new Vector3(0.5f, 0.5f, 0)),
                new VertexPositionNormal(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0, 0.5f, 0)),
                // Right
                new VertexPositionNormal(new Vector3(+0.5f, +0.5f, +0.5f), new Vector3(0, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, +0.5f, -0.5f), new Vector3(0.5f, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, -0.5f, +0.5f), new Vector3(0, 0.5f, 0)),
                // Back
                new VertexPositionNormal(new Vector3(+0.5f, +0.5f, -0.5f), new Vector3(0, 0, 0)),
                new VertexPositionNormal(new Vector3(-0.5f, +0.5f, -0.5f), new Vector3(0.5f, 0, 0)),
                new VertexPositionNormal(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, -0.5f, -0.5f), new Vector3(0, 0.5f, 0)),
                // Front
                new VertexPositionNormal(new Vector3(-0.5f, +0.5f, +0.5f), new Vector3(0, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, +0.5f, +0.5f), new Vector3(0.5f, 0, 0)),
                new VertexPositionNormal(new Vector3(+0.5f, -0.5f, +0.5f), new Vector3(0.5f, 0.5f, 0)),
                new VertexPositionNormal(new Vector3(-0.5f, -0.5f, +0.5f), new Vector3(0, 0.5f, 0)),
            };
    }
}
