using Godot;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angel.Objects.Interfaces
{
    /// <summary>
    /// Exposes properties for the SpringArm3D between the Player and the Camera.
    /// </summary>
    internal interface ICameraDistance
    {
        SpringArm3D GetSpringArm();
        uint GetSpringArmCollissionMask();
        void SetSpringArmCollissionMask(uint mask);
        float GetSpringArmMargin();
        void SetSpringArmDistance(float distance);
        float GetSpringArmDistance();
        void SetSpringArmMargin(float margin);
    }
}
