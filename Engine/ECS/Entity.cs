using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Engine.Components;

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

        public Tags Tags { get; private set; } = 0;

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

        public void AddTag(Tags tag)
        {
            Tags |= tag;

            Console.WriteLine($"{this.Id} tags:");
            foreach (var presentTag in Enum.GetValues(typeof(Components.Tags)))
            {
                if (presentTag is Tags t && Tags.HasFlag(t)) {
                    Console.WriteLine(presentTag.ToString());
                }
            }
            Console.WriteLine();
        }

        public bool HasTag(Tags tag) => Tags.HasFlag(tag);

        public bool HasComponent<T>() => _components.Values.Any(component => component is T);

        public bool HasComponentsOfTypes(params Type[] types) =>
            types.All(type =>
                _components.Values.Any(component => type.IsInstanceOfType(component))
            );

        public IEnumerable<T> GetComponents<T>() => _components.Values.OfType<T>();

        public T GetComponent<T>() => GetComponents<T>().FirstOrDefault();
    }
}
