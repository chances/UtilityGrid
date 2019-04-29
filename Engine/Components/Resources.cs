using System;
using Veldrid;

namespace Engine.Components
{
    public class Resources : IResource
    {
        public Action<ResourceFactory, GraphicsDevice> OnInitialize { private get; set; }
        public Action OnDispose { private get; set; }
        public bool Initialized { get; private set; } = false;

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            OnInitialize(factory, device);
            Initialized = true;
        }

        public void Dispose()
        {
            OnDispose();
            Initialized = false;
        }
    }
}
