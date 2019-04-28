using System.Numerics;
using Engine.ECS;

namespace Engine.Components.Geometry
{
    public class Translation : Component
    {
        public Translation() : base(nameof(Translation))
        {
            Value = Vector3.Zero;
        }

        public Vector3 Value { get; set; }
    }
}
