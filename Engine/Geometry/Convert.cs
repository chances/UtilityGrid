namespace Engine.Geometry
{
    public static class Convert
    {
        private const float OneDegreeInRadians = 0.01745329252f;

        public static float RadToDeg(this float radians) => radians / OneDegreeInRadians;

        public static float DegToRad(this float degrees) => OneDegreeInRadians * degrees;
    }
}
