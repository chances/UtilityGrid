using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Engine;
using Engine.Assets;
using Engine.Components;
using Engine.Entities;
using Engine.Primitives;
using Game.Content;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Key = Engine.Input.Key;
using UI = Engine.Components.UI;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class UtilityGridGame : Engine.Game
    {
        private Sdl2Window _window;
        private static CommandList _commandList;

        public UtilityGridGame()
        {
            foreach (var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Console.WriteLine(resourceName);
            }
        }

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            // TODO: Migrate this to Sdl2Native.SDL_WINDOWPOS_CENTERED
            const int windowPositionCentered = 0x2FFF0000;
            var windowCreateInfo = new WindowCreateInfo
            {
                X = windowPositionCentered,
                Y = windowPositionCentered,
                WindowWidth = 960,
                WindowHeight = 540,
                WindowTitle = "Utility Grid"
            };
            _window = VeldridStartup.CreateWindow(ref windowCreateInfo);
            _window.Resized += () => _framebufferSizeProvider.Update(_window.Width, _window.Height);
            // _window.Closing += Exit; // TODO: Save game if necessary?
            _window.Closed += Exit;
            _window.CursorVisible = true;

            // TODO: Setup multisampling AA

            var options = new GraphicsDeviceOptions(debug: false);
            options.SwapchainDepthFormat = PixelFormat.R16_UNorm;
            options.PreferDepthRangeZeroToOne = true;
            options.PreferStandardClipSpaceYDirection = true;

            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                ? VeldridStartup.CreateGraphicsDevice(_window, options)
                : GraphicsDeviceUtils.CreateOpenGLGraphicsDevice(_window, options);
        }

        protected override void Initialize()
        {
            AssetDirectoryPaths.Add(AssetType.Model, "Game.Content.Models");
            AssetDirectoryPaths.Add(AssetType.Shader, "Game.Content.Shaders");

            var flatMaterial = new Material("FlatMaterial", Shaders.Flat);
            var uiMaterial = new Material("UIMaterial", Shaders.UI);

            World.Add(EntityFactory.Create<Camera>());

            // World.Add(EntityFactory.Create(new UI.Surface(), UI.Surface.Mesh, uiMaterial));

            World.Add(EntityFactory.Create(new Buildings.Building(), new Cube("Box").MeshData, flatMaterial));

            _commandList = ResourceFactory.CreateCommandList();
        }

        public override void Dispose()
        {
            _commandList.Dispose();

            base.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!_window.Exists)
            {
                Exit();
                return;
            }

            var inputSnapshot = _window.PumpEvents();
            ProcessInput(InputSnapshotConverter.Mouse(inputSnapshot, MouseState),
                InputSnapshotConverter.Keyboard(inputSnapshot));

            if (KeyboardState.IsKeyDown(Key.Escape))
                Exit();

            base.Update(gameTime);

            var frameTime = gameTime.ElapsedGameTime.Milliseconds;
            _window.Title = $"Utility Grid - {frameTime} ms - {FramesPerSecond} fps";
        }
    }
}
