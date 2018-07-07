using System.Linq;
using Engine;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class UtilityGridGame : Engine.Game
    {
        private Sdl2Window _window;
        private Camera _camera;
        private static CommandList _commandList;

        public UtilityGridGame()
        {
            Components.Add(_camera = new Camera(this));
        }

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            var windowCreateInfo = new WindowCreateInfo()
            {
                X = 100,
                Y = 100,
                WindowWidth = 960,
                WindowHeight = 540,
                WindowTitle = "Utility Grid"
            };
            _window = VeldridStartup.CreateWindow(ref windowCreateInfo);
            _window.CursorVisible = true;

            // TODO: Setup multisampling AA

            return VeldridStartup.CreateGraphicsDevice(_window);
        }

        protected override void Initialize()
        {
            _commandList = ResourceFactory.CreateCommandList();

            base.Initialize();
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

            if (IsKeyDown(inputSnapshot, Key.Escape))
                Exit();

            // TODO: Add your update logic here

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
            _commandList.ClearColorTarget(0, RgbaFloat.CornflowerBlue);

            // TODO: Draw geometry with the command list

            _commandList.End();
            GraphicsDevice.SubmitCommands(_commandList);

            base.Render(gameTime);
        }

        private bool IsKeyDown(InputSnapshot snapshot, Key key)
        {
            return snapshot.KeyEvents.Any(k => k.Down && k.Key == key);
        }
    }
}
