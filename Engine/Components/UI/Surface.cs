using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Cairo;
using Engine.Buffers;
using Engine.Components.Receivers;
using Engine.ECS;
using Veldrid;

namespace Engine.Components.UI
{
    public class Surface : Component, IFramebufferSize, IReady, IResource, IUpdatable, IResourceSet, IDrawAction
    {
        private GraphicsDevice _device;
        private Texture _texture;
        private TextureView _textureView;
        private UniformBuffer<UniformViewProjection> _viewProj;

        private Cairo.Surface _surface;

        public Surface() : base("UI Surface")
        {
        }

        public bool Ready => _surface != null;

        public Size FramebufferSize
        {
            set
            {
                _surface?.Dispose();
                _surface = new ImageSurface(Format.Argb32, value.Width, value.Height);
            }
        }

        public ResourceSet ResourceSet { get; private set; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            _device = device;
            var framebuffer = device.SwapchainFramebuffer;
            _texture = factory.CreateTexture(TextureDescription.Texture2D(framebuffer.Width, framebuffer.Height,
                1, 1, Veldrid.PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            _textureView = factory.CreateTextureView(_texture);

            _viewProj = new UniformBuffer<UniformViewProjection>(
                new UniformViewProjection(Matrix4x4.CreateOrthographic(framebuffer.Width, framebuffer.Height, 0, 1)));
            _viewProj.Initialize(factory, device);

            var uiTextureLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription(
                        "ViewProj", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription(
                        "SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription(
                        "SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                uiTextureLayout,
                _viewProj.DeviceBuffer,
                _textureView,
                device.LinearSampler));
        }

        public void Draw(Action<Context> drawDelegate)
        {
            using (var context = new Context(_surface))
            {
                drawDelegate(context);
            }
        }

        public void Update(GameTime gameTime)
        {
            Update();
        }

        private void Update()
        {
            if (!(_surface is ImageSurface surface)) return;

            surface.Flush();

            var sourceData = Argb32ToRgba32(surface.Data, surface.Width, surface.Height);
            var width = (uint) surface.Width;
            var height = (uint) surface.Height;
            var gcHandle = GCHandle.Alloc(sourceData, GCHandleType.Pinned);
            _device?.UpdateTexture(_texture, gcHandle.AddrOfPinnedObject(), (uint) sourceData.Length, 0, 0, 0, width, height, 1, 0, 0);
            gcHandle.Free();

            // Clear surface to fully transparent
            using (var c = new Context(surface))
            {
                c.Save();
                c.Operator = Operator.Clear;
                c.Paint();
                c.Restore();
            }
        }

        public void Dispose()
        {
            _surface?.Dispose();
            _textureView.Dispose();
            _texture.Dispose();
            _viewProj.Dispose();
            ResourceSet.Dispose();
        }

        private static byte[] Argb32ToRgba32(IReadOnlyList<byte> source, int width, int height)
        {
            var destination = new byte[width * height * 32];

            for (var pixel = 0; pixel < source.Count; pixel += 32)
            {
                byte a = source[pixel],
                    r = source[pixel + 1],
                    g = source[pixel + 2],
                    b = source[pixel + 3];
                destination[pixel] = r;
                destination[pixel + 1] = g;
                destination[pixel + 2] = b;
                destination[pixel + 3] = a;
            }

            return destination;
        }
    }
}
