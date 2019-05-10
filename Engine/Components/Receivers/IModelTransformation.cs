using System.Numerics;
using Engine.Buffers.Uniforms;

namespace Engine.Components.Receivers
{
    public interface IModelTransformation
    {
        Matrix4x4 ModelTransformation { set; }
    }
}
