using Engine.Assets;
using Engine.ECS;
using Veldrid;
using Veldrid.SPIRV;

namespace Engine.Components
{
    public class Material : Component, IResource, IAsset
    {
        private ResourceFactory _factory;

        public Material(
            string name,
            string shaderFilename,
            BlendStateDescription blendState,
            DepthStencilStateDescription depthStencilState,
            PolygonFillMode fillMode = PolygonFillMode.Solid,
            bool depthClipEnabled = true)
            : base(name)
        {
            ShaderFilename = shaderFilename;
            DepthStencilState = depthStencilState;
            FillMode = fillMode;
            DepthClipEnabled = depthClipEnabled;
            BlendState = blendState;
        }

        public static readonly DepthStencilStateDescription DefaultDepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true, depthWriteEnabled: true, comparisonKind: ComparisonKind.LessEqual);

        public string ShaderFilename { get; }
        public Shader[] Shaders { get; private set; }
        public DepthStencilStateDescription DepthStencilState { get; }
        public PolygonFillMode FillMode { get; }
        public bool DepthClipEnabled { get; }
        public BlendStateDescription BlendState { get; }

        public void Initialize(ResourceFactory factory, GraphicsDevice device)
        {
            _factory = factory;
        }

        public void LoadAssets(AssetDataLoader assetDataLoader)
        {
            var shaderSource = ShaderImporter.Instance.Import(assetDataLoader.Load(AssetType.Shader, ShaderFilename));
            // Compile shaders
            Shaders = _factory.CreateFromSpirv(
                new ShaderDescription(ShaderStages.Vertex, shaderSource, "VS"),
                new ShaderDescription(ShaderStages.Fragment, shaderSource, "FS"));
        }

        public void Dispose()
        {
            foreach (var shader in Shaders)
            {
                shader.Dispose();
            }
        }
    }
}
