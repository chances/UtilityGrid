using Engine.Input;

namespace Engine.ECS.Components.Receivers
{
    public interface IKeyboardInputReceiver
    {
        void Update(GameTime gameTime, KeyboardState keyboardState);
    }
}
