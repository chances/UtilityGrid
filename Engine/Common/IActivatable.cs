using System;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public interface IActivatable
    {
        bool Active { get; set; }

        IObservable<bool> OnActiveStateChange { get; }
    }

    public static class ActivatableExtensions
    {
        public static void Activate([NotNull] this IActivatable activatable)
        {
            Guard.AgainstNullArgument(nameof(activatable), activatable);

            activatable.Active = true;
        }

        public static void Deactivate([NotNull] this IActivatable activatable)
        {
            Guard.AgainstNullArgument(nameof(activatable), activatable);

            activatable.Active = false;
        }
    }
}
