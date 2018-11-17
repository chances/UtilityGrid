using System;
using Engine.ECS;
using Engine.ECS.Components.Receivers;
using Veldrid;

namespace Engine.Systems
{
    public class FramebufferSizeUpdater : System<IFramebufferSize>
    {
        private readonly Framebuffer _framebuffer;

        public FramebufferSizeUpdater(World world, Framebuffer framebuffer) : base(world)
        {
            _framebuffer = framebuffer;
        }

        public override void Operate()
        {
            foreach (var componentToUpdate in OperableComponents)
            {
                componentToUpdate.FramebufferSize = new Tuple<uint, uint>(_framebuffer.Width, _framebuffer.Height);
            }
        }
    }
}
