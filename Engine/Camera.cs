using System;
using System.Numerics;
using Engine.Components;
using Veldrid;

namespace Engine
{
    public class Camera : Component
    {
        private readonly GraphicsDevice _device;
        private Vector3 _position = new Vector3(15, 10, 10);
        // TODO: Implement tweener from MonoGame.Extended.Tween
//        TweeningComponent _tweener;

        public Camera(Game game) : base(game)
        {
            _device = game.GraphicsDevice;
//            _tweener = new TweeningComponent(game, new AnimationComponent(game));
        }

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
                var aspectRatio = _device.SwapchainFramebuffer.Width / (float) _device.SwapchainFramebuffer.Height;

                return Matrix4x4.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }
    }
}
