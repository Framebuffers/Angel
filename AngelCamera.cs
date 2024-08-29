//using ThirdPersonCamera;
//using ThirdPersonCamera.Helpers;
//using ConsoleTables;
//using Godot;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;

//namespace Angel
//{
//    /// <summary>
//    /// Third Person Camera. Add as child to a node derived from CharacterBody3D.
//    /// 
//    /// How does it work?
//    /// There's two main objects: a RotationPivot and a Camera3D. 
//    /// The RotationPivot is "attached" to the Player it follows. The Camera3D renders to the Viewport.
//    /// Inside this RotationPivot, There's an OffsetPivot and a SpringArm3D.
//    /// The OffsetPivot lets the camera move away from the player.
//    /// Inside the SpringArm3D, there's a Marker3D for the Camera. 
//    /// The marker sets the target on which the Camera looks at.
//    /// The SpringArm is connected to the Camera.
//    /// </summary>
//    [GlobalClass]
//    public partial class AngelCamera : Node3D
//    {
//        private Camera3D camera;
//        private Vector3 ParentCoordinates;
//        private SpringArm3D springArm;
//        private Node3D rotationPivot;
//        private Node3D offsetPivot;
//        private Marker3D cameraMarker;
//        private MeshInstance3D gizmo;

//        // --------------------------------------------------------------------------------
//        // Camera
//        private double fov = 75.0;
//        private bool current = false;
//        private Camera3D.ProjectionType projectionType = Camera3D.ProjectionType.Perspective;
//        private double near = 0.05;
//        private double far = 4000.0;
//        private Camera3D.KeepAspectEnum keepAspect = Camera3D.KeepAspectEnum.Height;
//        private Camera3D.DopplerTrackingEnum dopplerTracking = Camera3D.DopplerTrackingEnum.Disabled;
//        private uint cullMask = 1048575;
//        private float initialDiveAngle = -45.0f;
//        private float springArmLength = 10.0f;

//        /// <summary>
//        /// Mapped property of <see cref="Camera3D.Projection"/>
//        /// </summary>
//        [Export]
//        public Camera3D.ProjectionType ProjectionType
//        {
//            get
//            {
//                return projectionType;
//            }
//            set
//            {
//                projectionType = value;
//                camera.Projection = value;
//            }
//        }


//        /// <summary>
//        /// Mapped property from <see cref="Camera3D.Fov"/>
//        /// </summary>
//        [Export(PropertyHint.Range, "1.0, 179.0, 0.1, suffix:d")]
//        public double Fov
//        {
//            get
//            {
//                return fov;
//            }
//            set
//            {
//                fov = value;
//                SetWhenReady(camera, Camera3D.PropertyName.Fov, value);
//            }
//        }

//        /// <summary>
//        /// Mapped property from <see cref="Camera3D.Near"/>
//        /// </summary>
//        [Export]
//        public double Near
//        {
//            get
//            {
//                return near;
//            }
//            set
//            {
//                near = value;
//                SetWhenReady(camera, Camera3D.PropertyName.Near, value);
//            }
//        }

//        /// <summary>
//        /// Mapped property from <see cref="Camera3D.Far"/>
//        /// </summary>
//        [Export]
//        public double Far
//        {
//            get
//            {
//                return far;
//            }
//            set
//            {
//                far = value;
//                SetWhenReady(camera, Camera3D.PropertyName.Far, value);
//            }
//        }

//        /// <summary>
//        /// Mapped property from <see cref="Camera3D.KeepAspect"/>
//        /// </summary>
//        [ExportCategory("Camera3D")]
//        [Export]
//        public Camera3D.KeepAspectEnum KeepAspect
//        {
//            get
//            {
//                return keepAspect;
//            }

//            set
//            {
//                keepAspect = value;
//                camera.KeepAspect = value;
//            }
//        }

//        /// <summary>
//        /// Mapped property from <see cref="Camera3D.DopplerTracking"/>
//        /// </summary>
//        [Export]
//        public Camera3D.DopplerTrackingEnum DopplerTracking
//        {
//            get
//            {
//                return dopplerTracking;
//            }

//            set
//            {
//                dopplerTracking = value;
//                camera.DopplerTracking = value;
//            }
//        }

//        /// <summary>
//        /// Mapped property of <see cref="Camera3D.CullMask"/>
//        /// </summary>
//        [Export(PropertyHint.Layers3DRender)]
//        public uint CullMask
//        {
//            get
//            {
//                return cullMask;
//            }

//            set
//            {
//                cullMask = value;
//                SetWhenReady(camera, Camera3D.PropertyName.CullMask, value);
//            }
//        }

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

