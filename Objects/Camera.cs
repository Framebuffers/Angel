using ThirdPersonCamera.Objects.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Angel.Objects.Interfaces;
using Angel.Objects;

namespace ThirdPersonCamera.Objects
{
    [GlobalClass]
    public partial class Camera : Node3D
    {
        private IObjectTree Objects;
        private ICameraDistance Distance;

        // remove
        private Camera3D ActiveCamera { get => Objects.GetCamera(); }

        public override void _Ready()
        {
            Objects = new ObjectTree(GetParent() as Node3D);
            Distance = new CameraDistance(Objects);
        }

        public override void _Process(double delta)
        {
            Objects.TweenCameraToPlayer();
        }

        [Export(PropertyHint.Layers3DRender)]
        private uint SpringArmCollissionMask
        {
            get => Distance.GetSpringArmCollissionMask();
            set => Distance.SetSpringArmCollissionMask(value);
        }

        /// <summary>
        /// Property matching <see cref="SpringArm3D.Margin"/>.
        /// </summary>
        [Export(PropertyHint.Range, "0.0, 100.0, 0.01, or_greater, or_less, hide_slider, suffix:m")]
        private float SpringArmCollissionMargin
        {
            get => Distance.GetSpringArmMargin();
            set => Distance.SetSpringArmMargin(value);
        }

        /// <summary>
        /// SpringArm distance parameter.
        /// </summary>
        [Export(PropertyHint.)]
        private float SpringArmDistance
        {
            get => Distance.GetSpringArmDistance();
            set => Distance.SetSpringArmDistance(value);
        }

    }
}
