using Godot;

namespace UtilityGrid.Engine.Common
{
    public static class AABBExtensions
    {
        public static Vector3 GetCenter(this AABB source)
        {
            var localCenter = new Vector3(source.Size.x / 2, source.Size.y / 2, source.Size.z / 2);
            return source.Position - localCenter;
        }
    }
}
