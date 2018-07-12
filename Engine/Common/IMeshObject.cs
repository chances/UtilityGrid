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
    }

    public static class MeshObjectExtensions
    {
        public static AABB CalculateBounds([NotNull] this IMeshObject source)
        {
            Guard.AgainstNullArgument(nameof(source), source);

            return source.Meshes.Select(m => m.GetAabb()).Aggregate((b1, b2) => b1.Merge(b2));
        }
    }
}
