using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine.Components;
using Engine.ECS;
using Engine.Input;
using Engine.Systems;
using Veldrid;

namespace Engine
{
    public abstract class Game : IDisposable
    {
        private readonly FrameTimeAverager _frameTimeAverager = new FrameTimeAverager(0.666);
        private GameTime _gameTime;

        protected Game()
        {
            World = new World();
            LimitFrameRate = true;
            DesiredFrameLengthSeconds = 1.0 / 60.0;

            // ReSharper disable once VirtualMemberCallInConstructor
            GraphicsDevice = CreateGraphicsDevice();
            // ReSharper disable once VirtualMemberCallInConstructor
            Initialize();
        }

        protected World World { get; }

        public bool IsActive { get; private set; }
        public bool LimitFrameRate { get; }
        public double DesiredFrameLengthSeconds { get; }
        public double FramesPerSecond => Math.Round(_frameTimeAverager.CurrentAverageFramesPerSecond, 2);
        public MouseState MouseState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }

        public GraphicsDevice GraphicsDevice { get; }
        public ResourceFactory ResourceFactory => GraphicsDevice.ResourceFactory;
        public Framebuffer Framebuffer => GraphicsDevice.SwapchainFramebuffer;

        private TimeSpan TotalElapsedTime => _gameTime?.TotalGameTime ?? TimeSpan.Zero;

        public virtual void Dispose()
        {
            // Dispose all world resources
            new ResourceDisposal(World).Operate();

            GraphicsDevice.Dispose();
        }

        protected abstract GraphicsDevice CreateGraphicsDevice();

        protected virtual void Initialize()
        {
            new ResourceInitializer(World, ResourceFactory, GraphicsDevice).Operate();
        }

        public void Run()
        {
            IsActive = true;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (IsActive)
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
                if (!IsActive) break;

                Render(_gameTime);
            }
        }

        protected void Exit()
        {
            IsActive = false;
        }

        protected void ProcessInput(MouseState mouseState, KeyboardState keyboardState)
        {
            MouseState = mouseState;
            KeyboardState = keyboardState;
        }

        protected virtual void Update(GameTime gameTime)
        {
            new ComponentUpdater(World).Operate(gameTime);
        }

        protected virtual void Render(GameTime gameTime)
        {
            GraphicsDevice.SwapBuffers();
        }
    }
}
