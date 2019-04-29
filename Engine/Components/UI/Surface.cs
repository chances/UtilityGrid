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
    public class Surface : ResourceComponent, IFramebufferSize, IReady, IUpdatable, IResourceSet, IDrawAction
    {
        private GraphicsDevice _device;
        private Texture _texture;
        private TextureView _textureView;
        private Sampler _sampler;
        private Size _size = new Size(0, 0);

        private Cairo.Surface _surface;

        public Surface() : base("UI Surface")
        {
            Resources.OnInitialize = (factory, device) => {
                _device = device;

                _size = new Size(
                    (int) device.SwapchainFramebuffer.Width,
                    (int) device.SwapchainFramebuffer.Height
                );

                _sampler = device.LinearSampler;

                ResourceLayout = factory.CreateResourceLayout(
                    new ResourceLayoutDescription(
                        new ResourceLayoutElementDescription(
                            "SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                        new ResourceLayoutElementDescription(
                            "SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

                CreateTexture();
            };
            Resources.OnDispose = () => {
                _surface?.Dispose();
                _textureView.Dispose();
                _texture.Dispose();
                ResourceSet.Dispose();
            };
        }

        public static readonly MeshData Mesh =
            MeshBuilder.TexturedUnitQuad("UI Surface Mesh");

        public bool IsReady => _surface != null;

        public Size FramebufferSize
        {
            set
            {
                _size = value;

                // Recreate the UI's backing texture, sampler, and surface
                CreateTexture();
            }
        }

        public ResourceLayout ResourceLayout { get; private set; }

        public ResourceSet ResourceSet { get; private set; }

        public void Draw(Action<Context> drawDelegate)
        {
            using (var context = new Context(_surface))
            {
                drawDelegate(context);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!(_surface is ImageSurface surface)) return;

            Draw(c => {
                c.SetSourceRGBA(1, 0, 0, 1);
                c.Rectangle(30, 10, _size.Width / 2, 10);
                c.Fill();

                c.Save();

                c.Scale(4, 4);

                c.SetSourceRGB(1, 1, 1);
                c.SelectFontFace("Helvetica", FontSlant.Normal, FontWeight.Bold);
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

                c.Restore();
            });

            surface.Flush();

            var sourceData = Bgra32ToRgba32(surface.Data, surface.Width, surface.Height);
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

        private void CreateTexture()
        {
            var factory = _device.ResourceFactory;

            _texture?.Dispose();
            _texture = factory.CreateTexture(TextureDescription.Texture2D(
                (uint) _size.Width, (uint) _size.Height,
                1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            _textureView?.Dispose();
            _textureView = factory.CreateTextureView(_texture);

            ResourceSet?.Dispose();
            ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                ResourceLayout, _textureView, _sampler
            ));

            _surface?.Dispose();
            _surface = new ImageSurface(Format.Argb32, _size.Width, _size.Height);
        }

        private static byte[] Bgra32ToRgba32(IReadOnlyList<byte> source, int width, int height)
        {
            var destination = new byte[source.Count];

            for (var pixel = 0; pixel < source.Count; pixel += 4)
            {
                byte b = source[pixel],
                    g = source[pixel + 1],
                    r = source[pixel + 2],
                    a = source[pixel + 3];
                destination[pixel] = r;
                destination[pixel + 1] = g;
                destination[pixel + 2] = b;
                destination[pixel + 3] = a;
            }

            return destination;
        }
    }
}
