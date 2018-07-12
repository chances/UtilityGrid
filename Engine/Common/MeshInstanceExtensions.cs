using Godot;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public static class MeshInstanceExtensions
    {
        public static Material GetMaterial(this MeshInstance source)
        {
            Guard.AgainstNullArgument(nameof(source), source);

            return source.GetSurfaceMaterial(0);
        }

        public static void SetMaterial([NotNull] this MeshInstance source, Material material)
        {
            Guard.AgainstNullArgument(nameof(source), source);

            source.SetSurfaceMaterial(0, material);
        }
    }
}
