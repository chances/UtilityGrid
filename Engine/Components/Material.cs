using Engine.ECS;
using Veldrid;

namespace Engine.Components
{
    public class Material : Component, IResource
    {
        private readonly byte[] _shaderSource;
        private Shader _vertexShader;
        private Shader _fragmentShader;

        public Material(
            string name,
            byte[] shaderSource,
            string shaderFilename,
            BlendStateDescription blendState,
            DepthStencilStateDescription depthStencilState,
            PolygonFillMode fillMode = PolygonFillMode.Solid,
            bool depthClipEnabled = true)
            : base(name)
        {
            _shaderSource = shaderSource;
            ShaderFilename = shaderFilename;
            DepthStencilState = depthStencilState;
            FillMode = fillMode;
            DepthClipEnabled = depthClipEnabled;
            BlendState = blendState;
        }

        public static readonly DepthStencilStateDescription DefaultDepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true, depthWriteEnabled: true, comparisonKind: ComparisonKind.LessEqual);

        public string ShaderFilename { get; }
        public Shader[] Shaders => new Shader[] {_vertexShader, _fragmentShader};
        public DepthStencilStateDescription DepthStencilState { get; }
        public PolygonFillMode FillMode { get; }
        public bool DepthClipEnabled { get; }
        public BlendStateDescription BlendState { get; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            // Compile shaders
            _vertexShader = CompileShader(factory, ShaderStages.Vertex);
            _fragmentShader = CompileShader(factory, ShaderStages.Fragment);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private Shader CompileShader(ResourceFactory factory, ShaderStages stage)
        {
            var entryPoint = stage == ShaderStages.Vertex ? "VS" : "FS";
            return factory.CreateShader(new ShaderDescription(stage, _shaderSource, entryPoint));
        }
    }
}
