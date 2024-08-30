using Angel;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Environment = Godot.Environment;

namespace TPC
{
    

    // NodeTree
    public partial class ThirdPersonCamera : Node3D
    {
        private Camera3D camera;
        private Node3D rotationPivot;
        private Node3D offsetPivot;
        private SpringArm3D springArm;
        private Marker3D cameraMarker;
        private Node3D cameraShaker;

        public Camera3D Camera
        {
            get
            {
                return camera;
            }

            set
            {
                $"Property set: {value.Name}".ToConsole();
                camera = value;
            }
        }
        public Node3D RotationPivot
        {
            get
            {
                return rotationPivot;
            }

            set
            {
                $"Property set: {value.Name}".ToConsole();
                rotationPivot = value;
            }
        }
        public Node3D OffsetPivot
        {
            get
            {
                return offsetPivot;
            }

            set
            {
                $"Property set: {value.Name}".ToConsole();
                offsetPivot = value;
            }
        }
        public SpringArm3D SpringArm
        {
            get
            {
                return springArm;
            }

            set
            {
                $"Property set: {value.Name}".ToConsole();
                springArm = value;
            }
        }
        public Marker3D CameraMarker
        {
            get
            {
                return cameraMarker;
            }

            set
            {
                $"Property set: {value.Name}".ToConsole();
                cameraMarker = value;
            }
        }
        public Node3D CameraShaker
        {
            get
            {
                return cameraShaker;
            }

            set
            {
                $"Property set: {value.Name}".ToConsole();
                cameraShaker = value;
            }
        }
    }

    public partial class ThirdPersonCamera : Node3D
    {
        public override void _Ready()
        {
            CallDeferred(MethodName.LoadNodes);

        }

        public override void _PhysicsProcess(double delta)
        {
            UpdateCameraProperties();
            if (Engine.IsEditorHint())
            {
                UpdateEditor();
            }
            TweenCameraToMarker();
            ProcessCameraOffset();
            ProcessRotationPivot();
            ProcessTiltInput();
            ProcessHorizontalRotationInput();
            UpdateCameraTilt();
            UpdateCameraHorizontalRotation();
        }

        private void LoadNodes()
        {
            Camera = GetNode<Camera3D>("Camera");
            RotationPivot = GetNode<Node3D>("RotationPivot");
            OffsetPivot = GetNode<Node3D>("RotationPivot/OffsetPivot");
            SpringArm = GetNode<SpringArm3D>("RotationPivot/OffsetPivot/CameraSpringArm");
            CameraMarker = GetNode<Marker3D>("RotationPivot/OffsetPivot/CameraSpringArm/CameraMarker");
            CameraShaker = GetNode<Node3D>("CameraShaker");
            Camera.TopLevel = true;
        }

        /// <summary>
        /// Waits until a node is ready, and sets a value inside a Node's property. Executes asyncronously.
        /// </summary>
        /// <param name="n">Node to set a property in.</param>
        /// <param name="property">Property to change.</param>
        /// <param name="value">Value to set.</param>
        async void SetWhenReady(Node n, StringName property, Variant value)
        {
            if (IsNodeReady())
            {
                if (!n.IsNodeReady() && n != null)
                {
                    "Node is not ready. Awaiting...".ToConsole();
                    await ToSignal(this, SignalName.Ready);
                    "Node is ready.".ToConsole();
                    n.Set(property, value);
                    $"\t{property} = {n.Get(property)} set".ToConsole();
                }
                else
                {
                    $"Node is ready.".ToConsole();
                    n.Set(property, value);
                    $"\t{property} = {value} set".ToConsole();
                }
            }
        }

    }

    

    // Fields
    // Camera Position and Rotation
    public partial class ThirdPersonCamera : Node3D
    {
        [Export]
        public float DistanceFromPivot { get; set; } = 10.0f;

        [Export]
        public Vector2 PivotOffset { get; set; } = Vector2.Zero;

        [Export(PropertyHint.Range, "-90.0,90.0")]
        private float initialDiveAngleDeg = -45.0f;
        public float InitialDiveAngleDeg
        {
            get
            {
                return initialDiveAngleDeg;
            }
            set
            {
                initialDiveAngleDeg = Mathf.Clamp(value, TiltLowerLimitDeg, TiltUpperLimitDeg);
            }
        }

        [Export(PropertyHint.Range, "-90.0, 90.0")]
        public float TiltUpperLimitDeg { get; set; } = 60.0f;

        [Export(PropertyHint.Range, "-90.0, 90.0")]
        public float TiltLowerLimitDeg { get; set; } = -60.0f;