//        // --------------------------------------------------------------------------------
//        // SpringArm
//        private int springArmCollissionMask = 1;
//        [Export(PropertyHint.Layers3DRender)]
//        public int SpringArmCollissionMask
//        {
//            get
//            {
//                return springArmCollissionMask;
//            }
//            set
//            {
//                springArmCollissionMask = value;
//                SetWhenReady(springArm, SpringArm3D.PropertyName.CollisionMask, value);
//            }
//        }

//        private float springArmCollissionMargin = 0.01f;
//        /// <summary>
//        /// Property matching <see cref="SpringArm3D.Margin"/>.
//        /// </summary>
//        [Export(PropertyHint.Range, "0.0, 100.0, 0.01, or_greater, or_less, hide_slider, suffix:m")]
//        public float SpringArmCollissionMargin
//        {
//            get
//            {
//                return springArmCollissionMargin;
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
//                return springArmLength;
//            }

//            set
//            {
//                springArmLength = value;
//                SetWhenReady(springArm, SpringArm3D.PropertyName.SpringLength, springArmLength);
//            }
//        }

//        // --------------------------------------------------------------------------------
//        // Methods
//        // --------------------------------------------------------------------------------
//        // Main thread
//        public override void _Process(double delta)
//        {
//            UpdateCamera();
//            UpdateEditor();
//            TweenGizmo();
//            TweenCameraToMarker();
//            SetOffsetPosition();
//            SetRotationPivot();
//            ProcessTiltInput();
//            ProcessHorizontalRotationInput();
//            UpdateCameraTilt();
//            UpdateCameraHorizontalRotation();
//        }

//        /// <summary>
//        /// On ready, it creates the tree with all nodes. Then after each frame, values are refreshed.
//        /// </summary>
//        public override void _Ready()
//        {
//            CallDeferred(MethodName.CreateTree);
//        }

//        // --------------------------------------------------------------------------------
//        // Creational
//        /// <summary>
//        /// Creates necessary to initialise the third person camera.
//        ///
//        /// Tree created:
//        ///     CameraRoot
//        ///     |_RotationPivot
//        ///     | |_OffsetPivot
//        ///     | |_CameraSpringArm
//        ///     |   |_CameraMarker
//        ///     |_Camera3D
//        /// 
//        /// 
//        /// </summary>
//        private void CreateTree()
//        {
//            // Populate fields with new objects.
//            cameraMarker = new Marker3D();
//            springArm = new SpringArm3D();
//            camera = new Camera3D();
//            rotationPivot = new Node3D();
//            offsetPivot = new Node3D();

//            // Build tree:
//            // Marker goes inside the SpringArm
//            springArm.AddChild(cameraMarker);

//            // Add all to RotationPivot
//            rotationPivot.AddChild(offsetPivot);
//            rotationPivot.AddChild(springArm);

//            // Camera3D and RotationPivot go inside Camera.
//            AddChild(camera);
//            AddChild(rotationPivot);

//            // Check if DebugGizmo is enabled.
//            if (EnableGizmo) gizmo = GetNode<MeshInstance3D>("DebugGizmo");
//            MapCamera();
//        }

//        private void MapCamera()
//        {
//            current = camera.Current;
//            keepAspect = camera.KeepAspect;
//            cullMask = camera.CullMask;
//            dopplerTracking = camera.DopplerTracking;
//            projectionType = camera.Projection;
//            fov = camera.Fov;
//            near = camera.Near;
//            far = camera.Far;

//            springArmCollissionMask = (int)springArm.CollisionMask;
//            springArmCollissionMargin = (uint)springArm.Margin;
//            springArmLength = springArm.SpringLength;

//        }

//        /// <summary>
//        /// Waits until a node is ready, and sets a value inside a Node's property. Executes asyncronously.
//        /// </summary>
//        /// <param name="n">Node to set a property in.</param>
//        /// <param name="property">Property to change.</param>
//        /// <param name="value">Value to set.</param>
//        async void SetWhenReady(Node n, StringName property, Variant value)
//        {
//            if (IsNodeReady())
//            {
//                if (!n.IsNodeReady() && n != null)
//                {
//                    "Node is not ready. Awaiting...".ToConsole();
//                    await ToSignal(this, SignalName.Ready);
//                    "Node is ready.".ToConsole();
//                    n.Set(property, value);
//                    $"\t{property} = {n.Get(property)} set".ToConsole();
//                }
//                else
//                {
//                    $"Node is ready.".ToConsole();
//                    n.Set(property, value);
//                    $"\t{property} = {value} set".ToConsole();
//                }
//            }
//        }

//        // --------------------------------------------------------------------------------
//        // Processing

