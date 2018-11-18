using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Engine;
using Engine.Components;
using Engine.ECS;
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
            var windowCreateInfo = new WindowCreateInfo()
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
                : CreateOpenGLGraphicsDevice(_window, new GraphicsDeviceOptions(false));
        }

        private static unsafe GraphicsDevice CreateOpenGLGraphicsDevice(Sdl2Window window, GraphicsDeviceOptions options)
        {
            Sdl2Native.SDL_ClearError();
            var sdlHandle = window.SdlWindowHandle;
            SDL_SysWMinfo sdlSysWmInfo;
            Sdl2Native.SDL_GetVersion(&sdlSysWmInfo.version);
            Sdl2Native.SDL_GetWMWindowInfo((SDL_Window) sdlHandle, &sdlSysWmInfo);

            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextFlags, options.Debug ? 3 : 2);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextProfileMask, 1);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextMajorVersion, 4);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextMinorVersion, 1);

            options.SwapchainDepthFormat = PixelFormat.D32_Float_S8_UInt;
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.DepthSize, 32);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.StencilSize, 8);

            var contextHandle = Sdl2Native.SDL_GL_CreateContext((SDL_Window) sdlHandle);
            var error = Sdl2Native.SDL_GetError();
            if ((IntPtr) error != IntPtr.Zero)
            {
                string str = GetString(error);
                if (!string.IsNullOrEmpty(str))
                    throw new VeldridException(string.Format("Unable to create OpenGL Context: \"{0}\". This may indicate that the system does not support the requested OpenGL profile, version, or Swapchain format.", (object) str));
            }
            int num1;
            Sdl2Native.SDL_GL_GetAttribute(SDL_GLAttribute.DepthSize, &num1);
            int num2;
            Sdl2Native.SDL_GL_GetAttribute(SDL_GLAttribute.StencilSize, &num2);
            Sdl2Native.SDL_GL_SetSwapInterval(options.SyncToVerticalBlank ? 1 : 0);
            var getProcAddress = new Func<string, IntPtr>(Sdl2Native.SDL_GL_GetProcAddress);
            var makeCurrent = (Action<IntPtr>) (context => Sdl2Native.SDL_GL_MakeCurrent((SDL_Window) sdlHandle, context));
            var deleteContext = new Action<IntPtr>(Sdl2Native.SDL_GL_DeleteContext);
            var swapBuffers = (Action) (() => Sdl2Native.SDL_GL_SwapWindow((SDL_Window) sdlHandle));
            var platformInfo = new OpenGLPlatformInfo(contextHandle, getProcAddress, makeCurrent, (Func<IntPtr>) (Sdl2Native.SDL_GL_GetCurrentContext), (Action) (() => Sdl2Native.SDL_GL_MakeCurrent(new SDL_Window(IntPtr.Zero), IntPtr.Zero)), deleteContext, swapBuffers, (Action<bool>) (sync => Sdl2Native.SDL_GL_SetSwapInterval(sync ? 1 : 0)));
            return GraphicsDevice.CreateOpenGL(options, platformInfo, (uint) window.Width, (uint) window.Height);
        }

        private static unsafe string GetString(byte* stringStart)
        {
            var byteCount = 0;
            while (stringStart[byteCount] != (byte) 0)
                ++byteCount;
            return Encoding.UTF8.GetString(stringStart, byteCount);
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
