using System;

namespace Engine.Components.Receivers
{
    public interface IFramebufferSize
    {
        Tuple<uint, uint> FramebufferSize { set; }
    }
}
