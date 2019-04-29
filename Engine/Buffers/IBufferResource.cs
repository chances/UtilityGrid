using System;
using Veldrid;

namespace Engine.Buffers
{
    interface IBufferResource : IDisposable
    {
        void Initialize(ResourceFactory factory, GraphicsDevice device);
    }
}
