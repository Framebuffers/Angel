using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angel
{
    // --------------------------------------------------------------------------------
    // Creational Methods
    public partial class AngelCamera
    {
        /// <summary>
        /// Creates necessary to initialise the third person camera.
        ///
        /// Tree created:
        ///     CameraRoot
        ///     |_RotationPivot
        ///     | |_OffsetPivot
        ///     | |_CameraSpringArm
        ///     |   |_CameraMarker
        ///     |_Camera3D
        /// 
        /// 
        /// </summary>
        private void CreateTree()
        {
            camera = GetNode<Camera3D>("Camera");
            cameraRotationPivot = GetNode<Node3D>("RotationPivot");
            cameraOffsetPivot = GetNode<Node3D>("RotationPivot/OffsetPivot");
            springArm = GetNode<SpringArm3D>("RotationPivot/OffsetPivot/CameraSpringArm");
            cameraMarker = GetNode<Marker3D>("RotationPivot/OffsetPivot/CameraSpringArm/CameraMarker");
            gizmo = GetNode<MeshInstance3D>("RotationPivot/OffsetPivot/PivotDebug");
            //cameraShaker = GetNode("CameraShaker") as TPCShaker;
            //camera.TopLevel = true;
            camera.Current = true;

            //// Populate fields with new objects.
            //cameraMarker = new Marker3D();
            //springArm = new SpringArm3D();
            //camera = new Camera3D();
            //cameraRotationPivot = new Node3D();
            //offsetPivot = new Node3D();

            //// Build tree:
            //// Marker goes inside the SpringArm
            //springArm.AddChild(cameraMarker);

            //// Add all to RotationPivot
            //cameraRotationPivot.AddChild(offsetPivot);
            //cameraRotationPivot.AddChild(springArm);

            //// Camera3D and RotationPivot go inside Camera.
            //AddChild(camera);
            //AddChild(cameraRotationPivot);

            //// Check if DebugGizmo is enabled.
            //if (EnableGizmo) gizmo = GetNode<MeshInstance3D>("DebugGizmo");
            //MapCamera();
        }

        private void MapCamera()
        {
            current = camera.Current;
            keepAspect = camera.KeepAspect;
            cullMask = camera.CullMask;
            dopplerTracking = camera.DopplerTracking;
            projectionType = camera.Projection;
            fov = camera.Fov;
            near = camera.Near;
            far = camera.Far;

            springArmCollissionMask = springArm.CollisionMask;
            springArmCollissionMargin = (uint)springArm.Margin;
            springArmLength = springArm.SpringLength;

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
}
