using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using UtilityGrid.Engine.Common;

namespace UtilityGrid.Game
{
    public class Buildings : Spatial, IMeshObject
    {
        private ShaderMaterial _material;
        private static readonly Color Green = new Color(0.2f, 0.9f, 0.3f);
        private static readonly Color Purple = new Color(0.5f, 0.3f, 0.75f);

        public Spatial Spatial => this;

        public AABB Bounds => this.CalculateBounds();

        public IEnumerable<MeshInstance> Meshes => this.GetChildren<MeshInstance>();

        public Material Material
        {
            get
            {
                if (!Meshes.Any())
                {
                    return null;
                }

                var materialPtrs = Meshes.Select(m => m.GetMaterial().NativeInstance).ToList();
                return materialPtrs.SequenceEqual(materialPtrs)
                    ? Meshes.First().GetMaterial()
                    : null;
            }
            set => this.SetMaterial(value ?? throw new ArgumentNullException(nameof(value)));
        }

        public override void _Ready()
        {
            base._Ready();

            _material = new ShaderMaterial()
            {
                Shader = (Shader) GD.Load("res://Data/Shaders/Default.shader")
            };
            _material.SetShaderParam("color", Green);

            var height = 0.375f;
            // House Dimensions
            // 2.5m height (~8 feet)
            // Minimum room footprint: 6.5 meter²
            // Four rooms, 6.5 meter² * 4 => 26 meter², ≈ ~5.1m × ~5.1m
            var mesh = GenerateHouse(6, 5, 2.5f, 1);

            GenerateBuildings(mesh, 15, 5, 10, height, 6);

            GetParentSpatial().GetNode<Spatial>("Building").Translate(new Vector3(0, height, 0));
        }

        private Mesh GenerateHouse(float length, float width, float height, float gableHeight)
        {
            var houseHeight = height + gableHeight;
            var halfWidth = width / 2.0f;
            var builder = new SurfaceTool();
            builder.Begin(Mesh.PrimitiveType.Triangles);

            // Walls

            builder.AddVertex(new Vector3(0, 0, 0));
            builder.AddVertex(new Vector3(length, 0, 0));
            builder.AddVertex(new Vector3(length, height, 0));
            builder.AddVertex(new Vector3(0, 0, 0));
            builder.AddVertex(new Vector3(length, height, 0));
            builder.AddVertex(new Vector3(0, height, 0));

            builder.AddVertex(new Vector3(0, 0, 0));
            builder.AddVertex(new Vector3(0, height, width));
            builder.AddVertex(new Vector3(0, 0, width));
            builder.AddVertex(new Vector3(0, 0, 0));
            builder.AddVertex(new Vector3(0, height, 0));
            builder.AddVertex(new Vector3(0, height, width));

            builder.AddVertex(new Vector3(0, 0, width));
            builder.AddVertex(new Vector3(length, height, width));
            builder.AddVertex(new Vector3(length, 0, width));
            builder.AddVertex(new Vector3(0, 0, width));
            builder.AddVertex(new Vector3(0, height, width));
            builder.AddVertex(new Vector3(length, height, width));

            builder.AddVertex(new Vector3(length, 0, 0));
            builder.AddVertex(new Vector3(length, 0, width));
            builder.AddVertex(new Vector3(length, height, width));
            builder.AddVertex(new Vector3(length, 0, 0));
            builder.AddVertex(new Vector3(length, height, width));
            builder.AddVertex(new Vector3(length, height, 0));

            // Gables

            builder.AddVertex(new Vector3(0, height, 0));
            builder.AddVertex(new Vector3(0, houseHeight, halfWidth));
            builder.AddVertex(new Vector3(0, height, width));

            builder.AddVertex(new Vector3(length, height, 0));
            builder.AddVertex(new Vector3(length, height, width));
            builder.AddVertex(new Vector3(length, houseHeight, halfWidth));

            // Roof

            builder.AddVertex(new Vector3(0, height, 0));
            builder.AddVertex(new Vector3(length, height, 0));
            builder.AddVertex(new Vector3(length, houseHeight, halfWidth));
            builder.AddVertex(new Vector3(0, height, 0));
            builder.AddVertex(new Vector3(length, houseHeight, halfWidth));
            builder.AddVertex(new Vector3(0, houseHeight, halfWidth));

            builder.AddVertex(new Vector3(0, height, width));
            builder.AddVertex(new Vector3(length, houseHeight, halfWidth));
            builder.AddVertex(new Vector3(length, height, width));
            builder.AddVertex(new Vector3(0, height, width));
            builder.AddVertex(new Vector3(0, houseHeight, halfWidth));
            builder.AddVertex(new Vector3(length, houseHeight, halfWidth));

            builder.GenerateNormals();
            return builder.Commit();
        }

        private void GenerateBuildings(Mesh mesh, int amount, int rowAmount, float footprintSize, float height, float spacing)
        {
            if (rowAmount > amount)
            {
                throw new ArgumentException($"{nameof(rowAmount)} must be less than {nameof(amount)}");
            }

            for (var i = 0; i < amount; i++)
            {
                var column = i / rowAmount;
                var row = i % rowAmount;
                var xPos = column * footprintSize + (spacing * column);
                var yPos = row * footprintSize + (spacing * row);
                var instance = new MeshInstance()
                {
                    Mesh = mesh,
                    Translation = new Vector3(xPos, height / 2, yPos),
                };
                AddChild(instance, true);
            }

            Material = _material;

            var boundsCenter = Bounds.GetCenter();
            Translate(new Vector3(boundsCenter.x, 0, boundsCenter.z));
            RotateY(Mathf.Pi / 4);
        }
    }
}
