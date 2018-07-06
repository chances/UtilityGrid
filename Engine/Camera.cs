using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Tweening;

namespace Engine
{
    public class Camera : GameComponent
    {
        private readonly GraphicsDevice _device;
        private Vector3 _position = new Vector3(15, 10, 10);
        TweeningComponent _tweener;

        public Camera(Game game, GraphicsDevice graphicsDevice) : base(game)
        {
            _device = graphicsDevice;
            _tweener = new TweeningComponent(game, new AnimationComponent(game));
        }

        public Matrix ViewMatrix
        {
            get
            {
                var lookAtVector = Vector3.Zero;
                var upVector = Vector3.UnitZ;

                return Matrix.CreateLookAt (
                    _position, lookAtVector, upVector);
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                var fieldOfView = MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 200;
                var aspectRatio = _device.Viewport.Width / (float)_device.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public override void Initialize()
        {
            _tweener.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _tweener.Update(gameTime);
        }
    }
}
