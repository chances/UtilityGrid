using System.Numerics;
using Engine.Buffers.Uniforms;
using Engine.Input;

namespace Engine.Components.Receivers
{
    public interface ICameraViewProjection
    {
        UniformBuffer<Matrix4x4> CameraViewProjection { set; }
    }
}
