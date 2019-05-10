using Engine.Components.Receivers;
using Engine.ECS;
using Engine.Input;

namespace Engine.Systems
{
    public class KeyboardProvider : System<IKeyboardInput>
    {
        private KeyboardState _keyboardState;

        public KeyboardProvider(World world, KeyboardState keyboardState) : base(world)
        {
            _keyboardState = keyboardState;
        }

        public override void Operate()
        {
            foreach (var componentToUpdate in OperableComponents)
            {
                componentToUpdate.KeyboardState = _keyboardState;
            }
        }
    }
}
