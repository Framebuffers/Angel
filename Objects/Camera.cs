using ThirdPersonCamera.Objects.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPersonCamera.Objects
{
    [GlobalClass]
    public partial class Camera : Node3D
    {
        private IObjectTree Objects;
        private Camera3D ActiveCamera { get => Objects.GetCamera(); }

        public override void _Ready()
        {
            Objects = new ObjectTree(GetParent() as Node3D);
        }

        public override void _Process(double delta)
        {
            Objects.TweenCameraToPlayer();
        }
    }
}
