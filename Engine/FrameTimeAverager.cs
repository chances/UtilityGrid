namespace Engine
{
    public class FrameTimeAverager
    {
        private readonly double _decayRate = .3;
        private readonly double _timeLimit;

        private double _accumulatedTime;
        private int _frameCount;

        public FrameTimeAverager(double maxTimeSeconds = 666)
        {
            _timeLimit = maxTimeSeconds;
        }

        public double CurrentAverageFrameTimeSeconds { get; private set; }
        public double CurrentAverageFrameTimeMilliseconds => CurrentAverageFrameTimeSeconds * 1000.0;
        public double CurrentAverageFramesPerSecond => 1 / CurrentAverageFrameTimeSeconds;

        public void Reset()
        {
            _accumulatedTime = 0;
            _frameCount = 0;
        }

        public void AddTime(double seconds)
        {
            _accumulatedTime += seconds;
            _frameCount++;
            if (_accumulatedTime >= _timeLimit) Average();
        }

        private void Average()
        {
            var total = _accumulatedTime;
            CurrentAverageFrameTimeSeconds =
                CurrentAverageFrameTimeSeconds * _decayRate
                + total / _frameCount * (1 - _decayRate);

            _accumulatedTime = 0;
            _frameCount = 0;
        }
    }
}
