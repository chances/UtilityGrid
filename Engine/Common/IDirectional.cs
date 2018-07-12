using Godot;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public interface IDirectional
    {
        Vector3 Origin { get; }

        Vector3 Forward { get; }

        Vector3 Up { get; }

        Vector3 Right { get; }
    }

    public static class DirectionalExtensions
    {
        public static Transform GetTransform([NotNull] this IDirectional directional)
        {
            Guard.AgainstNullArgument(nameof(directional), directional);

            var basis = BasisExtensions.CreateFromAxes(directional.Right, directional.Up, directional.Forward * -1);

            return new Transform(basis, directional.Origin);
        }
    }
}
