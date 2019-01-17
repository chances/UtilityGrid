using System;
using System.Collections.Immutable;
using System.Linq;
using Engine.Input;
using Veldrid;
using Key = Engine.Input.Key;
using MouseButton = Engine.Input.MouseButton;

namespace Game
{
    public static class InputSnapshotConverter
    {
        public static MouseState Mouse(InputSnapshot snapshot, MouseState previousMouseState)
        {
            var downButtons = snapshot.MouseEvents.Where(m => m.Down).Select(m => MouseButton(m.MouseButton))
                .ToImmutableList();
            var upButtons = snapshot.MouseEvents.Where(m => !m.Down).Select(m => MouseButton(m.MouseButton))
                .ToImmutableList();
            var wheelValue = (int) Math.Round(snapshot.WheelDelta);

            return new MouseState(snapshot.MousePosition, downButtons, upButtons, wheelValue, previousMouseState);
        }

        private static MouseButton MouseButton(Veldrid.MouseButton button) => (MouseButton) (int) button;

        public static KeyboardState Keyboard(InputSnapshot snapshot)
        {
            var downKeys = snapshot.KeyEvents.Where(key => key.Down).Select(key => Key(key.Key)).ToImmutableList();
            var upKeys = snapshot.KeyEvents.Where(key => !key.Down).Select(key => Key(key.Key)).ToImmutableList();
            var keyModifiers = snapshot.KeyEvents.Where(key => key.Down)
                .Select(key => key.Modifiers)
                .Aggregate(Engine.Input.KeyboardModifiers.None,
                    (modifiers, keys) => modifiers | KeyboardModifiers(keys));

            return new KeyboardState(downKeys, upKeys, keyModifiers);
        }

        private static Key Key(Veldrid.Key key) => (Key) (int) key;

        private static KeyboardModifiers KeyboardModifiers(ModifierKeys modifierKeys) =>
            (KeyboardModifiers) (int) modifierKeys;
    }
}
