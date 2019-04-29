using System;
using System.Numerics;
using Engine.Buffers;
using Engine.Components;
using Engine.Components.Receivers;
using Engine.ECS;
using JetBrains.Annotations;
using Veldrid;

namespace Game.Buildings
{
    public class Building : ResourceComponent, IDependencies, IResourceSet, ICameraViewProjection
    {
        private UniformBuffer<Matrix4x4> _viewProj;
        private UniformColor _color;

        public Building() : base("Building")
        {
            Resources.OnInitialize = (factory, device) => {
                _color = new UniformColor(RgbaFloat.Orange);
                _color.Buffer.Initialize(factory, device);

                ResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription[]
                    {
                        UniformViewProjection.ResourceLayout,
                        _color.LayoutDescription
                    }
                ));

                ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                    ResourceLayout,
                    _viewProj.DeviceBuffer,
                    _color.Buffer.DeviceBuffer
                ));
            };
            Resources.OnDispose = () => {
                _color.Buffer.Dispose();
                ResourceLayout.Dispose();
                ResourceSet.Dispose();
            };
        }

        public bool AreDependenciesSatisfied => _viewProj != null;

        public UniformBuffer<Matrix4x4> CameraViewProjection
        {
            set => _viewProj = value;
        }

        public ResourceLayout ResourceLayout { get; private set; }

        public ResourceSet ResourceSet { get; private set; }
    }
}
