using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Assimp;
using Assimp.Configs;
using Engine.Buffers;
using Engine.Components;
using Matrix4x4 = System.Numerics.Matrix4x4;

namespace Engine.Assets
{
    public class ModelImporter : IAssetImporter<MeshData<VertexPositionNormal>>
    {
        private static readonly PostProcessSteps PostProcessSteps =
            PostProcessSteps.FlipWindingOrder | PostProcessSteps.ImproveCacheLocality |
            PostProcessSteps.JoinIdenticalVertices | PostProcessSteps.CalculateTangentSpace |
            PostProcessSteps.SortByPrimitiveType;

        private Scene _scene;
        private Vector3 _sceneCenter, _sceneMin, _sceneMax;

        public MeshData<VertexPositionNormal> Import(Stream assetData)
        {
            var importer = new AssimpContext();
            importer.SetConfig(new SortByPrimitiveTypeConfig(PrimitiveType.Polygon | PrimitiveType.Line));
            _scene = importer.ImportFileFromStream(assetData, PostProcessSteps);
            ComputeBoundingBox();

            var vertices = new List<IVertexBufferDescription>();
            var indices = new List<ushort>();

            foreach (var mesh in _scene.Meshes)
            {
                vertices.AddRange(mesh.Vertices.Select((vertex, i) =>
                    new VertexPositionNormal(FromVector(vertex), FromVector(mesh.Normals[i])) as
                        IVertexBufferDescription));
                indices.AddRange(mesh.GetUnsignedIndices().Cast<ushort>());
            }

            return new MeshData<VertexPositionNormal>(_scene.RootNode.Name, new VertexBuffer<VertexPositionNormal>(
                vertices.ToArray(),
                indices.ToArray()));
        }

        private void ComputeBoundingBox()
        {
            _sceneMin = new Vector3(1e10f, 1e10f, 1e10f);
            _sceneMax = new Vector3(-1e10f, -1e10f, -1e10f);
            var identity = Matrix4x4.Identity;

            ComputeBoundingBox(_scene.RootNode, ref _sceneMin, ref _sceneMax, ref identity);

            _sceneCenter.X = (_sceneMin.X + _sceneMax.X) / 2.0f;
            _sceneCenter.Y = (_sceneMin.Y + _sceneMax.Y) / 2.0f;
            _sceneCenter.Z = (_sceneMin.Z + _sceneMax.Z) / 2.0f;
        }

        private void ComputeBoundingBox(Node node, ref Vector3 min, ref Vector3 max, ref Matrix4x4 trafo)
        {
            var prev = trafo;
            trafo = Matrix4x4.Multiply(prev, FromMatrix(node.Transform));

            if(node.HasMeshes)
            {
                foreach(var index in node.MeshIndices)
                {
                    var mesh = _scene.Meshes[index];
                    for(var i = 0; i < mesh.VertexCount; i++)
                    {
                        var tmp = FromVector(mesh.Vertices[i]);
                        tmp = Vector3.Transform(tmp, trafo);

                        min.X = Math.Min(min.X, tmp.X);
                        min.Y = Math.Min(min.Y, tmp.Y);
                        min.Z = Math.Min(min.Z, tmp.Z);

                        max.X = Math.Max(max.X, tmp.X);
                        max.Y = Math.Max(max.Y, tmp.Y);
                        max.Z = Math.Max(max.Z, tmp.Z);
                    }
                }
            }

            for(var i = 0; i < node.ChildCount; i++)
            {
                ComputeBoundingBox(node.Children[i], ref min, ref max, ref trafo);
            }
            trafo = prev;
        }

        private static Matrix4x4 FromMatrix(Assimp.Matrix4x4 mat)
        {
            return new Matrix4x4
            {
                M11 = mat.A1,
                M12 = mat.A2,
                M13 = mat.A3,
                M14 = mat.A4,
                M21 = mat.B1,
                M22 = mat.B2,
                M23 = mat.B3,
                M24 = mat.B4,
                M31 = mat.C1,
                M32 = mat.C2,
                M33 = mat.C3,
                M34 = mat.C4,
                M41 = mat.D1,
                M42 = mat.D2,
                M43 = mat.D3,
                M44 = mat.D4
            };
        }

        private static Vector3 FromVector(Vector3D vec) => new Vector3(vec.X, vec.Y, vec.Z);
    }
}