//        private void UpdateEditor()
//        {
//            if (Engine.IsEditorHint())
//            {
//                CreateTree();
//                var a = new Vector3(0.0f, 0.0f, 1.0f);
//                var rotationA = new Vector3(1.0f, 0.0f, 0.0f);
//                var rotationB = Mathf.DegToRad(InitialDiveAngle);
//                var rotationC = Mathf.DegToRad(-HorizontalRotationAngle);

//                Vector3 cameraMarkerGlobalPosition =
//                        (a.Rotated(axis: rotationA,
//                        angle: (float)rotationB)
//                        .Rotated(
//                            axis: new Vector3(x: 0.0f, y: 1.0f, z: 0.0f),
//                            angle: (float)rotationC)
//                        * springArm.SpringLength) + springArm.GlobalPosition;

//                cameraMarker.GlobalPosition = cameraMarkerGlobalPosition;
//            }
//        }

//        private async void UpdateCamera()
//        {
//            void update()
//            {
//                camera.KeepAspect = keepAspect;
//                camera.DopplerTracking = dopplerTracking;
//                camera.Projection = projectionType;
//                camera.Set(Camera3D.PropertyName.CullMask, cullMask);
//                camera.Set(Camera3D.PropertyName.Fov, fov);
//                camera.Set(Camera3D.PropertyName.Far, far);
//                camera.Set(Camera3D.PropertyName.Near, near);
//                $"\tCamera updated".ToConsole();
//            }

//            if (IsNodeReady())
//            {
//                if (!camera.IsNodeReady() && camera != null)
//                {
//                    "Node is not ready. Awaiting...".ToConsole();
//                    await ToSignal(this, SignalName.Ready);
//                    "Node is ready.".ToConsole();
//                    update();
//                }
//                else
//                {
//                    update();
//                }
//            }
//        }

//        /// <summary>
//        /// If <see cref="EnableGizmo"/> is true, tweens the gizmo to the Camera's rotation.
//        /// </summary>
//        private void TweenGizmo()
//        {
//            if (EnableGizmo && gizmo != null)
//            {
//                var cameraRotation = camera.Rotation;
//                gizmo.Rotation = cameraRotation;
//            }
//        }

//        /// <summary>
//        /// Moves the camera by the values of its parent and the offset in <see cref="PivotOffset"/>.
//        /// </summary>
//        private void SetOffsetPosition()
//        {
//            // Get the position of the player of which this camera is attached to.
//            var parent = offsetPivot.GetParent() as Node3D;

//            // Set new position. If there's a PivotOffset value, it's traslated by those values.
//            offsetPivot.GlobalPosition = parent.ToGlobal(new Vector3(PivotOffset.X, PivotOffset.Y, 0.0f));
//        }

//        /// <summary>
//        /// Sets the angle and position of the RotationPivot.
//        /// </summary>
//        private void SetRotationPivot()
//        {
//            var rotationPivotPosition = rotationPivot.GlobalPosition;

//            rotationPivot.GlobalRotation = GlobalPosition;
//            rotationPivotPosition.X = InitialDiveAngle;
//        }

//        /// <summary>
//        /// Processes lateral camera movement.
//        /// </summary>
//        private void ProcessHorizontalRotationInput()
//        {
//            if (InputMap.HasAction("tp_camera_right") && InputMap.HasAction("tp_camera_left"))
//            {
//                float cameraHorizontalVarition = Input.GetActionStrength("tp_camera_right") - Input.GetActionStrength("tp_camera_left");
//                cameraHorizontalVarition = cameraHorizontalVarition * (float)GetProcessDeltaTime() * 30 * HorizontalRotationSensitivity;
//                HorizontalRotationAngle += cameraHorizontalVarition;
//            }
//        }

//        /// <summary>
//        /// Processes vertical camera movement. Angle is always between upper and lower limits.
//        /// </summary>
//        private void ProcessTiltInput()
//        {
//            if (InputMap.HasAction("tp_camera_up") && InputMap.HasAction("tp_camera_down"))
//            {
//                float tiltVariation = Input.GetActionStrength("tp_camera_up") - Input.GetActionStrength("tp_camera_down");
//                tiltVariation = tiltVariation * (float)GetProcessDeltaTime() * 5 * TiltSensitivity;
//                CameraTiltAngle = Math.Clamp(CameraTiltAngle + tiltVariation, TiltLowerLimit - InitialDiveAngle, TiltUpperLimit - InitialDiveAngle);
//            }
//        }

//        private void UpdateCameraTilt()
//        {
//            // Clamps current value between the limits.
//            var finalTiltValue = Mathf.Clamp(InitialDiveAngle + CameraTiltAngle, TiltLowerLimit, TiltUpperLimit);

//            // Interpolates the X angle to the final calculated value in 0.1 seconds.
//            var tween = CreateTween();
//            tween.TweenProperty(camera, "global_rotation_degrees:x", finalTiltValue, 0.1);
//        }

