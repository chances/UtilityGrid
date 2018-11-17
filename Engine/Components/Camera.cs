using System;
using System.Numerics;
using Engine.Components.Receivers;
using Engine.ECS;

namespace Engine.Components
{
    public class Camera : Component, IFramebufferSize, IUpdatable
    {
        private readonly Vector3 _position = new Vector3(15, 10, 10);
        // TODO: Implement tweener from MonoGame.Extended.Tween
//        TweeningComponent _tweener;

        public Camera() : base(nameof(Camera))
        {
//            _tweener = new TweeningComponent(game, new AnimationComponent(game));
        }

        public Tuple<uint, uint> FramebufferSize { get; set; }

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

        public void Update(GameTime time)
        {
            // TODO: Do tweening here with a tweener
        }
    }
}
