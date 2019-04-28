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
            _mesh = new MeshBuilder()
            .WithVertices(
                Solids.Cube(1, true).Polygons.Select(polygon => {
                    var normal = polygon.Plane.Normal;
                    return polygon.Vertices.Select(vertex => new VertexPositionNormal(
                        new Vector3((float) vertex.Pos.X, (float) vertex.Pos.Y, (float) vertex.Pos.Z),
                        new Vector3((float) normal.X, (float) normal.Y, (float) normal.Z)
                    ));
                }).OfType<IVertexBufferDescription>()
            ).Build<VertexPositionNormal>(name);
        }

        public MeshData MeshData => _mesh;
    }
}
