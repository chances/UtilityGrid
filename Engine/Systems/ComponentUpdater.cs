using Engine.ECS;
using Engine.ECS.Components;
using Veldrid;

namespace Engine.Systems
{
    public class ComponentUpdater : ECS.System
    {
        public ComponentUpdater(World world) : base(world, new[] {typeof(IUpdatable)})
        {
        }

        private GameTime _gameTime;

        public void Operate(GameTime gameTime)
        {
            _gameTime = gameTime;
            Operate();
        }

        public override void Operate()
        {
            foreach (var component in World.OperableComponentsFor(this, typeof(IUpdatable)))
            {
                var componentToUpdate = (IUpdatable) component;
                componentToUpdate.Update(_gameTime);
            }
        }
    }
}
