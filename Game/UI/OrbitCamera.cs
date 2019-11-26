using System;
using System.Numerics;
using Engine;
using Engine.Components;
using Engine.Components.Geometry;
using Engine.Components.Receivers;
using Engine.Geometry;
using Engine.Input;

namespace Game.UI
{
    public class OrbitCamera : Camera, IKeyboardInput
    {
        private const float MinZoom = 1;
        private const float MaxZoom = 200;
        private readonly float MinTilt = 5f.DegToRad();
        private readonly float MaxTilt = 85f.DegToRad();

        private const float ZoomPerSecond = 5;
        private readonly float OrbitPerSecond = 180f.DegToRad();

        private float _zoom = 3.0f;
        private Vector3 _yawPitchRoll = Vector3.Zero;

        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                UpdatePosition(YawPitchRoll);
            }
        }
        public Vector3 FocalPoint { get => LookAt; set => LookAt = value; }
        public Vector3 YawPitchRoll
        {
            get => _yawPitchRoll;
            set
            {
                _yawPitchRoll = value;
                UpdatePosition(YawPitchRoll);
            }
        }
        public KeyboardState KeyboardState { private get; set; }

        private bool ShouldZoomIn => KeyboardState.IsKeyDown(Key.Plus) || KeyboardState.IsKeyDown(Key.KeypadPlus);
        private bool ShouldZoomOut => KeyboardState.IsKeyDown(Key.Minus) || KeyboardState.IsKeyDown(Key.KeypadMinus);

        private bool ShouldOrbitLeft => KeyboardState.IsKeyDown(Key.Left) && !KeyboardState.IsKeyDown(Key.Right);
        private bool ShouldOrbitRight => KeyboardState.IsKeyDown(Key.Right) && !KeyboardState.IsKeyDown(Key.Left);
        private bool ShouldOrbitUp => KeyboardState.IsKeyDown(Key.Up) && !KeyboardState.IsKeyDown(Key.Down);
        private bool ShouldOrbitDown => KeyboardState.IsKeyDown(Key.Down) && !KeyboardState.IsKeyDown(Key.Up);

        public override void Update(GameTime gameTime)
        {
            var zoom = (float)gameTime.ElapsedGameTime.TotalSeconds * ZoomPerSecond;
            var orbit = (float)gameTime.ElapsedGameTime.TotalSeconds * OrbitPerSecond;

            var zoomIn = ShouldZoomIn && !ShouldZoomOut;
            var zoomOut = ShouldZoomOut && !ShouldZoomIn;

            zoom *= zoomIn ? -1 : zoomOut ? 1 : 0;
            Zoom = Math.Clamp(Zoom += zoom, MinZoom, MaxZoom);

            var orbitY = orbit * (ShouldOrbitLeft ? -1 : ShouldOrbitRight ? 1 : 0);
            var orbitX = orbit * (ShouldOrbitDown ? -1 : ShouldOrbitUp ? 1 : 0);
            YawPitchRoll += new Vector3(orbitX, orbitY, 0);
            YawPitchRoll = new Vector3(
                Math.Clamp(YawPitchRoll.X, MinTilt, MaxTilt), YawPitchRoll.Y, YawPitchRoll.Z);

            if (KeyboardState.IsKeyDown(Key.Home))
                YawPitchRoll = Vector3.Zero;

            base.Update(gameTime);
        }

        private void UpdatePosition(Vector3 yawPitchRoll)
        {
            var transformation = Matrix4x4.Identity;
            transformation.Translation = new Vector3(0, 0, Zoom * -1);
            transformation = Matrix4x4.Transform(transformation, Quaternion.CreateFromYawPitchRoll(
                yawPitchRoll.Y,
                yawPitchRoll.X,
                yawPitchRoll.Z
            ));

            Position = Vector3.Transform(FocalPoint, transformation);
        }
    }
}
