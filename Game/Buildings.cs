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

            GenerateBuildings(8, 3, 8, 4, 2);
        }

        private void GenerateBuildings(int amount, int rowAmount, float footprintSize, float height, float spacing)
        {
            if (rowAmount > amount)
            {
                throw new ArgumentException($"{nameof(rowAmount)} must be less than {nameof(amount)}");
            }

            var mesh = new CubeMesh()
            {
                Size = new Vector3(footprintSize, height, footprintSize)
            };

            for (var i = 0; i < amount; i++)
            {
                var column = i / rowAmount;
                var row = i % rowAmount;
                var xPos = column * footprintSize + (spacing * column);
                var yPos = row * footprintSize + (spacing * row);
                var instance = new MeshInstance()
                {
                    Mesh = mesh,
                    Translation = new Vector3(xPos, 0, yPos),
                };
                AddChild(instance, true);
            }

            Material = _material;

            var boundsCenter = Bounds.GetCenter();
            Translate(new Vector3(boundsCenter.x, 0, boundsCenter.z));
            RotateY(Mathf.Pi / 4 + Mathf.Pi / 8);
        }
    }
}
