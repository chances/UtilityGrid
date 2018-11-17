using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS
{
    public class World : IReadOnlyList<Entity>
    {
        private readonly List<Entity> _entities = new List<Entity>();

        public IEnumerator<Entity> GetEnumerator() => _entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _entities.GetEnumerator();

        public int Count => _entities.Count;

        public Entity this[int index] => _entities[index];

        public void Add(Entity entity) => _entities.Add(entity);
    }
}
