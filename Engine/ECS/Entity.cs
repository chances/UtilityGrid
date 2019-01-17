using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS
{
    public class Entity : IReadOnlyDictionary<string, Component>
    {
        private readonly Dictionary<string, Component> _components = new Dictionary<string, Component>();

        public Entity()
        {
        }

        public Entity(IEnumerable<Component> components)
        {
            foreach (var component in components)
            {
                Add(component);
            }
        }

        public Guid Id { get; } = Guid.NewGuid();

        public IEnumerator<KeyValuePair<string, Component>> GetEnumerator() => _components.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _components.GetEnumerator();

        public int Count => _components.Count;
        public bool ContainsKey(string key) => _components.ContainsKey(key);

        public bool TryGetValue(string key, out Component value) => _components.TryGetValue(key, out value);

        public Component this[string key] => _components[key];

        public IEnumerable<string> Keys => _components.Keys;
        public IEnumerable<Component> Values => _components.Values;

        public void Add(Component component)
        {
            if (_components.ContainsKey(component.Name) || _components.Values.Contains(component))
                _components[component.Name] = component;
            else
                _components.Add(component.Name, component);
        }

        public bool HasComponentOfType(Type type) => _components.Values.Any(component => component.GetType() == type);

        public T GetComponent<T>() => _components.Values.OfType<T>().FirstOrDefault();
    }
}
