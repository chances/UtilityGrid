using Godot;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public interface IBounded : ITransformable
    {
        AABB Bounds { get; }
    }

    public static class BoundedExtensions
    {
        public static Vector3 Center([NotNull] this IBounded bounded)
        {
            Guard.AgainstNullArgument(nameof(bounded), bounded);

            var bounds = bounded.Bounds;

            return bounded.Spatial.GlobalTransform.origin + (bounds.Position + bounds.End) / 2f;
        }
    }
}
