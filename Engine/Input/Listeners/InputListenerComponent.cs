using System.Collections.Generic;
using Engine.Components;

namespace Engine.Input.Listeners
{
    public class InputListenerComponent : Component
    {
        private readonly List<InputListener> _listeners;

        public InputListenerComponent(Game game)
            : base(game)
        {
            _listeners = new List<InputListener>();
        }

        public InputListenerComponent(Game game, params InputListener[] listeners)
            : base(game)
        {
            _listeners = new List<InputListener>(listeners);
        }

        public IList<InputListener> Listeners => _listeners;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Game.IsActive) return;
            foreach (var listener in _listeners)
            {
                if (listener is MouseListener mouseListener)
                    mouseListener.MouseState = Game.MouseState;
                if (listener is KeyboardListener keyboardListener)
                    keyboardListener.KeyboardState = Game.KeyboardState;

                listener.Update(gameTime);
            }

            // TODO: Support game pad input?
//            GamePadListener.CheckConnections();
        }
    }
}
