using System;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public interface IDisposableCollector
    {
        void Collect([NotNull] IDisposable disposable);
    }

    public static class DisposableExtensions
    {
        private const string NodeName = "DisposableCollector";

        [NotNull]
        public static T AddTo<T>([NotNull] this T disposable, [NotNull] Godot.Node node)
            where T : IDisposable
        {
            if (disposable == null)
            {
                throw new NullReferenceException($"Method context's {nameof(disposable)} is null");
            }
            Guard.AgainstNullArgument(nameof(node), node);

            if (node is IDisposableCollector collector)
            {
                collector.Collect(disposable);
            }
            else
            {
                node.GetOrCreateChild(_ => new Node(NodeName)).Collect(disposable);
            }

            return disposable;
        }
    }
}
