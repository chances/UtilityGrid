using System.Numerics;
using Engine.ECS;
using JetBrains.Annotations;

namespace Engine.Components.Geometry
{
    public class Transformation : Component
    {
        public static Transformation Identity = new Transformation();
        private Matrix4x4 _value = Matrix4x4.Identity;

        public Transformation() : base(nameof(Transformation))
        {
        }

        public Matrix4x4 Value
        {
            get => _value;
            set => _value = value;
        }

        public Vector3 Translation
        {
            get => _value.Translation;
            set => _value.Translation = value;
        }

        public Quaternion Rotation
        {
            get => Quaternion.CreateFromRotationMatrix(Value);
            set
            {
                var translation = Translation;
                _value = Matrix4x4.Transform(Matrix4x4.Identity, value);
                _value.Translation = translation;
            }
        }

        public void Translate(float x = 0, float y = 0, float z = 0)
        {
            _value.Translation += new Vector3(x, y, z);
        }

        public void Translate(Vector3 translation)
        {
            _value.Translation += translation;
        }

        public void Rotate(Quaternion rotation)
        {
            _value = Matrix4x4.Transform(_value, rotation);
        }
    }
}
