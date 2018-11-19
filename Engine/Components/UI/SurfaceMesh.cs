using Engine.Buffers;
using Engine.Primitives;

namespace Engine.Components.UI
{
    public static class SurfaceMesh
    {
        public static readonly MeshData<VertexPositionTexture> Instance =
            new MeshBuilder().WithTexturedUnitQuad().Build<VertexPositionTexture>("UI Surface Mesh");
    }
}
