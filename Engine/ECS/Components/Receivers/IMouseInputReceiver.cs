using Engine.Input;

namespace Engine.ECS.Components.Receivers
{
    public interface IMouseInputReceiver
    {
        void Update(GameTime gameTime, MouseState mouseState);
    }
}
