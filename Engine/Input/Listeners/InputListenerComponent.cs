using System.Collections.Generic;
using System.Linq;
using Engine.Components;
using Engine.Components.Receivers;
using Engine.ECS;

namespace Engine.Input.Listeners
{
    public class InputListenerComponent : Component, IUpdatable, IMouseInput, IKeyboardInput
    {
        public MouseState MouseState { private get; set; }
        public KeyboardState KeyboardState { private get; set; }

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

        public void Update(GameTime gameTime)
        {
            foreach (var mouseListener in _listeners.OfType<MouseListener>())
            {
                mouseListener.MouseState = MouseState;
                mouseListener.Update(gameTime);
            }

            foreach (var keyboardListener in _listeners.OfType<KeyboardListener>())
            {
                keyboardListener.KeyboardState = KeyboardState;
                keyboardListener.Update(gameTime);
            }

        }
    }
}
