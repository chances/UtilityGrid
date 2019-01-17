using System;
using System.Numerics;

namespace Engine.Input.Listeners
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(TimeSpan time, MouseState previousState, MouseState currentState,
            MouseButton? button = null)
        {
            PreviousState = previousState;
            CurrentState = currentState;
            Position = currentState.Position;
            Button = button;
            ScrollWheelValue = currentState.ScrollWheelValue;
            ScrollWheelDelta = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            Time = time;
        }

        public TimeSpan Time { get; }

        public MouseState PreviousState { get; }
        public MouseState CurrentState { get; }
        public Vector2 Position { get; }
        public MouseButton? Button { get; }
        public int ScrollWheelValue { get; }
        public int ScrollWheelDelta { get; }

        public Vector2 DistanceMoved => CurrentState.Position - PreviousState.Position;
    }
}
