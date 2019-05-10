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
            var cube = Solids.Cube(1, true).Translate(0, 0.5);

            MeshData = MeshBuilder.FromSolid(cube, name);
        }

        public MeshData MeshData { get; private set; }
    }
}
