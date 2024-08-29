//using Angel.Objects.Interfaces;
//using Godot;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Angel.Objects
//{
//    /// <summary>
//    /// SpringArm3D that tweens the Camera and the Player. Creates an offset between <see cref="Node3D.GlobalPosition"/> and <see cref="SpringArm3D.SpringLength"/>.
//    /// </summary>
//    internal sealed partial class CameraDistance : SpringArm3D, ICameraDistance
//    {
//        public CameraDistance(IObjectTree tree)
//        {
//            _tree = tree;
//            CollisionMask = 1;
//            Margin = 0.01f;
//            SpringLength = 10.0f;
//        }

//        public SpringArm3D GetSpringArm() => this;
//        private readonly IObjectTree _tree;

//        /// <summary>
//        /// Collision Mask for ray between camera and Player.
//        /// </summary>
//        [Export(PropertyHint.Layers3DRender)]
//        public uint CollissionMask
//        {
//            get => Get(PropertyName.CollissionMask).AsUInt32();
//            set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.CollisionMask, value);
//        }

//        /// <summary>
//        /// Property matching <see cref="SpringArm3D.Margin"/>.
//        /// </summary>
//        [Export(PropertyHint.Range, "0.0, 100.0, 0.01, or_greater, or_less, hide_slider, suffix:m")]
//        public float CollissionMargin
//        {
//            get => (float)Get(PropertyName.CollissionMargin).AsDouble();
//            set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.Margin, value);
//        }

//        /// <summary>
//        /// SpringArm distance parameter.
//        /// </summary>
//        [Export]
//        public float Length
//        {
//            get => (float)Get(PropertyName.Length).AsDouble();
//            set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.SpringLength, value);
//        }

//        //[Export(PropertyHint.Layers3DRender)]
//        //private uint SpringArmCollissionMask
//        //{
//        //    get => GetSpringArmCollissionMask();
//        //    set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.CollisionMask, value);
//        //}

//        ///// <summary>
//        ///// Property matching <see cref="SpringArm3D.Margin"/>.
//        ///// </summary>
//        //[Export(PropertyHint.Range, "0.0, 100.0, 0.01, or_greater, or_less, hide_slider, suffix:m")]
//        //private float SpringArmCollissionMargin
//        //{
//        //    get => GetSpringArmMargin();
//        //    set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.Margin, value);
//        //}

//        ///// <summary>
//        ///// SpringArm distance parameter.
//        ///// </summary>
//        //[Export]
//        //private float SpringArmDistance
//        //{
//        //    get => GetSpringArmDistance();
//        //    set => _tree.SetWhenReady(this, SpringArm3D.PropertyName.SpringLength, value);
//        //}

//    }
//}
