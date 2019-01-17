using JetBrains.Annotations;

namespace Engine.ECS
{
    public abstract class Component
    {
        protected Component([CanBeNull] string name)
        {
            Name = name ?? GetType().Name;
        }

        public string Name { get; set; }
    }
}
