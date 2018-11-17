using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS
{
    public abstract class System
    {
        protected World World { get; }

        public System(World world)
        {
            World = world;
        }

        private IEnumerable<Entity> OperableEntities => World;

        protected IEnumerable<Component> OperableComponents => OperableEntities.SelectMany(entity => entity.Values);
    }

    public abstract class System<T> : System
    {
        public System(World world) : base(world)
        {
        }

        protected new IEnumerable<T> OperableComponents =>
            World.Where(CanOperateOn).SelectMany(entity => entity.Values).OfType<T>();

        public abstract void Operate();

        private static bool CanOperateOn(Entity entity) => entity.Values.Any(component => component is T);
    }

    public abstract class System<T, UInterface> : System<UInterface> where T : Component
    {
        public System(World world) : base(world)
        {
        }

        protected new IEnumerable<T> OperableComponents => base.OperableComponents.OfType<T>();
    }
}