        [Export(PropertyHint.Range, "1.0, 1000.0")]
        public float TiltSensitiveness { get; set; } = 10.0f;

        [Export(PropertyHint.Range, "1.0, 1000.0")]
        public float HorizontalRotationSensitiveness { get; set; } = 10.0f;

        [Export(PropertyHint.Range, "0.1, 1.0")]
        public float CameraSpeed { get; set; } = 0.1f;

        public float CameraTiltDeg { get; set; } = 0.0f;
        public float CameraHorizontalRotationDeg { get; set; } = 0.0f;
    }

    // Mouse Properties
    public partial class ThirdPersonCamera : Node3D
    {
        [ExportGroup("Mouse")]
        [Export]
        public bool MouseFollow { get; set; } = false;

        [Export(PropertyHint.Range, "0.0, 100.0")]
        public float MouseXSensitiveness { get; set; } = 1.0f;

        [Export(PropertyHint.Range, "0.0, 100.0")]
        public float MouseYSensitiveness { get; set; } = 1.0f;
    }

    // SpringArm3D Properties
    public partial class ThirdPersonCamera : Node3D
    {
        [ExportCategory("SpringArm3D")]
        [Export(PropertyHint.Layers3DRender)]
        private int _springArmCollisionMask = 1;
        public int SpringArmCollisionMask
        {
            get => _springArmCollisionMask;
            set
            {
                _springArmCollisionMask = value;
                SetWhenReady(SpringArm, SpringArm3D.PropertyName.CollisionMask, value);
            }
        }

        [Export(PropertyHint.Range, "0.0, 100.0, 0.01, or_greater, or_less, hide_slider, suffix:m")]
        private float _springArmMargin = 0.01f;
        public float SpringArmMargin
        {
            get => _springArmMargin;
            set
            {
                _springArmMargin = value;
                SetWhenReady(springArm, SpringArm3D.PropertyName.Margin, value);
            }
        }
    }

    // Camera3D properties
    public partial class ThirdPersonCamera : Node3D
    {
        private bool _current;
        [Export]
        public bool Current
        {
            get => _current;
            set
            {
                _current = value;
                SetWhenReady(camera, Camera3D.PropertyName.Current, value);
            }
        }

        [ExportCategory("Camera3D")]
        [Export]
        public Camera3D.KeepAspectEnum KeepAspect { get; set; } = Camera3D.KeepAspectEnum.Height;


        [Export(PropertyHint.Layers3DRender)]
        public int CullMask { get; set; } = 1048575;

        [Export]
        public Environment Environment { get; set; }

        [Export]
        public CameraAttributes Attributes { get; set; }

        [Export]
        public Camera3D.DopplerTrackingEnum DopplerTracking { get; set; } = Camera3D.DopplerTrackingEnum.Disabled;

        [Export]
        public Camera3D.ProjectionType Projection { get; set; } = Camera3D.ProjectionType.Perspective;

        [Export(PropertyHint.Range, "1.0, 179.0, 0.1, suffix:°")]
        public float FOV { get; set; } = 75.0f;

        [Export]
        public float Near { get; set; } = 0.05f;

        [Export]
        public float Far { get; set; } = 4000.0f;
    }

    // Physics process Methods
    public partial class ThirdPersonCamera : Node3D
    {
        private void UpdateCameraProperties()
        {
            camera.KeepAspect = KeepAspect;
            camera.CullMask = (uint)CullMask;
            camera.DopplerTracking = DopplerTracking;
            camera.Projection = Projection;
            camera.Near = Near;
            camera.Far = Far;
            camera.Fov = FOV;

            if (camera.Environment != Environment) camera.Environment = Environment;
            if (camera.Attributes != Attributes) camera.Attributes = Attributes;
        }

        private void UpdateEditor()
        {
            if (camera == null) LoadNodes();
            // init new vector with Z=1.0
            Vector3 initialPosition = new(0.0f, 0.0f, 1.0f);

            // rotate init around X by the initial dive angle.
            // rotate result around Y axis by -CameraHorizontalRotationDeg
            Vector3 rotatedPosition = initialPosition
                                        .Rotated(Vector3.Right, Mathf.DegToRad(initialDiveAngleDeg)) 
                                        .Rotated(Vector3.Up, Mathf.DegToRad(-CameraHorizontalRotationDeg));

            // multiply vector by length of SpringArm and adding it to its GlobalPosition
            Vector3 finalPosition = rotatedPosition * SpringArm.SpringLength + SpringArm.GlobalTransform.Origin;

            // GlobalTransform of the marker => new position.
            cameraMarker.GlobalTransform = new Transform3D(cameraMarker.GlobalTransform.Basis, finalPosition);
        }

