namespace Engine.ECS
{
    public abstract class Component
    {
        protected Component(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
