using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Components
{
    public abstract class Component : IUpdatable, IRenderable
    {
        protected Game _game;
        private ICollection<Component> _children;

        public Component(Game game)
        {
            _game = game;
            _children = new List<Component>();
        }

        public ICollection<Component> Children => _children;

        public virtual void Initialize()
        {
            foreach (var component in _children)
            {
                component.Initialize();
            }
        }

        public virtual void Update(GameTime time)
        {
            foreach (var component in _children)
            {
                component.Update(time);
            }
        }

        public virtual void Render()
        {
            foreach (var component in _children)
            {
                component.Render();
            }
        }
    }
}
