using System;
using System.Drawing;
using Engine.Components.Receivers;
using Engine.ECS;
using Veldrid;

namespace Engine.Systems
{
    public class FramebufferSizeProvider : System<IFramebufferSize>
    {
        private readonly Framebuffer _framebuffer;
        private int _oldSize;

        public FramebufferSizeProvider(World world, Framebuffer framebuffer) : base(world)
        {
            _framebuffer = framebuffer;
        }

        public override void Operate()
        {
            if (IsDirty)
            {
                foreach (var componentToUpdate in OperableComponents)
                {
                    componentToUpdate.FramebufferSize = new Size((int) _framebuffer.Width, (int) _framebuffer.Height);
                }
            }

            _oldSize = Size;
        }

        private int Size => (int) (_framebuffer.Width * _framebuffer.Height);
        private bool IsDirty => Size != _oldSize;
    }
}
