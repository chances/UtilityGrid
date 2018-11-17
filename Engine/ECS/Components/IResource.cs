using System;
using Veldrid;

namespace Engine.ECS.Components
{
    public interface IResource : IDisposable
    {
        void Initialize(ResourceFactory factory, GraphicsDevice device);
    }
}
