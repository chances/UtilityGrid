using System;

namespace UtilityGrid.Engine.Common
{
    public struct Range<T> where T : IComparable<T>
    {
        public T Min { get; }

        public T Max { get; }

        public Range(T min, T max)
        {
            if (!(max.CompareTo(min) > 0))
            {
                throw new ArgumentException($"Argument '{nameof(max)}' must be greater than '{nameof(min)}'.");
            }

            Min = min;
            Max = max;
        }

        public T Clamp(T value)
        {
            var v = value.CompareTo(Min) > 0 ? value : Min;

            return v.CompareTo(Max) < 0 ? v : Max;
        }
    }
}
