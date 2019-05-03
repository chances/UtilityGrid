using System;
using System.Numerics;
using Engine;
using Engine.Components;
using Engine.Components.Receivers;
using Engine.Geometry;
using Engine.Input;

namespace Game.UI
{
    public class OrbitCamera : Camera, IKeyboardInput
    {
        private const float MinZoom = 1;
        private const float MaxZoom = 200;

        private const float ZoomPerSecond = 5;
        private readonly float OrbitPerSecond = 180f.DegToRad();

        public float Zoom { get; set; } = 3.0f;
        public Vector3 FocalPoint { get => LookAt; set => LookAt = value; }
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public KeyboardState KeyboardState { private get; set; }

        private bool ShouldZoomIn => KeyboardState.IsKeyDown(Key.Plus) || KeyboardState.IsKeyDown(Key.KeypadPlus);
        private bool ShouldZoomOut => KeyboardState.IsKeyDown(Key.Minus) || KeyboardState.IsKeyDown(Key.KeypadMinus);

        private bool ShouldOrbitLeft => KeyboardState.IsKeyDown(Key.Left) && !KeyboardState.IsKeyDown(Key.Right);
        private bool ShouldOrbitRight => KeyboardState.IsKeyDown(Key.Right) && !KeyboardState.IsKeyDown(Key.Left);

        public override void Update(GameTime gameTime)
        {
            var zoom = (float) gameTime.ElapsedGameTime.TotalSeconds * ZoomPerSecond;
            var orbit = (float) gameTime.ElapsedGameTime.TotalSeconds * OrbitPerSecond;

            var zoomIn = ShouldZoomIn && !ShouldZoomOut;
            var zoomOut = ShouldZoomOut && !ShouldZoomIn;

            zoom *= zoomIn ? -1 : zoomOut ? 1 : 0;
            Zoom = Math.Clamp(Zoom += zoom, MinZoom, MaxZoom);

            orbit *= ShouldOrbitLeft ? -1 : ShouldOrbitRight ? 1 : 0;
            Rotation += Quaternion.CreateFromAxisAngle(Vector3.UnitY, orbit);

            // Update camera's absolute position
            Position = new Vector3((float) Math.Cos(Rotation.Y) * Zoom, 0, (float) Math.Sin(Rotation.Y) * Zoom) + FocalPoint;

            base.Update(gameTime);
        }
    }
}
