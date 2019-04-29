using System.IO;
using System.Linq;
using Engine.Assets;
using Engine.ECS;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;
using Veldrid.SPIRV;

namespace Engine.Components
{
    public class Material : ResourceComponent, IAsset, IDependencies
    {
        private byte[] _vertexShaderSource, _fragmentShaderSource;

        public Material([NotNull] string name, string shaderFilename) : base(name)
        {
            Guard.AgainstNullArgument(nameof(name), name);
            Guard.AgainstNullArgument(nameof(shaderFilename), shaderFilename);
            ShaderFilename = shaderFilename;
            DepthStencilState = DefaultDepthStencilState;
            CullMode = FaceCullMode.Back;
            FillMode = PolygonFillMode.Solid;
            DepthClipEnabled = true;
            BlendState = DefaultBlendState;

            Resources.OnInitialize = (factory, _) => {
                // Compile shaders
                try {
                    Shaders = factory.CreateFromSpirv(
                        new ShaderDescription(ShaderStages.Vertex, _vertexShaderSource, "VS"),
                        new ShaderDescription(ShaderStages.Fragment, _fragmentShaderSource, "FS"));
                } finally {
                    _vertexShaderSource = null;
                    _fragmentShaderSource = null;
                }
            };
            Resources.OnDispose = () => {
                foreach (var shader in Shaders)
                {
                    shader.Dispose();
                }
            };
        }

        public static readonly DepthStencilStateDescription DefaultDepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true, depthWriteEnabled: true, comparisonKind: ComparisonKind.LessEqual);
        public static readonly BlendStateDescription DefaultBlendState = BlendStateDescription.SingleOverrideBlend;

        public string ShaderFilename { get; }
        public Shader[] Shaders { get; private set; }
        public DepthStencilStateDescription DepthStencilState { get; }
        public FaceCullMode CullMode {get; set; }
        public PolygonFillMode FillMode { get; set; }
        public bool DepthClipEnabled { get; }
        public BlendStateDescription BlendState { get; }

        public bool AreDependenciesSatisfied =>
            _vertexShaderSource != null && _fragmentShaderSource != null;

        public void LoadAssets(AssetDataLoader assetDataLoader)
        {
            var shaderFilenameWithoutExtension = ShaderFilename.Split('.').FirstOrDefault();
            var compiledShadersExist = new string[] {
                $"{shaderFilenameWithoutExtension}.vs.spirv",
                $"{shaderFilenameWithoutExtension}.fs.spirv"
            }.Aggregate(true, (shadersExist, filename) =>
                shadersExist && assetDataLoader.Exists(AssetType.Shader, filename)
            );

            if (compiledShadersExist) {
                // TODO: Wait for https://github.com/mellinoe/veldrid-spirv/pull/2 and remove this?
                _vertexShaderSource = ShaderImporter.Instance.Import(assetDataLoader.Load(
                    AssetType.Shader,
                    $"{shaderFilenameWithoutExtension}.vs.spirv"
                ));
                _fragmentShaderSource = ShaderImporter.Instance.Import(assetDataLoader.Load(
                    AssetType.Shader,
                    $"{shaderFilenameWithoutExtension}.fs.spirv"
                ));
            }
            else if (assetDataLoader.Exists(AssetType.Shader, ShaderFilename))
            {
                var shaderSource = ShaderImporter.Instance.Import(
                    assetDataLoader.Load(AssetType.Shader, ShaderFilename)
                );
                _vertexShaderSource = _fragmentShaderSource = shaderSource;
            }
            else
            {
                throw new FileNotFoundException($"Shader not found: {ShaderFilename}");
            }
        }
    }
}
