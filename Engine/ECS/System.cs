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

        protected IEnumerable<Entity> OperableEntities => World;

        protected IEnumerable<Component> OperableComponents => OperableEntities.SelectMany(entity => entity.Values);

        public abstract void Operate();
    }

    public abstract class System<T> : System
    {
        public System(World world) : base(world)
        {
        }

        protected new IEnumerable<Entity> OperableEntities => World.Where(CanOperateOn);

        protected new IEnumerable<T> OperableComponents =>
            OperableEntities.SelectMany(entity => entity.Values).OfType<T>();

        public override abstract void Operate();

        private static bool CanOperateOn(Entity entity) => entity.Values.Any(component => component is T);
    }
}
