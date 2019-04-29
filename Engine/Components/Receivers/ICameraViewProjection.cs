using System.Numerics;
using Engine.Input;

namespace Engine.Components.Receivers
{
    public interface ICameraViewProjection
    {
        Buffers.UniformBuffer<Matrix4x4> CameraViewProjection { set; }
    }
}
