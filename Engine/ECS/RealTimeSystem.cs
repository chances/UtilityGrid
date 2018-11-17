using System.Collections.Generic;
using System.Linq;
using Engine.ECS.Components;

namespace Engine.ECS
{
    public abstract class RealTimeSystem : System
    {
        protected RealTimeSystem(World world) : base(world)
        {
        }

        protected new IEnumerable<IUpdatable> OperableComponents =>
            World.Where(CanOperateOn).SelectMany(entity => entity.Values).OfType<IUpdatable>();

        public abstract void Operate(GameTime gameTime);

        private static bool CanOperateOn(Entity entity) => entity.Values.Any(component => component is IUpdatable);
    }
}
