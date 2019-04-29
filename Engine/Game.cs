using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
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
        private Renderer _renderer;
        protected FramebufferSizeProvider _framebufferSizeProvider;
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

        protected Components.Camera Camera => World
            .FirstOrDefault(entity => entity.HasComponent<Components.Camera>())
            ?.GetComponent<Components.Camera>() ?? null;

        protected abstract GraphicsDevice CreateGraphicsDevice();

        protected abstract void Initialize();

        /// <summary>
        /// Initialize all world resources
        /// </summary>
        private void InitializeWorld()
        {
            new ComponentAssetLoader(World, _assetDataLoader).Operate();
            new ResourceInitializer(World, ResourceFactory, GraphicsDevice).Operate();
        }

        public void Run()
        {
            IsActive = true;

            GraphicsDevice = CreateGraphicsDevice();

            Initialize();
            InitializeWorld();

            _renderer = new Renderer(World, ResourceFactory, Framebuffer, GraphicsDevice.SubmitCommands);
            _framebufferSizeProvider = new FramebufferSizeProvider(
                World, GraphicsDevice.SwapchainFramebuffer.Width, GraphicsDevice.SwapchainFramebuffer.Height
            );

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

                    // Don't gobble up all available cycles while waiting
                    var deltaMilliseconds = DesiredFrameLengthSeconds * 1000.0 - deltaSeconds * 1000.0;
                    if (deltaMilliseconds > 8)
                        Thread.Sleep(5);
                    else
                        Thread.SpinWait(100);
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
            // TODO: update input provider system for input receivers
        }

        protected virtual void Update(GameTime gameTime)
        {
            new ViewProjectionProvider(World, Camera?.ViewProjectionUniform ?? null).Operate();
            new ResourceInitializer(World, ResourceFactory, GraphicsDevice).Operate();
            _framebufferSizeProvider.Operate();
            new ComponentUpdater(World).Operate(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected virtual void Render(GameTime gameTime)
        {
            _renderer.Operate();

            GraphicsDevice.SwapBuffers();
            GraphicsDevice.WaitForIdle();
        }

        protected void Exit()
        {
            IsActive = false;
        }

        public virtual void Dispose()
        {
            // Dispose all world resources
            new ResourceDisposal(World).Operate();

            _renderer.Dispose();
            GraphicsDevice.Dispose();
        }
    }
}
