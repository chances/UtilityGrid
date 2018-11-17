using System.Collections.Generic;
using System.Linq;
using Engine.ECS;
using Engine.ECS.Components;
using Engine.ECS.Components.Receivers;

namespace Engine.Input.Listeners
{
    public class InputListenerComponent : Component, IMouseInputReceiver, IKeyboardInputReceiver
    {
        private readonly List<InputListener> _listeners;

        public InputListenerComponent(string name)
            : this(name, new InputListener[0])
        {
        }

        public InputListenerComponent(string name, params InputListener[] listeners)
            : base($"InputListener-{name}")
        {
            _listeners = new List<InputListener>(listeners);
        }

        public IList<InputListener> Listeners => _listeners;

        // TODO: Support game pad input?
//        GamePadListener.CheckConnections();

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            foreach (var mouseListener in _listeners.OfType<MouseListener>())
            {
                mouseListener.MouseState = mouseState;
                mouseListener.Update(gameTime);
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            foreach (var keyboardListener in _listeners.OfType<KeyboardListener>())
            {
                keyboardListener.KeyboardState = keyboardState;
                keyboardListener.Update(gameTime);
            }
        }
    }
}