        private void TweenCameraToMarker() => camera.GlobalTransform = new Transform3D(camera.GlobalBasis, camera.GlobalTransform.Origin.Lerp(cameraMarker.GlobalTransform.Origin, CameraSpeed));

        private void ProcessCameraOffset() => offsetPivot.GlobalPosition = offsetPivot.GetParent<Node3D>().ToGlobal(new Vector3(PivotOffset.X, PivotOffset.Y, 0.0f));

        private void ProcessRotationPivot()
        {
            Vector3 rotationPivotGlobalRotationDeg = rotationPivot.GlobalRotationDegrees;
            rotationPivotGlobalRotationDeg.X = initialDiveAngleDeg;
            rotationPivot.GlobalPosition= GlobalPosition;
        }

        private void ProcessTiltInput()
        {
            if (InputMap.HasAction("tp_camera_up") && InputMap.HasAction("tp_camera_down"))
            {
                float tiltVariation = Input.GetActionStrength("tp_camera_up") - Input.GetActionStrength("tp_camera_down");
                tiltVariation = tiltVariation * (float)GetProcessDeltaTime() * 5 * TiltSensitiveness;
                CameraTiltDeg = Math.Clamp(CameraTiltDeg + tiltVariation, TiltLowerLimitDeg - InitialDiveAngleDeg, TiltUpperLimitDeg - InitialDiveAngleDeg);
            }
        }

        private void ProcessHorizontalRotationInput()
        {
            if (InputMap.HasAction("tp_camera_right") && InputMap.HasAction("tp_camera_left"))
            {
                float cameraHorizontalVarition = Input.GetActionStrength("tp_camera_right") - Input.GetActionStrength("tp_camera_left");
                cameraHorizontalVarition = cameraHorizontalVarition * (float)GetProcessDeltaTime() * 30 * HorizontalRotationSensitiveness;
                CameraHorizontalRotationDeg += cameraHorizontalVarition;
            }
        }

        private void UpdateCameraTilt()
        {
            // Clamps current value between the limits.
            float finalTiltValue = Mathf.Clamp(InitialDiveAngleDeg + CameraTiltDeg, TiltLowerLimitDeg, TiltUpperLimitDeg);

            // Interpolates the X angle to the final calculated value in 0.1 seconds.
            Tween tween = CreateTween();
            tween.TweenProperty(camera, "global_rotation_degrees:x", finalTiltValue, 0.1);
        }

        private void UpdateCameraHorizontalRotation()
        {
            var tween = CreateTween();
            tween.TweenProperty(RotationPivot, "global_rotation_degrees:y", CameraHorizontalRotationDeg * -1, 0.1).AsRelative();

            CameraHorizontalRotationDeg = 0.0f; // reset value

            // check this part here.
            // Calculates a normalized vector between the camera and the offset pivot X and Z.
            Vector2 vectToOffsetPivot =
                // offsetPivotX,Y
                (new Vector2(OffsetPivot.GlobalPosition.X, OffsetPivot.GlobalPosition.Z)
                // cameraX,Y
                - new Vector2(camera.GlobalPosition.X, camera.GlobalPosition.Z))
                .Normalized();

            // assign to Y.
            var cameraRotation = camera.GlobalRotation;
            cameraRotation.Y = -new Vector2(0.0f, -1.0f).AngleTo(vectToOffsetPivot);
        }

        

    }

    // Motion
    public partial class ThirdPersonCamera : Node3D
    {
        public override void _UnhandledInput(InputEvent @event)
        {
            if (MouseFollow && @event is InputEventMouseMotion)
            {
                var mouseEvent = (InputEventMouseMotion)@event;
                CameraHorizontalRotationDeg += mouseEvent.Relative.X * 0.1f * MouseXSensitiveness;
                CameraTiltDeg -= mouseEvent.Relative.Y * 0.07f * MouseYSensitiveness;
            }
        }
        
        public Vector3 GetFrontDirection()
        {
            var direction = offsetPivot.GlobalPosition - camera.GlobalPosition;
            direction.Y = 0.0f;
            direction = direction.Normalized();
            return direction;
        }

        public Vector3 GetBackDirection() => -GetFrontDirection();
        public Vector3 GetLeftDirection() => GetFrontDirection().Rotated(Vector3.Up, (float)Math.PI / 2);
        public Vector3 GetRightDirection() => GetFrontDirection().Rotated(Vector3.Up, -(float)Math.PI / 2);
    }
}
