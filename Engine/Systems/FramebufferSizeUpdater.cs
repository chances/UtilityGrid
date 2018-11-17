using System;
using Engine.ECS;
using Engine.ECS.Components.Receivers;
using Veldrid;

namespace Engine.Systems
{
    public class FramebufferSizeUpdater : ECS.System
    {
        private readonly Framebuffer _framebuffer;

        public FramebufferSizeUpdater(World world, Framebuffer framebuffer)
            : base(world, new[] {typeof(IFramebufferSize)})
        {
            _framebuffer = framebuffer;
        }

        public override void Operate()
        {
            foreach (var component in World.OperableComponentsFor(this, typeof(IFramebufferSize)))
            {
                var componentToUpdate = (IFramebufferSize) component;
                componentToUpdate.FramebufferSize = new Tuple<uint, uint>(_framebuffer.Width, _framebuffer.Height);
            }
        }
    }
}
