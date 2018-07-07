using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine.Components;
using Veldrid;

namespace Engine
{
    public abstract class Game : IDisposable
    {
        private bool _running;
        private GameTime _gameTime;
        private readonly FrameTimeAverager _frameTimeAverager = new FrameTimeAverager(0.666);

        protected Game()
        {
            Components = new List<Component>();
            LimitFrameRate = true;
            DesiredFrameLengthSeconds = 1.0 / 60.0;

            GraphicsDevice = CreateGraphicsDevice();
            Initialize();
        }

        protected ICollection<Component> Components { get; }

        public bool LimitFrameRate { get; set; }
        public double DesiredFrameLengthSeconds { get; set; }
        public double FramesPerSecond => Math.Round(_frameTimeAverager.CurrentAverageFramesPerSecond, 2);

        public GraphicsDevice GraphicsDevice { get; }
        public ResourceFactory ResourceFactory => GraphicsDevice.ResourceFactory;
        public Framebuffer Framebuffer => GraphicsDevice.SwapchainFramebuffer;

        private TimeSpan TotalElapsedTime => _gameTime?.TotalGameTime ?? TimeSpan.Zero;

        protected abstract GraphicsDevice CreateGraphicsDevice();

        protected virtual void Initialize()
        {
            foreach (var component in Components)
            {
                component.Initialize();
            }
        }

        public void Run()
        {
            _running = true;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (_running)
            {
                _gameTime = new GameTime(TotalElapsedTime + stopwatch.Elapsed, stopwatch.Elapsed);
                var deltaSeconds = _gameTime.ElapsedGameTime.TotalSeconds;
                stopwatch.Restart();

                while (LimitFrameRate && deltaSeconds < DesiredFrameLengthSeconds)
                {
                    var elapsed = stopwatch.Elapsed;
                    _gameTime = new GameTime(TotalElapsedTime + elapsed, _gameTime.ElapsedGameTime + elapsed);
                    deltaSeconds += elapsed.TotalSeconds;
                    stopwatch.Restart();
                }
                if (deltaSeconds > DesiredFrameLengthSeconds * 1.25) _gameTime = GameTime.RunningSlowly(_gameTime);

                _frameTimeAverager.AddTime(deltaSeconds);

                Update(_gameTime);
                if (!_running) break;

                Render(_gameTime);
            }
        }

        protected void Exit()
        {
            _running = false;
        }

        protected virtual void Update(GameTime gameTime)
        {
            foreach (var component in Components)
            {
                component.Update(gameTime);
            }
        }

        protected virtual void Render(GameTime gameTime)
        {
            GraphicsDevice.SwapBuffers();
        }

        public virtual void Dispose()
        {
            GraphicsDevice.Dispose();
        }
    }
}
