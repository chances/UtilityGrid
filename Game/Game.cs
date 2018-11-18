using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Engine;
using Engine.Components;
using Veldrid;
using Veldrid.OpenGL;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Key = Engine.Input.Key;
using MouseButton = Engine.Input.MouseButton;

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
            var windowCreateInfo = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 960,
                WindowHeight = 540,
                WindowTitle = "Utility Grid"
            };
            _window = VeldridStartup.CreateWindow(ref windowCreateInfo);
            _window.Closing += Exit;
            _window.Closed += Dispose;
            _window.CursorVisible = true;

            // TODO: Setup multisampling AA

            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                ? VeldridStartup.CreateGraphicsDevice(_window)
                : GraphicsDeviceUtils.CreateOpenGLGraphicsDevice(_window, new GraphicsDeviceOptions(false));
        }

        protected override void Initialize()
        {
            World.Add(Camera.CreateEntity());

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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Render(GameTime gameTime)
        {
            _commandList.Begin();
            _commandList.SetFramebuffer(Framebuffer);
            _commandList.ClearColorTarget(0,
                MouseState.IsButtonDown(MouseButton.Left) ? RgbaFloat.Cyan : RgbaFloat.CornflowerBlue);

            // TODO: Draw geometry with the command list

            _commandList.End();
            GraphicsDevice.SubmitCommands(_commandList);
            GraphicsDevice.WaitForIdle();

            base.Render(gameTime);
        }
    }
}
