//using Godot;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Angel
//{
//    public partial class AngelCamera
//    {
//        // --------------------------------------------------------------------------------
//        // Rotation and position
//        /// <summary>
//        /// Offset coordinates from the pivot marker.
//        /// </summary>
//        [Export]
//        public Vector2 PivotOffset { get; set; } = Vector2.Zero;

//        [Export(PropertyHint.Range, "-90.0,90.0")]
//        public float TiltLowerLimit { get; set; } = -60.0f;

//        [Export(PropertyHint.Range, "-90.0,90.0")]
//        public float TiltUpperLimit { get; set; } = 60.0f;

//        [Export(PropertyHint.Range, "1.0,1000.0")]
//        public float HorizontalRotationSensitivity { get; set; } = 10.0f;
//        [Export(PropertyHint.Range, "1.0,1000.0")]
//        public float TiltSensitivity { get; set; } = 10.0f;

//        [Export(PropertyHint.Range, "-90.0,90.0")]
//        public float HorizontalRotationAngle { get; set; } = -45.0f;
//        [Export(PropertyHint.Range, "-90.0,90.0")]
//        public float CameraTiltAngle { get; set; } = 0.0f;

//        /// <summary>
//        /// Speed at which <see cref="activeCamera"/> will traslate between points. This will speed up or slow down any rotation and 
//        /// </summary>
//        [Export(PropertyHint.Range, "0.1,1")]
//        public float CameraSpeed { get; set; } = 0.1f;

//        /// <summary>
//        /// RotationPivot X axis. Defines the angle against the Player on which the Camera will start pointing at.
//        /// </summary>
//        [Export(PropertyHint.Range, "-90.0,90.0")]
//        public float InitialDiveAngle
//        {
//            get
//            {
//                return initialDiveAngle;
//            }

//            set
//            {
//                float v = Mathf.Clamp(value, TiltLowerLimit, TiltUpperLimit);
//                initialDiveAngle = v;
//            }
//        }
//    }
//}
