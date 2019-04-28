using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Cairo;
using Engine.Buffers;
using Engine.Buffers.Layouts;
using Engine.Components.Receivers;
using Engine.ECS;
using Engine.Primitives;
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

        public static readonly MeshData Mesh =
            MeshBuilder.TexturedUnitQuad("UI Surface Mesh");

        public bool Ready => _surface != null;

        public Size FramebufferSize
        {
            set
            {
                _surface?.Dispose();
                _surface = new ImageSurface(Format.Argb32, value.Width, value.Height);
            }
        }

        public ResourceLayout ResourceLayout { get; private set; }

        public ResourceSet ResourceSet { get; private set; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            _device = device;
            // TODO: Refactor this so not dependant on the GraphicsDevice ref, make use of IFramebufferSize
            var framebuffer = device.SwapchainFramebuffer;
            _texture = factory.CreateTexture(TextureDescription.Texture2D(framebuffer.Width, framebuffer.Height,
                1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            _textureView = factory.CreateTextureView(_texture);

            _viewProj = new UniformBuffer<UniformViewProjection>(
                new UniformViewProjection(Matrix4x4.CreateOrthographic(framebuffer.Width, framebuffer.Height, 0, 1)));
            _viewProj.Initialize(factory, device);

            ResourceLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    // TODO: Do I need a View Projection uniform for UI?
                    // new ResourceLayoutElementDescription(
                    //     "ViewProj", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription(
                        "SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription(
                        "SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                ResourceLayout,
                // _viewProj.DeviceBuffer,
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

            Draw(c => {
                c.SetSourceRGB(1, 0, 0);
                c.Rectangle(10, 10, 50, 10);
                c.Fill();

                c.Scale(4, 4);

                c.SetSourceRGB(1, 1, 1);
                c.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold);
                c.SetFontSize(12);
                var text = "Hello, world!";
                var te = c.TextExtents(text);
                c.MoveTo(
                    0.5 - te.Width / 2,
                    0.5 - te.Height / 2);
                // bearing has stuff to do with where glyphs are placed relative to the baseline
                // https://www.cairographics.org/manual/cairo-cairo-scaled-font-t.html#cairo-text-extents-t
                // c.MoveTo(
                //     0.5 - te.Width / 2 - te.XBearing + 10,
                //     0.5 - te.Height / 2 - te.YBearing + 60);
                c.ShowText(text);
            });

            surface.Flush();

            var sourceData = Argb32ToRgba32(surface.Data, surface.Width, surface.Height);
            var width = (uint) surface.Width;
            var height = (uint) surface.Height;
            var gcHandle = GCHandle.Alloc(sourceData, GCHandleType.Pinned);
            _device?.UpdateTexture(_texture, gcHandle.AddrOfPinnedObject(), (uint) sourceData.Length, 0, 0, 0, width, height, 1, 0, 0);
            gcHandle.Free();

            Draw(c => {
                // Clear surface to fully transparent
                c.Save();
                c.Operator = Operator.Clear;
                c.Paint();
                c.Restore();
            });
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
