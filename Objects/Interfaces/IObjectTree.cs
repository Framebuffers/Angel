using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPersonCamera.Objects.Interfaces
{
    internal interface IObjectTree
    {
        Camera3D GetCamera();
        SpringArm3D GetSpringArm();
        void TweenCameraToPlayer();
        void SetWhenReady(Node target, StringName property, object value);
    }
}
