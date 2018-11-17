using Engine.ECS;
using Engine.ECS.Components;
using Veldrid;

namespace Engine.Systems
{
    public class ResourceDisposal : ECS.System
    {
        public ResourceDisposal(World world)
            : base(world, new[] {typeof(IResource)})
        {
        }

        public override void Operate()
        {
            foreach (var component in World.OperableComponentsFor(this, typeof(IResource)))
            {
                var resource = (IResource) component;
                resource.Dispose();
            }
        }
    }
}
