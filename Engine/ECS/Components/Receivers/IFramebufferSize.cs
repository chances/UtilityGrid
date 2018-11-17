using System;

namespace Engine.ECS.Components.Receivers
{
    public interface IFramebufferSize
    {
        Tuple<uint, uint> FramebufferSize { set; }
    }
}
