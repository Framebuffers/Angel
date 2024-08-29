//using Godot;

//namespace Angel
//{
//    [GlobalClass]
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
//    public partial class AngelCamera : Node3D
//    {
//        private Camera3D camera;
//        private Vector3 ParentCoordinates;
//        private SpringArm3D springArm;
//        private Node3D cameraRotationPivot;
//        private Node3D cameraOffsetPivot;
//        private Marker3D cameraMarker;
//        private MeshInstance3D gizmo;

//        // --------------------------------------------------------------------------------
//        // Main thread
//        /// <summary>
//        /// On ready, it creates the tree with all nodes. Then after each frame, values are refreshed.
//        /// </summary>
//        public override void _Ready()
//        {
//            CallDeferred(MethodName.CreateTree);
//            CallDeferred(MethodName.MapCamera);
//        }

//        /// <summary>
//        /// Each frame:
//        ///     - Camera fields inside Angel are written inside <see cref="camera"/>.
//        ///     - If <see cref="Engine.IsEditorHint()"/> is true, updates the camera inside the editor.
//        ///     - If <see cref="gizmo"/> is enabled, tweens the mesh to the camera.
//        ///     - <see cref="camera"/> is tweened with <see cref="cameraMarker"/>.
//        ///     - An offset is calculated between <see cref="PivotOffset"/> and the parent Player.
//        ///     - <see cref="cameraRotationPivot"/> angle is set to the 
//        /// </summary>
//        /// <param name="delta">Time between frames.</param>
//        public override void _Process(double delta)
//        {
//            UpdateCamera();

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

//            //UpdateEditor();
//            //TweenGizmo();
//            TweenCameraToMarker();
            
//            //SetOffsetPosition();
//            cameraOffsetPivot.GlobalPosition = cameraOffsetPivot.GetParent<Node3D>().ToGlobal(new Vector3(PivotOffset.X, PivotOffset.Y, 0.0f));

//            //SetRotationPivot();
//            Vector3 rotationPivotGlobalRotationDeg = cameraRotationPivot.GlobalRotationDegrees;
//            rotationPivotGlobalRotationDeg.X = initialDiveAngle;
//            cameraRotationPivot.GlobalPosition = GetParent<Node3D>().GlobalPosition;

//            ProcessTiltInput();
//            ProcessHorizontalRotationInput();
//            UpdateCameraTilt();
//            UpdateCameraHorizontalRotation();
//        }
//    }
//}