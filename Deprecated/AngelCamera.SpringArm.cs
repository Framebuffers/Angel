//using Godot;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Angel
//{
//    // Properties for the SpringArm3D.
//    public partial class AngelCamera
//    {
//        private uint springArmCollissionMask = 1;
//        private float springArmCollissionMargin = 0.01f;
//        private float springArmLength = 10.0f;

//        [Export(PropertyHint.Layers3DRender)]
//        public uint SpringArmCollissionMask
//        {
//            get
//            {
//                return springArm.CollisionMask;
//            }
//            set
//            {
//                springArmCollissionMask = value;
//                SetWhenReady(springArm, SpringArm3D.PropertyName.CollisionMask, value);
//            }
//        }

//        /// <summary>
//        /// Property matching <see cref="SpringArm3D.Margin"/>.
//        /// </summary>
//        [Export(PropertyHint.Range, "0.0, 100.0, 0.01, or_greater, or_less, hide_slider, suffix:m")]
//        public float SpringArmCollissionMargin
//        {
//            get
//            {
//                return springArm.Margin;
//            }
//            set
//            {
//                springArmCollissionMargin = value;
//                SetWhenReady(springArm, SpringArm3D.PropertyName.Margin, value);
//            }
//        }

//        /// <summary>
//        /// SpringArm distance parameter.
//        /// </summary>
//        [Export]
//        float SpringArmDistance
//        {
//            get
//            {
//                return springArm.SpringLength;
//            }

//            set
//            {
//                springArmLength = value;
//                SetWhenReady(springArm, SpringArm3D.PropertyName.SpringLength, springArmLength);
//            }
//        }
//    }
//}
