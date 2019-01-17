using System.Collections.Generic;
using System.Linq;

namespace Engine.Input
{
    public class KeyboardState
    {
        public KeyboardState(IReadOnlyList<Key> downKeys, IReadOnlyList<Key> upKeys, KeyboardModifiers modifiers)
        {
            DownKeys = downKeys;
            UpKeys = upKeys;
            KeyboardModifiers = modifiers;
        }

        public IReadOnlyList<Key> DownKeys { get; }
        public IReadOnlyList<Key> UpKeys { get; }
        public KeyboardModifiers KeyboardModifiers { get; }

        public bool IsKeyDown(Key key) => DownKeys.Contains(key);

        public bool IsKeyUp(Key key) => UpKeys.Contains(key);
    }
}
