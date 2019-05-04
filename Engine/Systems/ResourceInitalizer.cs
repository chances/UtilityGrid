using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Buffers;
using Engine.Components;
using Engine.ECS;
using Veldrid;
using InitializeAction = System.Action<Veldrid.ResourceFactory, Veldrid.GraphicsDevice>;

namespace Engine.Systems
{
    public class ResourceInitializer : ECS.System
    {
        private readonly ResourceFactory _factory;
        private readonly GraphicsDevice _device;

        public ResourceInitializer(World world, ResourceFactory factory, GraphicsDevice device) : base(world)
        {
            _factory = factory;
            _device = device;
        }

        private new IEnumerable<Entity> OperableEntities => World
            .Where(entity => {
                var hasResources = (entity.HasComponent<IResource>());
                var isEntityInitialized = entity.HasTag(Tags.Initialized);
                var areAnyResourcesUninitialized = entity.HasComponent<IResource>() &&
                    entity.GetComponents<IResource>().Any(component => !component.Initialized);

                return hasResources && (!isEntityInitialized || areAnyResourcesUninitialized);
            });

        private Dictionary<Entity, IEnumerable<InitializeAction>> OperableEntityInitializers =>
            OperableEntities.ToDictionary(
                entity => entity,
                entity => entity.GetComponents<IResource>().Select<IResource, InitializeAction>(resource =>
            {
                // Only init device resources when dependencies are satisfied
                if (resource is IDependencies dependant && !dependant.AreDependenciesSatisfied)
                {
                    return (a, b) => { }; // No-op
                }

                return resource.Initialize;
            }));

        public override void Operate()
        {
            foreach (var entityAndInitializers in OperableEntityInitializers)
            {
                var entity = entityAndInitializers.Key;

                foreach (var initializeAction in entityAndInitializers.Value)
                {
                    initializeAction.Invoke(_factory, _device);
                }

                var isEntityInitialized = entity.Values.OfType<IResource>()
                    .Aggregate(true, (isInitialized, resource) => isInitialized && resource.Initialized);
                if (isEntityInitialized)
                {
                    entity.AddTag(Tags.Initialized);
                }
            }
        }
    }
}
