using ThirdPersonCamera.Objects.Interfaces;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPersonCamera.Objects
{
    internal partial class ObjectTree : Node3D, IObjectTree
    {
        private Node3D _parent;
        private Camera3D _camera;
        private Vector3 _parentCoordinates;
        private SpringArm3D _springArm;
        private Node3D _rotationPivot;
        private Node3D _offsetPivot;
        private Marker3D _cameraMarker;
        private MeshInstance3D _gizmo;
        private Tween _tween;

        public ObjectTree(Node3D parent) => CallDeferred(MethodName.InitTree, parent);

        public ObjectTree(Node3D parent, MeshInstance3D gizmo)
        {
            CallDeferred(MethodName.InitTree, parent);
            _gizmo = gizmo;
        }

        private void InitTree(Node3D parent)
        {
            _parent = parent;

            // Populate fields with new objects.
            _camera = new Camera3D();
            _cameraMarker = new Marker3D();
            _springArm = new SpringArm3D();
            _rotationPivot = new Node3D();
            _offsetPivot = new Node3D();

            // Build tree:
            // Marker goes inside the SpringArm
            _springArm.AddChild(_cameraMarker);

            // Add all to RotationPivot
            _rotationPivot.AddChild(_offsetPivot);
            _rotationPivot.AddChild(_springArm);

            // Camera3D and RotationPivot go inside Camera.
            AddChild(_camera);
            AddChild(_rotationPivot);
            
            // Add this to the player.
            parent.AddChild(this);
        }

        public Camera3D GetCamera() => _camera;

        public void TweenCameraToPlayer()
        {
            if (InputMap.HasAction("tween_camera"))
            {
                Vector3 target = GlobalTransform.Origin + _springArm.GlobalTransform.Origin;
                _tween.TweenProperty(
                    _camera,
                    "global_transform:origin",
                    target,
                    0.1f);
                _tween.Play();
            }
        }
    }

    
}
