using System;
using System.Text;
using Veldrid;
using Veldrid.OpenGL;
using Veldrid.Sdl2;

namespace Game
{
    public class GraphicsDeviceUtils
    {
        public static unsafe GraphicsDevice CreateOpenGLGraphicsDevice(Sdl2Window window, GraphicsDeviceOptions options)
        {
            Sdl2Native.SDL_ClearError();
            var sdlHandle = window.SdlWindowHandle;
            SDL_SysWMinfo sdlSysWmInfo;
            Sdl2Native.SDL_GetVersion(&sdlSysWmInfo.version);
            Sdl2Native.SDL_GetWMWindowInfo(sdlHandle, &sdlSysWmInfo);

            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextFlags, options.Debug ? 3 : 2);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextProfileMask, 1);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextMajorVersion, 4);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.ContextMinorVersion, 1);

            options.SwapchainDepthFormat = PixelFormat.D32_Float_S8_UInt;
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.DepthSize, 32);
            Sdl2Native.SDL_GL_SetAttribute(SDL_GLAttribute.StencilSize, 8);

            var contextHandle = Sdl2Native.SDL_GL_CreateContext(sdlHandle);
            var error = Sdl2Native.SDL_GetError();
            if ((IntPtr) error != IntPtr.Zero)
            {
                var str = GetString(error);
                if (!string.IsNullOrEmpty(str))
                    throw new VeldridException(string.Format("Unable to create OpenGL Context: \"{0}\". This may indicate that the system does not support the requested OpenGL profile, version, or Swapchain format.", str));
            }
            int num1;
            Sdl2Native.SDL_GL_GetAttribute(SDL_GLAttribute.DepthSize, &num1);
            int num2;
            Sdl2Native.SDL_GL_GetAttribute(SDL_GLAttribute.StencilSize, &num2);
            Sdl2Native.SDL_GL_SetSwapInterval(options.SyncToVerticalBlank ? 1 : 0);
            var getProcAddress = new Func<string, IntPtr>(Sdl2Native.SDL_GL_GetProcAddress);
            var makeCurrent = (Action<IntPtr>) (context => Sdl2Native.SDL_GL_MakeCurrent(sdlHandle, context));
            var deleteContext = new Action<IntPtr>(Sdl2Native.SDL_GL_DeleteContext);
            var swapBuffers = (Action) (() => Sdl2Native.SDL_GL_SwapWindow(sdlHandle));
            var platformInfo = new OpenGLPlatformInfo(contextHandle, getProcAddress, makeCurrent, Sdl2Native.SDL_GL_GetCurrentContext, () => Sdl2Native.SDL_GL_MakeCurrent(new SDL_Window(IntPtr.Zero), IntPtr.Zero), deleteContext, swapBuffers, sync => Sdl2Native.SDL_GL_SetSwapInterval(sync ? 1 : 0));
            return GraphicsDevice.CreateOpenGL(options, platformInfo, (uint) window.Width, (uint) window.Height);
        }

        private static unsafe string GetString(byte* stringStart)
        {
            var byteCount = 0;
            while (stringStart[byteCount] != 0)
                ++byteCount;
            return Encoding.UTF8.GetString(stringStart, byteCount);
        }
    }
}
