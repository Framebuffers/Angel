using Godot;

namespace Angel
{
    public partial class AngelCamera
    {
        // --------------------------------------------------------------------------------
        // Camera
        private double fov = 75.0;
        private bool current = false;
        private Camera3D.ProjectionType projectionType = Camera3D.ProjectionType.Perspective;
        private double near = 0.05;
        private double far = 4000.0;
        private Camera3D.KeepAspectEnum keepAspect = Camera3D.KeepAspectEnum.Height;
        private Camera3D.DopplerTrackingEnum dopplerTracking = Camera3D.DopplerTrackingEnum.Disabled;
        private uint cullMask = 1048575;
        private float initialDiveAngle = -45.0f;

        /// <summary>
        /// Mapped property of <see cref="Camera3D.Projection"/>
        /// </summary>
        [Export]
        public Camera3D.ProjectionType ProjectionType
        {
            get
            {
                return projectionType;
            }
            set
            {
                projectionType = value;
                camera.Projection = value;
            }
        }


        /// <summary>
        /// Mapped property from <see cref="Camera3D.Fov"/>
        /// </summary>
        [Export(PropertyHint.Range, "1.0, 179.0, 0.1, suffix:d")]
        public double Fov
        {
            get
            {
                return fov;
            }
            set
            {
                fov = value;
                SetWhenReady(camera, Camera3D.PropertyName.Fov, value);
            }
        }

        /// <summary>
        /// Mapped property from <see cref="Camera3D.Near"/>
        /// </summary>
        [Export]
        public double Near
        {
            get
            {
                return near;
            }
            set
            {
                near = value;
                SetWhenReady(camera, Camera3D.PropertyName.Near, value);
            }
        }

        /// <summary>
        /// Mapped property from <see cref="Camera3D.Far"/>
        /// </summary>
        [Export]
        public double Far
        {
            get
            {
                return far;
            }
            set
            {
                far = value;
                SetWhenReady(camera, Camera3D.PropertyName.Far, value);
            }
        }

        /// <summary>
        /// Mapped property from <see cref="Camera3D.KeepAspect"/>
        /// </summary>
        [ExportCategory("Camera3D")]
        [Export]
        public Camera3D.KeepAspectEnum KeepAspect
        {
            get
            {
                return keepAspect;
            }

            set
            {
                keepAspect = value;
                camera.KeepAspect = value;
            }
        }

        /// <summary>
        /// Mapped property from <see cref="Camera3D.DopplerTracking"/>
        /// </summary>
        [Export]
        public Camera3D.DopplerTrackingEnum DopplerTracking
        {
            get
            {
                return dopplerTracking;
            }

            set
            {
                dopplerTracking = value;
                camera.DopplerTracking = value;
            }
        }

        /// <summary>
        /// Mapped property of <see cref="Camera3D.CullMask"/>
        /// </summary>
        [Export(PropertyHint.Layers3DRender)]
        public uint CullMask
        {
            get
            {
                return cullMask;
            }

            set
            {
                cullMask = value;
                SetWhenReady(camera, Camera3D.PropertyName.CullMask, value);
            }
        }
    }
}
