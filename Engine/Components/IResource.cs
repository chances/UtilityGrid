using System;
using Veldrid;

namespace Engine.Components
{
    public interface IResource : IDisposable
    {
        void Initialize(ResourceFactory factory, GraphicsDevice device);
    }
}
