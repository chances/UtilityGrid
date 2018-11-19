using System;
using Cairo;

namespace Engine.Components.UI
{
    public interface IDrawAction
    {
        void Draw(Action<Context> drawDelegate);
    }
}
