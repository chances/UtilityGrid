using System;

namespace Engine.Input
{
    [Flags]
    public enum KeyboardModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4
    }
}
