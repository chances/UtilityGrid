using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Components;

namespace Engine.ECS
{
    public abstract class RealTimeSystem : System
    {
        protected RealTimeSystem(World world) : base(world)
        {
        }

        protected new IEnumerable<IUpdatable> OperableComponents =>
            World.Where(CanOperateOn).SelectMany(entity => entity.Values).OfType<IUpdatable>();

        public override void Operate()
        {
            throw new InvalidOperationException(
                $"Must operate {nameof(RealTimeSystem)} given current {nameof(GameTime)}"
            );
        }

        public abstract void Operate(GameTime gameTime);

        private static bool CanOperateOn(Entity entity) => entity.HasComponent<IUpdatable>();
    }
}
