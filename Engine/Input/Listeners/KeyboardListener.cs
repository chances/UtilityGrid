using System;
using System.Linq;

namespace Engine.Input.Listeners
{
    public class KeyboardListener : InputListener
    {
        private bool _isInitial;
        private TimeSpan _lastPressTime;

        private Key _previousKey;
        private KeyboardState _previousState;

        public KeyboardListener()
            : this(new KeyboardListenerSettings())
        {
        }

        public KeyboardListener(KeyboardListenerSettings settings)
        {
            RepeatPress = settings.RepeatPress;
            InitialDelay = settings.InitialDelayMilliseconds;
            RepeatDelay = settings.RepeatDelayMilliseconds;
        }

        public KeyboardState KeyboardState { private get; set; }
        public bool RepeatPress { get; }
        public int InitialDelay { get; }
        public int RepeatDelay { get; }

        public event EventHandler<KeyboardEventArgs> KeyTyped;
        public event EventHandler<KeyboardEventArgs> KeyPressed;
        public event EventHandler<KeyboardEventArgs> KeyReleased;

        public override void Update(GameTime gameTime)
        {
            RaisePressedEvents(gameTime, KeyboardState);
            RaiseReleasedEvents(KeyboardState);

            if (RepeatPress)
                RaiseRepeatEvents(gameTime, KeyboardState);

            _previousState = KeyboardState;
        }

        private void RaisePressedEvents(GameTime gameTime, KeyboardState currentState)
        {
            if (!currentState.IsKeyDown(Key.AltLeft) && !currentState.IsKeyDown(Key.AltRight))
            {
                var pressedKeys = Enum.GetValues(typeof(Key))
                    .Cast<Key>()
                    .Where(key => currentState.IsKeyDown(key) && _previousState.IsKeyUp(key));

                foreach (var key in pressedKeys)
                {
                    var args = new KeyboardEventArgs(key, currentState);

                    KeyPressed?.Invoke(this, args);

                    if (args.Character.HasValue)
                        KeyTyped?.Invoke(this, args);

                    _previousKey = key;
                    _lastPressTime = gameTime.TotalGameTime;
                    _isInitial = true;
                }
            }
        }

        private void RaiseReleasedEvents(KeyboardState currentState)
        {
            var releasedKeys = Enum.GetValues(typeof(Key))
                .Cast<Key>()
                .Where(key => currentState.IsKeyUp(key) && _previousState.IsKeyDown(key));

            foreach (var key in releasedKeys)
                KeyReleased?.Invoke(this, new KeyboardEventArgs(key, currentState));
        }

        private void RaiseRepeatEvents(GameTime gameTime, KeyboardState currentState)
        {
            var elapsedTime = (gameTime.TotalGameTime - _lastPressTime).TotalMilliseconds;

            if (currentState.IsKeyDown(_previousKey) &&
                (_isInitial && elapsedTime > InitialDelay || !_isInitial && elapsedTime > RepeatDelay))
            {
                var args = new KeyboardEventArgs(_previousKey, currentState);

                KeyPressed?.Invoke(this, args);

                if (args.Character.HasValue)
                    KeyTyped?.Invoke(this, args);

                _lastPressTime = gameTime.TotalGameTime;
                _isInitial = false;
            }
        }
    }
}
