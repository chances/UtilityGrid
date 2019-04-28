using System.Numerics;
using Engine.ECS;

namespace Engine.Components.Geometry
{
    public class Rotation : Component
    {
        public Rotation() : base(nameof(Rotation))
        {
            Value = Quaternion.Identity;
        }

        public Quaternion Value { get; set; }
    }
}
