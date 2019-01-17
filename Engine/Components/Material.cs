using Engine.Assets;
using Engine.ECS;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;
using Veldrid.SPIRV;

namespace Engine.Components
{
    public class Material : Component, IResource, IAsset
    {
        private ResourceFactory _factory;

        public Material([NotNull] string name, string shaderFilename) : base(name)
        {
            Guard.AgainstNullArgument(nameof(name), name);
            Guard.AgainstNullArgument(nameof(shaderFilename), shaderFilename);
            ShaderFilename = shaderFilename;
            DepthStencilState = DefaultDepthStencilState;
            FillMode = PolygonFillMode.Solid;
            DepthClipEnabled = true;
            BlendState = DefaultBlendState;
        }

        public static readonly DepthStencilStateDescription DefaultDepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true, depthWriteEnabled: true, comparisonKind: ComparisonKind.LessEqual);
        public static readonly BlendStateDescription DefaultBlendState = BlendStateDescription.SingleOverrideBlend;

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
