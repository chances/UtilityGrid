using System;
using System.Drawing;
using System.Numerics;
using Engine.Buffers;
using Engine.Components.Receivers;
using Engine.ECS;
using Veldrid;

namespace Engine.Components
{
    public class Camera : ResourceComponent, IFramebufferSize, IUpdatable
    {
        private Vector3 _position = Vector3.UnitZ * 5f;

        private UniformViewProjection _viewProj;
        // TODO: Implement tweener from MonoGame.Extended.Tween
//        TweeningComponent _tweener;

        public Camera() : base(nameof(Camera))
        {
//            _tweener = new TweeningComponent(game, new AnimationComponent(game));

            Resources.OnInitialize = (factory, device) => {
                _viewProj = new UniformViewProjection(ViewProjection);
                _viewProj.Buffer.Initialize(factory, device);

                Resources.OnDispose = _viewProj.Buffer.Dispose;
            };
        }

        public Size FramebufferSize { get; set; } = new Size(960, 540);

        public Matrix4x4 ViewMatrix
        {
            get
            {
                var lookAtVector = Vector3.Zero;
                var upVector = Vector3.UnitY;

                return Matrix4x4.CreateLookAt(_position, lookAtVector, upVector);
            }
        }

        public Matrix4x4 ProjectionMatrix
        {
            get
            {
                var fieldOfView = (float) Math.PI / 4.0f; // 45 degrees
                float nearClipPlane = 1;
                float farClipPlane = 200;
                var aspectRatio = FramebufferSize.Width / (float) FramebufferSize.Height;

                return Matrix4x4.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public UniformBuffer<Matrix4x4> ViewProjectionUniform => _viewProj.Buffer;

        private Matrix4x4 ViewProjection => Matrix4x4.Multiply(ViewMatrix, ProjectionMatrix);

        public void Update(GameTime gameTime)
        {
            // TODO: Do tweening here with a tweener

            _viewProj.Buffer.UniformData = ViewProjection;
            _viewProj.Buffer.Update();
        }
    }
}
