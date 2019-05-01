using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Components;
using Engine.ECS;
using Veldrid;

namespace Engine.Systems
{
    public class Renderer : ECS.System, IDisposable
    {
        private readonly ResourceFactory _factory;
        private readonly Framebuffer _framebuffer;
        private readonly Action<CommandList> _submitCommands;
        private CommandList _commands;
        private Dictionary<Material, Pipeline> _pipelines = new Dictionary<Material, Pipeline>();

        public Renderer(World world, ResourceFactory factory, Framebuffer framebuffer, Action<CommandList> submitCommands) : base(world)
        {
            _factory = factory;
            _framebuffer = framebuffer;
            _submitCommands = submitCommands;
            _commands = factory.CreateCommandList();
        }

        public override void Operate()
        {
            _commands.Begin();
            _commands.SetFramebuffer(_framebuffer);
            _commands.ClearColorTarget(0, RgbaFloat.Black);
            _commands.ClearDepthStencil(1f);

            // TODO: Implement a keyboard provider system with IKeyboard-ish component
            // _commands.ClearColorTarget(0,
            //     MouseState.IsButtonDown(MouseButton.Left) ? RgbaFloat.Cyan : RgbaFloat.CornflowerBlue);

            var renderables = World.Where(CanOperateOn)
                .GroupBy(entity =>
                {
                    var mesh = entity.GetComponent<MeshData>();
                    return (
                        entity.GetComponent<Material>(),
                        mesh.FrontFace,
                        mesh.PrimitiveTopology,
                        entity.GetComponent<IResourceSet>().ResourceLayout,
                        mesh.VertexBuffer.LayoutDescription
                    );
                });
            foreach (var renderable in renderables)
            {
                var material = renderable.Key.Item1;

                if (!_pipelines.ContainsKey(material))
                {
                    var frontFace = renderable.Key.FrontFace;
                    var primitiveTopology = renderable.Key.PrimitiveTopology;
                    var resourceLayout = renderable.Key.ResourceLayout;
                    var vertexLayout = renderable.Key.LayoutDescription;
                    var pipeline = CreatePipeline(material,
                        frontFace, primitiveTopology,
                        resourceLayout, vertexLayout);

                    _pipelines.Add(material, pipeline);
                }

                _commands.SetPipeline(_pipelines[material]);

                var meshesWithUniforms = renderable.Select(entity => (
                    entity.GetComponent<MeshData>().VertexBuffer,
                    entity.GetComponent<IResourceSet>().ResourceSet
                ));
                foreach (var meshAndUniforms in meshesWithUniforms)
                {
                    var mesh = meshAndUniforms.VertexBuffer;

                    _commands.SetVertexBuffer(0, mesh.Vertices);
                    _commands.SetIndexBuffer(mesh.Indices.DeviceBuffer, IndexFormat.UInt16);
                    _commands.SetGraphicsResourceSet(0, meshAndUniforms.ResourceSet);

                    _commands.DrawIndexed(
                        indexCount: (uint) mesh.Indices.Count,
                        instanceCount: 1, // TODO: Group renderables by MeshData and figure out instance uniforms
                        indexStart: 0,
                        vertexOffset: 0,
                        instanceStart: 0
                    );
                }
            }

            _commands.End();
            _submitCommands(_commands);
        }

        public void Dispose()
        {
            _commands.Dispose();

            foreach (var pipeline in _pipelines.Keys)
            {
                pipeline.Dispose();
            }
        }

        /// <remarks>
        /// A renderable entity requires these components:
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="Material"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="MeshData"/></description>
        /// </item>
        /// <item>
        /// <description>and <see cref="IResourceSet"/></description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="entity"></param>
        /// <returns>Whether this <see cref="Renderer"/> can operate on the given <see cref="Entity"/></returns>
        private static bool CanOperateOn(Entity entity) => entity.HasComponentsOfTypes
        (
            typeof(Material),
            typeof(MeshData),
            typeof(IResourceSet)
        ) && entity.HasTag(Tags.Initialized);

        private Pipeline CreatePipeline(
            Material material, FrontFace frontFace, PrimitiveTopology primitiveTopology,
            ResourceLayout resourceLayout, VertexLayoutDescription vertexLayout)
        {
            var pipelineDesc = new GraphicsPipelineDescription
            {
                BlendState = material.BlendState,
                DepthStencilState = material.DepthStencilState,
                RasterizerState = new RasterizerStateDescription(
                    cullMode: material.CullMode,
                    fillMode: material.FillMode,
                    frontFace: frontFace,
                    depthClipEnabled: true,
                    scissorTestEnabled: false
                ),
                PrimitiveTopology = primitiveTopology,
                ResourceLayouts = new ResourceLayout[]
                {
                    resourceLayout
                },
                ShaderSet = new ShaderSetDescription(
                    new VertexLayoutDescription[] { vertexLayout },
                    material.Shaders
                ),
                Outputs = _framebuffer.OutputDescription
            };

            return _factory.CreateGraphicsPipeline(pipelineDesc);
        }
    }
}
