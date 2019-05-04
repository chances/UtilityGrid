using System;
using System.Numerics;
using Engine.Buffers.Uniforms;
using Engine.Components;
using Engine.Components.Receivers;
using Engine.ECS;
using Veldrid;

namespace Game.Buildings
{
    public class Building : ResourceComponent, IDependencies, IResourceSet, IModelTransformation, ICameraViewProjection
    {
        private UniformModelTransformation _model =
            new UniformModelTransformation(Matrix4x4.Identity);
        private UniformColor _color;

        public Building() : base("Building")
        {
            Resources.OnInitialize = (factory, device) => {
                _model.Buffer.Initialize(factory, device);
                _color = new UniformColor(RgbaFloat.Orange);
                _color.Buffer.Initialize(factory, device);

                ResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription[]
                    {
                        UniformModelTransformation.ResourceLayout,
                        UniformViewProjection.ResourceLayout,
                        _color.LayoutDescription
                    }
                ));

                ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                    ResourceLayout,
                    _model.Buffer.DeviceBuffer,
                    CameraViewProjection.DeviceBuffer,
                    _color.Buffer.DeviceBuffer
                ));
            };
            Resources.OnDispose = () => {
                _color.Buffer.Dispose();
                ResourceLayout.Dispose();
                ResourceSet.Dispose();
            };
        }

        public bool AreDependenciesSatisfied => CameraViewProjection != null;

        public Matrix4x4 ModelTransformation
        {
            set => _model.Buffer.UniformData = value;
        }

        public UniformBuffer<Matrix4x4> CameraViewProjection { get; set; }

        public ResourceLayout ResourceLayout { get; private set; }

        public ResourceSet ResourceSet { get; private set; }
    }
}
