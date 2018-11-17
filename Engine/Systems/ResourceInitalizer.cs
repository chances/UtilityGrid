using Engine.ECS;
using Engine.ECS.Components;
using Veldrid;

namespace Engine.Systems
{
    public class ResourceInitializer : ECS.System
    {
        private readonly ResourceFactory _factory;
        private readonly GraphicsDevice _device;

        public ResourceInitializer(World world, ResourceFactory factory, GraphicsDevice device)
            : base(world, new[] {typeof(IResource)})
        {
            _factory = factory;
            _device = device;
        }

        public override void Operate()
        {
            foreach (var component in World.OperableComponentsFor(this, typeof(IResource)))
            {
                var resource = (IResource) component;
                resource.Initialize(_factory, _device);
            }
        }
    }
}
