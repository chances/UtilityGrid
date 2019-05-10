using System.Numerics;

namespace Engine.Components.Geometry
{
    public class Spatial : Transformation
    {
        public Spatial()
        {
            Name = nameof(Spatial);
        }

        public Vector3 Velocity { get; set; }

        public Quaternion AngularVelocity { get; set; }
    }
}
