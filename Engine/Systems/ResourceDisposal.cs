using Engine.ECS;
using Engine.ECS.Components;
using Veldrid;

namespace Engine.Systems
{
    public class ResourceDisposal : System<IResource>
    {
        public ResourceDisposal(World world) : base(world)
        {
        }

        public override void Operate()
        {
            foreach (var resource in OperableComponents)
            {
                resource.Dispose();
            }
        }
    }
}
