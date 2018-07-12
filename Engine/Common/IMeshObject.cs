using System.Collections.Generic;
using System.Linq;
using Godot;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public interface IMeshObject : IBounded, IHideable
    {
        IEnumerable<MeshInstance> Meshes { get; }

        Material Material { get; set; }
    }

    public static class MeshObjectExtensions
    {
        public static void SetMaterial([NotNull] this IMeshObject source, [NotNull] Material material)
        {
            Guard.AgainstNullArgument(nameof(source), source);

            foreach (var meshInstance in source.Meshes)
            {
                meshInstance.SetMaterial(material);
            }
        }

        public static AABB CalculateBounds([NotNull] this IMeshObject source)
        {
            Guard.AgainstNullArgument(nameof(source), source);

            return source.Meshes.Select(m => m.GetAabb()).Aggregate((b1, b2) => b1.Merge(b2));
        }
    }
}
