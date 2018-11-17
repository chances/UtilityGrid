using Engine.ECS;
using Engine.ECS.Components;
using Veldrid;

namespace Engine.Systems
{
    public class ResourceInitializer : System<IResource>
    {
        private readonly ResourceFactory _factory;
        private readonly GraphicsDevice _device;

        public ResourceInitializer(World world, ResourceFactory factory, GraphicsDevice device) : base(world)
        {
            _factory = factory;
            _device = device;
        }

        public override void Operate()
        {
            foreach (var resource in OperableComponents)
            {
                resource.Initialize(_factory, _device);
            }
        }
    }
}
