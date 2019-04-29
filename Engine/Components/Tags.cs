using System;

namespace Engine.Components
{
    [Flags]
    public enum Tags : ushort
    {
        // Values must be powers of two
        Visited = 1,
        Initialized = 2,
        RayCastHit = 4
    }
}
