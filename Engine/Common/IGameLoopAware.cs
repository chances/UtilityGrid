using System;

namespace UtilityGrid.Engine.Common
{
    public interface IGameLoopAware
    {
        ProcessMode ProcessMode { get; }

        IObservable<float> OnLoop { get; }
    }
}
