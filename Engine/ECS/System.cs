using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS
{
    public abstract class System
    {
        public World World { get; }
        public IEnumerable<Type> OperableComponentTypes { get; }

        protected System(World world, IEnumerable<Type> operableComponentTypes)
        {
            World = world;
            OperableComponentTypes = operableComponentTypes;
        }

        public bool CanOperateOn(Entity entity) =>
            entity.Values.All(component => OperableComponentTypes.Contains(component.GetType()));

        public abstract void Operate();
    }
}
