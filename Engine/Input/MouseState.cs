using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Engine.Input
{
    public class MouseState
    {
        private readonly MouseState _previousMouseState;

        public MouseState(Vector2 position, IReadOnlyList<MouseButton> downButtons,
            IReadOnlyList<MouseButton> upButtons, int scrollWheelValue, MouseState previousMouseState = null)
        {
            Position = position;
            var buttonsStillHeldDown =
                previousMouseState?.DownButtons.Where(button => !upButtons.Contains(button)).ToList();
            DownButtons = buttonsStillHeldDown?.Concat(downButtons).ToList() ?? downButtons;
            UpButtons = upButtons;
            ScrollWheelValue = scrollWheelValue;
            _previousMouseState = previousMouseState;
        }

        public Vector2 Position { get; }
        public bool PositionChanged => _previousMouseState != null && Position != _previousMouseState.Position;
        public int X => (int) Position.X;
        public int Y => (int) Position.Y;
        public int DeltaX => _previousMouseState == null ? 0 : _previousMouseState.X - X;
        public int DeltaY => _previousMouseState == null ? 0 : _previousMouseState.Y - Y;
        public Vector2 DeltaPosition => new Vector2(DeltaX, DeltaY);

        public IReadOnlyList<MouseButton> DownButtons { get; }
        public IReadOnlyList<MouseButton> UpButtons { get; }
        public int ScrollWheelValue { get; }
        public int DeltaScrollWheelValue => _previousMouseState?.ScrollWheelValue - ScrollWheelValue ?? 0;

        public ButtonState LeftButton => IsButtonDown(MouseButton.Left) ? ButtonState.Pressed : ButtonState.Released;

        public ButtonState MiddleButton =>
            IsButtonDown(MouseButton.Middle) ? ButtonState.Pressed : ButtonState.Released;

        public ButtonState RightButton => IsButtonDown(MouseButton.Right) ? ButtonState.Pressed : ButtonState.Released;

        public bool IsButtonDown(MouseButton button)
        {
            return DownButtons.Contains(button);
        }

        public bool IsButtonUp(MouseButton button)
        {
            return UpButtons.Contains(button) || !DownButtons.Contains(button);
        }

        public bool WasButtonJustPressed(MouseButton button)
        {
            if (_previousMouseState == null) return false;
            return _previousMouseState.IsButtonUp(button) && IsButtonDown(button);
        }

        public bool WasButtonJustReleased(MouseButton button)
        {
            return UpButtons.Contains(button);
        }
    }
}
