using System;
using System.Numerics;
using Engine.Buffers;
using Engine.Components.Receivers;
using Engine.ECS;
using Veldrid;

namespace Engine.Components
{
    public class Camera : Component, IResource, IFramebufferSize, IUpdatable
    {
        private readonly Vector3 _position = new Vector3(15, 10, 10);

        private UniformBuffer<UniformViewProjection> _viewProjUniform;
        // TODO: Implement tweener from MonoGame.Extended.Tween
//        TweeningComponent _tweener;

        public Camera() : base(nameof(Camera))
        {
//            _tweener = new TweeningComponent(game, new AnimationComponent(game));
        }

        public static Entity CreateEntity() => new Entity(new[] {new Camera()});

        public Tuple<uint, uint> FramebufferSize { get; set; } = new Tuple<uint, uint>(960, 540);

        public Matrix4x4 ViewMatrix
        {
            get
            {
                var lookAtVector = Vector3.Zero;
                var upVector = Vector3.UnitZ;

                return Matrix4x4.CreateLookAt(_position, lookAtVector, upVector);
            }
        }

        public Matrix4x4 ProjectionMatrix
        {
            get
            {
                var fieldOfView = (float) Math.PI / 4.0f;
                float nearClipPlane = 1;
                float farClipPlane = 200;
                var framebufferWidth = FramebufferSize.Item1;
                var framebufferHeight = FramebufferSize.Item2;
                var aspectRatio = framebufferWidth / (float) framebufferHeight;

                return Matrix4x4.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        private UniformViewProjection ViewProjection =>
            new UniformViewProjection(Matrix4x4.Multiply(ViewMatrix, ProjectionMatrix));

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            _viewProjUniform = new UniformBuffer<UniformViewProjection>(ViewProjection);
            _viewProjUniform.Initialize(factory, device);
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Do tweening here with a tweener

            _viewProjUniform.UniformData = ViewProjection;
            _viewProjUniform.Update();
        }

        public void Dispose()
        {
            _viewProjUniform.Dispose();
        }
    }
}
