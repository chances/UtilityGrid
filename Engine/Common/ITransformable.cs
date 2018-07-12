using Godot;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public interface ITransformable
    {
        Spatial Spatial { get; }
    }

    public static class TransformableExtentions
    {
        public static Transform Transform([NotNull] this ITransformable transformable)
        {
            Guard.AgainstNullArgument(nameof(transformable), transformable);

            return transformable.Spatial.Transform;
        }

        public static Transform GlobalTransform([NotNull] this ITransformable transformable)
        {
            Guard.AgainstNullArgument(nameof(transformable), transformable);

            return transformable.Spatial.GlobalTransform;
        }

        public static Vector3 Origin([NotNull] this ITransformable transformable)
        {
            Guard.AgainstNullArgument(nameof(transformable), transformable);

            return transformable.Spatial.GlobalTransform.origin;
        }

        public static float DistanceTo([NotNull] this ITransformable transformable, [NotNull] ITransformable target)
        {
            Guard.AgainstNullArgument(nameof(transformable), transformable);
            Guard.AgainstNullArgument(nameof(target), target);

            return Origin(transformable).DistanceTo(Origin(target));
        }
    }
}
