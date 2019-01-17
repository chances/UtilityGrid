using System;

namespace Engine.Input.Listeners
{
    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEventArgs(Key key, KeyboardState keyboardState)
        {
            Key = key;

            Modifiers = KeyboardModifiers.None;

            if (keyboardState.IsKeyDown(Key.ControlLeft) || keyboardState.IsKeyDown(Key.ControlRight) ||
                keyboardState.IsKeyDown(Key.LControl) || keyboardState.IsKeyDown(Key.RControl))
                Modifiers |= KeyboardModifiers.Control;

            if (keyboardState.IsKeyDown(Key.ShiftLeft) || keyboardState.IsKeyDown(Key.ShiftRight) ||
                keyboardState.IsKeyDown(Key.LShift) || keyboardState.IsKeyDown(Key.RShift))
                Modifiers |= KeyboardModifiers.Shift;

            if (keyboardState.IsKeyDown(Key.AltLeft) || keyboardState.IsKeyDown(Key.AltRight) ||
                keyboardState.IsKeyDown(Key.LAlt) || keyboardState.IsKeyDown(Key.RAlt))
                Modifiers |= KeyboardModifiers.Alt;
        }

        public Key Key { get; }
        public KeyboardModifiers Modifiers { get; }

        public char? Character => ToChar(Key, Modifiers);

        private static char? ToChar(Key key, KeyboardModifiers modifiers = KeyboardModifiers.None)
        {
            var isShiftDown = (modifiers & KeyboardModifiers.Shift) == KeyboardModifiers.Shift;

            if (key == Key.A) return isShiftDown ? 'A' : 'a';
            if (key == Key.B) return isShiftDown ? 'B' : 'b';
            if (key == Key.C) return isShiftDown ? 'C' : 'c';
            if (key == Key.D) return isShiftDown ? 'D' : 'd';
            if (key == Key.E) return isShiftDown ? 'E' : 'e';
            if (key == Key.F) return isShiftDown ? 'F' : 'f';
            if (key == Key.G) return isShiftDown ? 'G' : 'g';
            if (key == Key.H) return isShiftDown ? 'H' : 'h';
            if (key == Key.I) return isShiftDown ? 'I' : 'i';
            if (key == Key.J) return isShiftDown ? 'J' : 'j';
            if (key == Key.K) return isShiftDown ? 'K' : 'k';
            if (key == Key.L) return isShiftDown ? 'L' : 'l';
            if (key == Key.M) return isShiftDown ? 'M' : 'm';
            if (key == Key.N) return isShiftDown ? 'N' : 'n';
            if (key == Key.O) return isShiftDown ? 'O' : 'o';
            if (key == Key.P) return isShiftDown ? 'P' : 'p';
            if (key == Key.Q) return isShiftDown ? 'Q' : 'q';
            if (key == Key.R) return isShiftDown ? 'R' : 'r';
            if (key == Key.S) return isShiftDown ? 'S' : 's';
            if (key == Key.T) return isShiftDown ? 'T' : 't';
            if (key == Key.U) return isShiftDown ? 'U' : 'u';
            if (key == Key.V) return isShiftDown ? 'V' : 'v';
            if (key == Key.W) return isShiftDown ? 'W' : 'w';
            if (key == Key.X) return isShiftDown ? 'X' : 'x';
            if (key == Key.Y) return isShiftDown ? 'Y' : 'y';
            if (key == Key.Z) return isShiftDown ? 'Z' : 'z';

            if (key == Key.Number0 && !isShiftDown || key == Key.Keypad0) return '0';
            if (key == Key.Number1 && !isShiftDown || key == Key.Keypad1) return '1';
            if (key == Key.Number2 && !isShiftDown || key == Key.Keypad2) return '2';
            if (key == Key.Number3 && !isShiftDown || key == Key.Keypad3) return '3';
            if (key == Key.Number4 && !isShiftDown || key == Key.Keypad4) return '4';
            if (key == Key.Number5 && !isShiftDown || key == Key.Keypad5) return '5';
            if (key == Key.Number6 && !isShiftDown || key == Key.Keypad6) return '6';
            if (key == Key.Number7 && !isShiftDown || key == Key.Keypad7) return '7';
            if (key == Key.Number8 && !isShiftDown || key == Key.Keypad8) return '8';
            if (key == Key.Number9 && !isShiftDown || key == Key.Keypad9) return '9';

            if (key == Key.Number0 && isShiftDown) return ')';
            if (key == Key.Number1 && isShiftDown) return '!';
            if (key == Key.Number2 && isShiftDown) return '@';
            if (key == Key.Number3 && isShiftDown) return '#';
            if (key == Key.Number4 && isShiftDown) return '$';
            if (key == Key.Number5 && isShiftDown) return '%';
            if (key == Key.Number6 && isShiftDown) return '^';
            if (key == Key.Number7 && isShiftDown) return '&';
            if (key == Key.Number8 && isShiftDown) return '*';
            if (key == Key.Number9 && isShiftDown) return '(';

            if (key == Key.Space) return ' ';
            if (key == Key.Tab) return '\t';
            if (key == Key.Enter) return (char) 13;
            if (key == Key.Back) return (char) 8;

            if (key == Key.KeypadAdd) return '+';
            if (key == Key.KeypadDecimal) return '.';
            if (key == Key.KeypadDivide) return '/';
            if (key == Key.KeypadMultiply) return '*';
            if (key == Key.NonUSBackSlash) return '\\';
            if (key == Key.Comma && !isShiftDown) return ',';
            if (key == Key.Comma && isShiftDown) return '<';
            if ((key == Key.BracketLeft || key == Key.LBracket) && !isShiftDown) return '[';
            if ((key == Key.BracketLeft || key == Key.LBracket) && isShiftDown) return '{';
            if ((key == Key.BracketRight || key == Key.RBracket) && !isShiftDown) return ']';
            if ((key == Key.BracketRight || key == Key.RBracket) && isShiftDown) return '}';
            if (key == Key.Period && !isShiftDown) return '.';
            if (key == Key.Period && isShiftDown) return '>';
            if (key == Key.BackSlash && !isShiftDown) return '\\';
            if (key == Key.BackSlash && isShiftDown) return '|';
            if (key == Key.Plus && !isShiftDown) return '=';
            if (key == Key.Plus && isShiftDown) return '+';
            if (key == Key.Minus && !isShiftDown) return '-';
            if (key == Key.Minus && isShiftDown) return '_';
            if (key == Key.Slash && !isShiftDown) return '/';
            if (key == Key.Slash && isShiftDown) return '?';
            if (key == Key.Quote && !isShiftDown) return '\'';
            if (key == Key.Quote && isShiftDown) return '"';
            if (key == Key.Semicolon && !isShiftDown) return ';';
            if (key == Key.Semicolon && isShiftDown) return ':';
            if (key == Key.Tilde && !isShiftDown) return '`';
            if (key == Key.Tilde && isShiftDown) return '~';

            return null;
        }
    }
}
