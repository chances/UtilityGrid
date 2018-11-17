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

        public List<Entity> this[IEnumerable<Type> componentTypes] =>
            _entities.Where(entity => componentTypes.All(entity.HasComponentOfType)).ToList();

        public void Add(Entity entity) => _entities.Add(entity);

        public List<Entity> EntitiesWith(IEnumerable<Type> componentTypes) => this[componentTypes];

        public List<Entity> OperableEntitiesFor(System system) => _entities.Where(system.CanOperateOn).ToList();

        public List<Component> OperableComponentsFor(System system, Type componentType) =>
            OperableEntitiesFor(system).Select(entity => entity.GetComponent(componentType)).ToList();
    }
}
