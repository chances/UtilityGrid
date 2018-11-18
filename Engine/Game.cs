using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Engine.Assets;
using Engine.ECS;
using Engine.Input;
using Engine.Systems;
using Veldrid;

namespace Engine
{
    public abstract class Game : IDisposable
    {
        private readonly AssetDataLoader _assetDataLoader;
        private readonly FrameTimeAverager _frameTimeAverager = new FrameTimeAverager(0.666);

        protected Game()
        {
            World = new World();
            LimitFrameRate = true;
            DesiredFrameLengthSeconds = 1.0 / 60.0;

            _assetDataLoader = new AssetDataLoader(Assembly.GetCallingAssembly(), AssetDirectoryPaths);
        }

        private GameTime _gameTime;

        protected World World { get; }

        public bool IsActive { get; private set; }
        public bool LimitFrameRate { get; }
        public double DesiredFrameLengthSeconds { get; }
        public double FramesPerSecond => Math.Round(_frameTimeAverager.CurrentAverageFramesPerSecond, 2);
        public MouseState MouseState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }
        public ResourceFactory ResourceFactory => GraphicsDevice.ResourceFactory;
        public Framebuffer Framebuffer => GraphicsDevice.SwapchainFramebuffer;

        public Dictionary<AssetType, string> AssetDirectoryPaths { get; } = new Dictionary<AssetType, string>();

        private TimeSpan TotalElapsedTime => _gameTime?.TotalGameTime ?? TimeSpan.Zero;

        protected abstract GraphicsDevice CreateGraphicsDevice();

        protected abstract void Initialize();

        private void InternalInitialize()
        {
            // Initialize all world resources
            new ResourceInitializer(World, ResourceFactory, GraphicsDevice).Operate();
            new ComponentAssetLoader(World, _assetDataLoader).Operate();
        }

        public void Run()
        {
            IsActive = true;

            GraphicsDevice = CreateGraphicsDevice();

            Initialize();
            InternalInitialize();

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

        protected void ProcessInput(MouseState mouseState, KeyboardState keyboardState)
        {
            MouseState = mouseState;
            KeyboardState = keyboardState;
        }

        protected virtual void Update(GameTime gameTime)
        {
            new FramebufferSizeUpdater(World, GraphicsDevice.SwapchainFramebuffer).Operate();
            new ComponentUpdater(World).Operate(gameTime);
        }

        protected virtual void Render(GameTime gameTime)
        {
            GraphicsDevice.SwapBuffers();
        }

        protected void Exit()
        {
            IsActive = false;
        }

        public virtual void Dispose()
        {
            // Dispose all world resources
            new ResourceDisposal(World).Operate();

            GraphicsDevice.Dispose();
        }
    }
}
