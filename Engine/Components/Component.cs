using System.Collections.Generic;

namespace Engine.Components
{
    public abstract class Component : IUpdatable, IRenderable
    {
        public Component(Game game)
        {
            Game = game;
            Children = new List<Component>();
        }

        public Game Game { get; }
        public ICollection<Component> Children { get; }

        public virtual void Initialize()
        {
            foreach (var component in Children) component.Initialize();
        }

        public virtual void Update(GameTime time)
        {
            foreach (var component in Children) component.Update(time);
        }
        
        public virtual void Render()
        {
            foreach (var component in Children) component.Render();
        }
    }
}
