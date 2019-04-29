using System;
using Veldrid;

namespace Engine.Components
{
    public interface IResource : IDisposable
    {
        bool Initialized { get; }
        void Initialize(ResourceFactory factory, GraphicsDevice device);
    }
}
