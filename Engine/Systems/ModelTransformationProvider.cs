using System.Linq;
using Engine.Components.Geometry;
using Engine.Components.Receivers;
using Engine.ECS;

namespace Engine.Systems
{
    public class ModelTransformationProvider : System<IModelTransformation>
    {
        public ModelTransformationProvider(World world) : base(world)
        {
        }

        public override void Operate()
        {
            foreach (var model in OperableEntities.Where(entity => entity.HasComponent<Transformation>()))
            {
                var transformation = model.GetComponent<Transformation>().Value;
                model.GetComponent<IModelTransformation>().ModelTransformation = transformation;
            }
        }
    }
}
