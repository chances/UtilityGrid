using Veldrid;

namespace Engine.Buffers
{
    public abstract class Buffer
    {
        protected DeviceBuffer _buffer;

        public string Name
        {
            get => _buffer.Name;
            set => _buffer.Name = value;
        }

        public DeviceBuffer DeviceBuffer => _buffer;
    }
}
