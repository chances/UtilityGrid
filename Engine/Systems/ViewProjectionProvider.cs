using System;
using System.Drawing;
using System.Numerics;
using Engine.Components.Receivers;
using Engine.ECS;
using Veldrid;

namespace Engine.Systems
{
    public class ViewProjectionProvider : System<ICameraViewProjection>
    {
        private Buffers.UniformBuffer<Matrix4x4> _cameraViewProjection;

        public ViewProjectionProvider(World world, Buffers.UniformBuffer<Matrix4x4> cameraViewProjection) : base(world)
        {
            _cameraViewProjection = cameraViewProjection;
        }

        public override void Operate()
        {
            foreach (var componentToUpdate in OperableComponents)
            {
                componentToUpdate.CameraViewProjection = _cameraViewProjection;
            }
        }
    }
}
