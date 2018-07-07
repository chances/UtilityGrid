using System;
using System.Numerics;

namespace Engine.Input.Listeners
{
    public class MouseListener : InputListener
    {
        private bool _dragging;
        private GameTime _gameTime;
        private bool _hasDoubleClicked;
        private MouseEventArgs _mouseDownArgs;
        private MouseEventArgs _previousClickArgs;
        private MouseState _previousState;

        public MouseListener()
            : this(new MouseListenerSettings())
        {
        }

        public MouseListener(MouseListenerSettings settings)
        {
            DoubleClickMilliseconds = settings.DoubleClickMilliseconds;
            DragThreshold = settings.DragThreshold;
        }

        public MouseState MouseState { private get; set; }
        public int DoubleClickMilliseconds { get; }
        public int DragThreshold { get; }

        /// <summary>
        ///     Returns true if the mouse has moved between the current and previous frames.
        /// </summary>
        /// <value><c>true</c> if the mouse has moved; otherwise, <c>false</c>.</value>
        public bool HasMouseMoved => _previousState != null &&
                                     (_previousState.X != MouseState.X || _previousState.Y != MouseState.Y);

        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseClicked;
        public event EventHandler<MouseEventArgs> MouseDoubleClicked;
        public event EventHandler<MouseEventArgs> MouseMoved;
        public event EventHandler<MouseEventArgs> MouseWheelMoved;
        public event EventHandler<MouseEventArgs> MouseDragStart;
        public event EventHandler<MouseEventArgs> MouseDrag;
        public event EventHandler<MouseEventArgs> MouseDragEnd;

        private void CheckButtonPressed(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if (getButtonState(MouseState) != ButtonState.Pressed ||
                getButtonState(_previousState) != ButtonState.Released) return;
            var args = new MouseEventArgs(_gameTime.TotalGameTime, _previousState, MouseState, button);

            MouseDown?.Invoke(this, args);
            _mouseDownArgs = args;

            if (_previousClickArgs == null) return;
            // If the last click was recent
            var clickMilliseconds = (args.Time - _previousClickArgs.Time).TotalMilliseconds;

            if (clickMilliseconds <= DoubleClickMilliseconds)
            {
                MouseDoubleClicked?.Invoke(this, args);
                _hasDoubleClicked = true;
            }

            _previousClickArgs = null;
        }

        private void CheckButtonReleased(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if (getButtonState(MouseState) == ButtonState.Released &&
                getButtonState(_previousState) == ButtonState.Pressed)
            {
                var args = new MouseEventArgs(_gameTime.TotalGameTime, _previousState, MouseState, button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                    // If the mouse hasn't moved much between mouse down and mouse up
                    if (clickMovement < DragThreshold)
                    {
                        if (!_hasDoubleClicked)
                            MouseClicked?.Invoke(this, args);
                    }
                    else // If the mouse has moved between mouse down and mouse up
                    {
                        MouseDragEnd?.Invoke(this, args);
                        _dragging = false;
                    }
                }

                MouseUp?.Invoke(this, args);

                _hasDoubleClicked = false;
                _previousClickArgs = args;
            }
        }

        private void CheckMouseDragged(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if (getButtonState(MouseState) == ButtonState.Pressed &&
                getButtonState(_previousState) == ButtonState.Pressed)
            {
                var args = new MouseEventArgs(_gameTime.TotalGameTime, _previousState, MouseState,
                    button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    if (_dragging)
                    {
                        MouseDrag?.Invoke(this, args);
                    }
                    else
                    {
                        // Only start to drag based on DragThreshold
                        var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                        if (clickMovement > DragThreshold)
                        {
                            _dragging = true;
                            MouseDragStart?.Invoke(this, args);
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            CheckButtonPressed(s => s.LeftButton, MouseButton.Left);
            CheckButtonPressed(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonPressed(s => s.RightButton, MouseButton.Right);

            CheckButtonReleased(s => s.LeftButton, MouseButton.Left);
            CheckButtonReleased(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonReleased(s => s.RightButton, MouseButton.Right);

            // Check for any sort of mouse movement.
            if (HasMouseMoved)
            {
                MouseMoved?.Invoke(this,
                    new MouseEventArgs(gameTime.TotalGameTime, _previousState, MouseState));

                CheckMouseDragged(s => s.LeftButton, MouseButton.Left);
                CheckMouseDragged(s => s.MiddleButton, MouseButton.Middle);
                CheckMouseDragged(s => s.RightButton, MouseButton.Right);
            }

            // Handle mouse wheel events.
            if (_previousState.ScrollWheelValue != MouseState.ScrollWheelValue)
                MouseWheelMoved?.Invoke(this,
                    new MouseEventArgs(gameTime.TotalGameTime, _previousState, MouseState));

            _previousState = MouseState;
        }

        private static int DistanceBetween(Vector2 a, Vector2 b)
        {
            return (int) (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
        }
    }
}
