using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Angel
{
    public partial class AngelCamera
    {
        // --------------------------------------------------------------------------------
        // Processing

        private void UpdateEditor()
        {
            if (Engine.IsEditorHint())
            {
                CreateTree();
                var a = new Vector3(0.0f, 0.0f, 1.0f);
                var rotationA = new Vector3(1.0f, 0.0f, 0.0f);
                var rotationB = Mathf.DegToRad(InitialDiveAngle);
                var rotationC = Mathf.DegToRad(-HorizontalRotationAngle);

                Vector3 cameraMarkerGlobalPosition =
                        (a.Rotated(axis: rotationA,
                        angle: (float)rotationB)
                        .Rotated(
                            axis: new Vector3(x: 0.0f, y: 1.0f, z: 0.0f),
                            angle: (float)rotationC)
                        * springArm.SpringLength) + springArm.GlobalPosition;

                cameraMarker.GlobalPosition = cameraMarkerGlobalPosition;
            }
        }

        private async void UpdateCamera()
        {
            void update()
            {
                camera.KeepAspect = keepAspect;
                camera.DopplerTracking = dopplerTracking;
                camera.Projection = projectionType;
                camera.Set(Camera3D.PropertyName.CullMask, cullMask);
                camera.Set(Camera3D.PropertyName.Fov, fov);
                camera.Set(Camera3D.PropertyName.Far, far);
                camera.Set(Camera3D.PropertyName.Near, near);
                $"\tCamera updated".ToConsole();
            }

            if (IsNodeReady())
            {
                if (!camera.IsNodeReady() && camera != null)
                {
                    "Node is not ready. Awaiting...".ToConsole();
                    await ToSignal(this, SignalName.Ready);
                    "Node is ready.".ToConsole();
                    update();
                }
                else
                {
                    update();
                }
            }
        }

        /// <summary>
        /// If <see cref="EnableGizmo"/> is true, tweens the gizmo to the Camera's rotation.
        /// </summary>
        private void TweenGizmo()
        {
            if (EnableGizmo && gizmo != null && Engine.IsEditorHint())
            {
                var cameraRotation = camera.Rotation;
                gizmo.Rotation = cameraRotation;
            }
        }

        /// <summary>
        /// Moves the camera by the values of its parent and the offset in <see cref="PivotOffset"/>.
        /// </summary>
        private void SetOffsetPosition() =>
            // Set new position. If there's a PivotOffset value, it's traslated by those values.
            cameraOffsetPivot.GlobalPosition = cameraOffsetPivot.GetParent<Node3D>().ToGlobal(new Vector3(PivotOffset.X, PivotOffset.Y, 0.0f));

        /// <summary>
        /// Sets the angle and position of the RotationPivot.
        /// </summary>
        private void SetRotationPivot()
        {
            Vector3 rotationPivotGlobalRotationDeg = cameraRotationPivot.GlobalRotationDegrees;
            rotationPivotGlobalRotationDeg.X = initialDiveAngle;
            cameraRotationPivot.GlobalPosition = GetParent<Node3D>().GlobalPosition;
        }

        /// <summary>
        /// Processes lateral camera movement.
        /// </summary>
        private void ProcessHorizontalRotationInput()
        {
            if (InputMap.HasAction("tp_camera_right") && InputMap.HasAction("tp_camera_left"))
            {
                float cameraHorizontalVarition = Input.GetActionStrength("tp_camera_right") - Input.GetActionStrength("tp_camera_left");
                cameraHorizontalVarition = cameraHorizontalVarition * (float)GetProcessDeltaTime() * 30 * HorizontalRotationSensitivity;
                HorizontalRotationAngle += cameraHorizontalVarition;
            }
        }

        /// <summary>
        /// Processes vertical camera movement. Angle is always between upper and lower limits.
        /// </summary>
        private void ProcessTiltInput()
        {
            if (InputMap.HasAction("tp_camera_up") && InputMap.HasAction("tp_camera_down"))
            {
                float tiltVariation = Input.GetActionStrength("tp_camera_up") - Input.GetActionStrength("tp_camera_down");
                tiltVariation = tiltVariation * (float)GetProcessDeltaTime() * 5 * TiltSensitivity;
                CameraTiltAngle = Math.Clamp(CameraTiltAngle + tiltVariation, TiltLowerLimit - InitialDiveAngle, TiltUpperLimit - InitialDiveAngle);
            }
        }

        private void UpdateCameraTilt()
        {
            // Clamps current value between the limits.
            var finalTiltValue = Mathf.Clamp(InitialDiveAngle + CameraTiltAngle, TiltLowerLimit, TiltUpperLimit);

            // Interpolates the X angle to the final calculated value in 0.1 seconds.
            var tween = CreateTween();
            tween.TweenProperty(camera, "global_rotation_degrees:x", finalTiltValue, 0.1);
        }

        // needs some review
        private void UpdateCameraHorizontalRotation()
        {
            var tween = CreateTween();
            tween.TweenProperty(cameraRotationPivot, "global_rotation_degrees:y", HorizontalRotationAngle * -1, 0.1).AsRelative();
            HorizontalRotationAngle = 0.0f; // reset value

            // check this part here.
            // Calculates a normalized vector between the camera and the offset pivot X and Z.
            Vector2 vectToOffsetPivot =
                // offsetPivotX,Y
                (new Vector2(cameraOffsetPivot.GlobalPosition.X, cameraOffsetPivot.GlobalPosition.Z)
                // cameraX,Y
                - new Vector2(camera.GlobalPosition.X, camera.GlobalPosition.Z))
                .Normalized();

            // assign to Y.
            var cameraRotation = camera.GlobalRotation;
            cameraRotation.Y = -new Vector2(0.0f, -1.0f).AngleTo(vectToOffsetPivot);
        }

        private Vector3 GetTargetPosition()
        {
            return GlobalTransform.Origin - springArm.Transform.Basis.Z * springArm.SpringLength;
        }

        /// <summary>
        /// Tweens camera to the marker inside the SpringArm.
        /// </summary>
        private void TweenCameraToMarker()
        {
            //cameraMarker.GlobalPosition = GetTargetPosition();
            camera.GlobalTransform = new Transform3D(camera.GlobalBasis, camera.GlobalTransform.Origin.Lerp(cameraMarker.GlobalTransform.Origin, CameraSpeed));
            //var a = new Vector3(0.0f, 0.0f, 1.0f);
            //var rotationA = new Vector3(1.0f, 0.0f, 0.0f);
            //var rotationB = Mathf.DegToRad(InitialDiveAngle);
            //var rotationC = Mathf.DegToRad(-HorizontalRotationAngle);

            //cameraMarker.GlobalPosition =
            //        (a.Rotated(axis: rotationA,
            //        angle: (float)rotationB)
            //        .Rotated(
            //            axis: new Vector3(x: 0.0f, y: 1.0f, z: 0.0f),
            //            angle: (float)rotationC)
            //        * springArm.SpringLength) + springArm.GlobalPosition;

            //cameraMarker.GlobalPosition = springArm.GlobalTransform.Origin + springArm.GlobalTransform.Basis.Z * springArm.SpringLength;

            //camera.GlobalTransform = new Transform3D(camera.GlobalBasis, camera.GlobalTransform.Origin.Lerp(cameraMarker.GlobalTransform.Origin, 0.1f));


            //camera.GlobalTransform = Mathf.Lerp((float)camera.GlobalTransform.Basis, GetTargetPosition(), 0.1f * GetProcessDeltaTime());

            //var cameraPosition = camera.GlobalPosition;
            //var markerPosition = cameraMarker.GlobalPosition;



            //cameraPosition.X = Mathf.Lerp(cameraPosition.X, markerPosition.X, 10);
            //cameraPosition.Y = Mathf.Lerp(cameraPosition.Y, markerPosition.Y, 10);
            //cameraPosition.Z = Mathf.Lerp(cameraPosition.Z, markerPosition.Z, 10);

            //cameraPosition.X = Mathf.Lerp(cameraPosition.X, markerPosition.X, 10);
            //cameraPosition.Y = Mathf.Lerp(cameraPosition.Y, markerPosition.Y, 10);
            //cameraPosition.Z = Mathf.Lerp(cameraPosition.Z, markerPosition.Z, 10);


            //if (InputMap.HasAction("tween_camera") && camera != null)
            //{
            //    //Vector3 target = camera.GlobalTransform.Origin + springArm.GlobalTransform.Origin;
            //    using Tween tw = new();
            //    tw.TweenProperty(
            //        camera,
            //        "global_transform:basis",
            //        tw,
            //        0.5f
            //    );
            //    tw.Play();
            //}
        }
    }
}