//        // needs some review
//        private void UpdateCameraHorizontalRotation()
//        {
//            var tween = CreateTween();
//            tween.TweenProperty(rotationPivot, "global_rotation_degrees:y", HorizontalRotationAngle * -1, 0.1).AsRelative();
//            HorizontalRotationAngle = 0.0f; // reset value

//            // check this part here.
//            // Calculates a normalized vector between the camera and the offset pivot X and Z.
//            Vector2 vectToOffsetPivot =
//                // offsetPivotX,Y
//                (new Vector2(offsetPivot.GlobalPosition.X, offsetPivot.GlobalPosition.Z)
//                // cameraX,Y
//                - new Vector2(camera.GlobalPosition.X, camera.GlobalPosition.Z))
//                .Normalized();

//            // assign to Y.
//            var cameraRotationY = camera.GlobalRotation.Y;
//            cameraRotationY = -new Vector2(0.0f, -1.0f).AngleTo(vectToOffsetPivot.Normalized());
//        }

//        /// <summary>
//        /// Tweens camera to the marker inside the SpringArm.
//        /// </summary>
//        private void TweenCameraToMarker()
//        {
//            var cameraPosition = camera.GlobalPosition;
//            var markerPosition = cameraMarker.GlobalPosition;
//            cameraPosition.X = Mathf.Lerp(cameraPosition.X, markerPosition.X, 10);
//            cameraPosition.Y = Mathf.Lerp(cameraPosition.Y, markerPosition.Y, 10);
//            cameraPosition.Z = Mathf.Lerp(cameraPosition.Z, markerPosition.Z, 10);
//        }
//    }

//    // Debugging
//    public partial class AngelCamera
//    {
//        // --------------------------------------------------------------------------------
//        // Debug
//        /// <summary>
//        /// Debug mesh. Base points towards the Camera's viewpoint.
//        /// </summary>
//        [Export]
//        public bool EnableGizmo { get; set; } = true;

//        /// <summary>
//        /// Prints a table with values.
//        /// </summary>
//        private void DbgStr_ConsoleDebug()
//        {
//            var table = new ConsoleTable("Name", "X", "Y", "Z");
//            table
//                .AddRow("Camera Global Position", GlobalPosition.X, GlobalPosition.Y, GlobalPosition.Z)
//                .AddRow("Offset Pivot Position", offsetPivot.GlobalPosition.X, offsetPivot.GlobalPosition.Y, offsetPivot.GlobalPosition.Z)
//                .AddRow("SpringArm Distance", SpringArmDistance, 0, 0)
//                .AddRow("Rotation Pivot Position", rotationPivot.GlobalPosition.X, rotationPivot.GlobalPosition.Y, rotationPivot.GlobalPosition.Z)
//                .AddRow("Rotation Pivot Angle", rotationPivot.GlobalRotationDegrees.X, rotationPivot.GlobalRotationDegrees.Y, rotationPivot.GlobalRotationDegrees.Z);
//            table.ToString().ToConsole();
//        }

//        private static SignalManager SignalManager
//        {
//            get
//            {
//                return SignalManager.Get();
//            }
//        }

//        private void AngelCamera_ChildExitingTree(Node node) => SignalManager.DbgMsg_ChildExitingTree(this, node);

//        private void AngelCamera_ChildEnteredTree(Node node) => SignalManager.DbgMsg_ChildEnteringTree(this, node);

//        protected virtual void Enter(Node node) => SignalManager.DbgMsg_ChildEnteringTree(this, node);

//        protected virtual void Exit(Node node) => SignalManager.DbgMsg_ChildExitingTree(this, node);

//        protected virtual void OrderChanged() => SignalManager.DbgMsg_OrderChanged(this);

//        protected virtual void OnEditorDescriptionChanged(Node node) => SignalManager.DbgMsg_EditorDescriptionChanged(this, node);

//        protected virtual void OnReady() => SignalManager.DbgMsg_OnReady(this);

//        protected virtual void OnRenamed() => SignalManager.DbgMsg_Renamed(this);

//        protected virtual void OnReplacing(Node node) => SignalManager.DbgMsg_Replaced(this, node);

//        protected virtual void OnEnteredTree() => SignalManager.DbgMsg_EnteredTree(this);

//        protected virtual void OnEnteringTree() => SignalManager.DbgMsg_Entering(this);

//        protected virtual void OnExitedTree() => SignalManager.DbgMsg_Exited(this);

//        protected virtual void OnTreeExiting() => SignalManager.DbgMsg_Exiting(this);

//        protected virtual void OnPropertyListChanged() => SignalManager.DbgMsg_PropertyChanged(this);

//        protected virtual void OnScriptChanged() => SignalManager.DbgMsg_ScriptChanged(this);
//    }
//}
