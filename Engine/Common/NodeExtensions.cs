using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using JetBrains.Annotations;
using LiteGuard;

namespace UtilityGrid.Engine.Common
{
    public static class NodeExtensions
    {
        [NotNull]
        public static T GetNode<T>([NotNull] this Godot.Node node, [NotNull] NodePath path) where T : class
        {
            Guard.AgainstNullArgument(nameof(node), node);
            Guard.AgainstNullArgument(nameof(path), path);

            if (!(node.GetNode(path) is T result))
            {
                throw new InvalidOperationException(
                    $"Unable to find node '{path}' in '{node.Name}'.");
            }

            return result;
        }

        [CanBeNull]
        public static T GetNodeOrDefault<T>(
            [NotNull] this Godot.Node node,
            [NotNull] NodePath path,
            [CanBeNull] T defaultValue = default(T)) where T : class
        {
            Guard.AgainstNullArgument(nameof(node), node);
            Guard.AgainstNullArgument(nameof(path), path);

            return (node.HasNode(path) ? node.GetNode(path) as T : null) ?? defaultValue;
        }

        [NotNull]
        public static TChild GetOrCreateNode<TParent, TChild>(
            [NotNull] this TParent node,
            [NotNull] NodePath path,
            [NotNull] Func<Godot.Node, TChild> factory)
            where TParent : Godot.Node
            where TChild : Godot.Node
        {
            Guard.AgainstNullArgument(nameof(node), node);
            Guard.AgainstNullArgument(nameof(path), path);
            Guard.AgainstNullArgument(nameof(factory), factory);

            var child = GetNodeOrDefault<TChild>(node, path);
            if (child != null) return child;

            var segments = path.GetConcatenatedSubnames().Split('/');

            Godot.Node parent;

            if (segments.Length <= 1)
            {
                parent = node;
            }
            else
            {
                var parentSegments = segments.Take(segments.Length - 1);
                var parentPath = string.Join("/", parentSegments);

                parent = node.GetNode(parentPath);
            }

            Guard.AgainstNullArgument(nameof(parent), parent);

            child = factory(parent);

            Guard.AgainstNullArgument(nameof(child), child);

            node.AddChild(child);

            return child;
        }

        [NotNull]
        public static T GetChild<T>([NotNull] this Godot.Node node) where T : class
        {
            Guard.AgainstNullArgument(nameof(node), node);

            switch (node.GetChildren().FirstOrDefault(n => n is T))
            {
                case T result:
                    return result;
                default:
                    throw new InvalidOperationException(
                        $"Unable to find node of type '{typeof(T)}' in '{node.Name}'.");
            }
        }

        [CanBeNull]
        public static T GetChildOrDefault<T>(
            [NotNull] this Godot.Node node,
            [CanBeNull] T defaultValue = default(T)) where T : class
        {
            Guard.AgainstNullArgument(nameof(node), node);

            switch (node.GetChildren().FirstOrDefault(n => n is T))
            {
                case T result:
                    return result;
                default:
                    return defaultValue;
            }
        }

        [NotNull]
        public static TChild GetOrCreateChild<TParent, TChild>(
            [NotNull] this TParent node,
            [NotNull] Func<TParent, TChild> factory)
            where TParent : Godot.Node
            where TChild : Godot.Node
        {
            Guard.AgainstNullArgument(nameof(node), node);
            Guard.AgainstNullArgument(nameof(factory), factory);

            switch (node.GetChildren().FirstOrDefault(n => n is TChild))
            {
                case TChild result:
                    return result;
                default:
                    var child = factory(node);

                    if (child == null)
                    {
                        throw new NullReferenceException($"{nameof(factory)} invocation resulted in null reference");
                    }

                    node.AddChild(child);

                    return child;
            }
        }

        [NotNull]
        public static IEnumerable<T> GetChildren<T>([NotNull] this Godot.Node node)
        {
            Guard.AgainstNullArgument(nameof(node), node);

            return node.GetChildren().OfType<T>();
        }

        [CanBeNull]
        public static Godot.Node GetClosestAncestor(
            [NotNull] this Godot.Node node, [NotNull] Func<Godot.Node, bool> predicate)
        {
            Guard.AgainstNullArgument(nameof(node), node);
            Guard.AgainstNullArgument(nameof(predicate), predicate);

            var ancestor = node;

            while ((ancestor = ancestor.GetParent()) != null)
            {
                if (predicate(ancestor))
                {
                    return ancestor;
                }
            }

            return null;
        }

        [CanBeNull]
        public static T GetClosestAncestor<T>([NotNull] this Godot.Node node) where T : class =>
            GetClosestAncestor(node, a => a is T) as T;
    }
}
