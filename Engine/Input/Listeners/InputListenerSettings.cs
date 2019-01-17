namespace Engine.Input.Listeners
{
    public abstract class InputListenerSettings<T>
        where T : InputListener
    {
        public abstract T CreateListener();
    }
}
