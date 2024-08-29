using Angel.Objects.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdPersonCamera.Objects.Interfaces;

namespace Angel.Objects
{
    /// <summary>
    /// SpringArm3D that tweens the Camera and the Player. Creates an offset between <see cref="Node3D.GlobalPosition"/> and <see cref="SpringArm3D.SpringLength"/>.
    /// </summary>
    internal sealed partial class CameraDistance : SpringArm3D, ICameraDistance
    {
        private readonly IObjectTree _tree;
        public SpringArm3D GetSpringArm() => this;

        public uint GetSpringArmCollissionMask() => CollisionMask;
        public void SetSpringArmCollissionMask(uint mask) => _tree.SetWhenReady(this, SpringArm3D.PropertyName.CollisionMask, mask);

        public float GetSpringArmDistance() => SpringLength;
        public void SetSpringArmDistance(float distance) => _tree.SetWhenReady(this, SpringArm3D.PropertyName.Margin, distance);
        
        public float GetSpringArmMargin() => Margin;
        public void SetSpringArmMargin(float margin) => _tree.SetWhenReady(this, SpringArm3D.PropertyName.Margin, margin);

        public CameraDistance(IObjectTree tree)
        {
            _tree = tree;
            CollisionMask = 1;
            Margin = 0.01f;
            SpringLength = 10.0f;
        }
        

        //[Export(PropertyHint.Layers3DRender)]
        //private uint SpringArmCollissionMask
        //{
        //    get => GetSpringArmCollissionMask();
        //    set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.CollisionMask, value);
        //}

        ///// <summary>
        ///// Property matching <see cref="SpringArm3D.Margin"/>.
        ///// </summary>
        //[Export(PropertyHint.Range, "0.0, 100.0, 0.01, or_greater, or_less, hide_slider, suffix:m")]
        //private float SpringArmCollissionMargin
        //{
        //    get => GetSpringArmMargin();
        //    set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.Margin, value);
        //}

        ///// <summary>
        ///// SpringArm distance parameter.
        ///// </summary>
        //[Export]
        //private float SpringArmDistance
        //{
        //    get => GetSpringArmDistance();
        //    set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.SpringLength, value);
        //}

    }
}
