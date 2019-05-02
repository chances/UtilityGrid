
using Engine.Components;

namespace Engine.Primitives
{
    // Adapted from THREE.js ExtrusionGeometry
    // https://github.com/mrdoob/three.js/blob/c2adfa442611818973843654ec2a57fd01ef91e3/src/geometries/ExtrudeGeometry.js
    // https://github.com/mrdoob/three.js/issues/10160

    // TODO: Rounded box? https://discourse.threejs.org/t/round-edged-box/1402

    public class Extrusions : IPrimitive
    {
        public Extrusions(int curveSegments, int steps, float depth)
        {
            CurveSegments = curveSegments;
            Steps = steps;
            Depth = depth;
        }

        /// <summary>
        /// Number of points on the curves.
        /// </summary>
        public int CurveSegments { get; set; }

        /// <summary>
        /// Number of points for z-side extrusions.
        /// </summary>
        /// <remarks>
        /// Used for subdividing segments of extrude spline, too.
        /// </remarks>
        public int Steps { get; set; }

        /// <summary>
        /// Depth to extrude the shape.
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Whether bevel is enabled,
        /// </summary>
        public bool IsBevelEnabled { get; set; }

        /// <summary>
        /// How deep into the original shape bevel goes.
        /// </summary>
        public float BevelThickness { get; set; }

        /// <summary>
        /// How far from shape outline (including <see cref="BevelOffset"/>) is bevel.
        /// </summary>
        public float BevelSize { get; set; }

        /// <summary>
        /// How far from shape outline does bevel start.
        /// </summary>
        public float BevelOffset { get; set; }

        /// <summary>
        /// Number of bevel layers.
        /// </summary>

        public MeshData MeshData => throw new System.NotImplementedException();
    }
}
