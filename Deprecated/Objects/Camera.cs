//using Godot;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Angel.Objects;
//using Angel.Objects.Interfaces;

//namespace Angel.Objects
//{
//    [GlobalClass]
//    public partial class Camera : Node3D
//    {
//        private IObjectTree Objects;
//        private CameraDistance Distance;

//        // remove
//        private Camera3D ActiveCamera { get => Objects.GetCamera(); }

//        public override void _Ready()
//        {
//            CallDeferred(MethodName.Init);
//        }

//        public override void _Process(double delta)
//        {
//            Objects.TweenCameraToPlayer();
//        }

//        void Init()
//        {
//            Objects = new ObjectTree(GetParent() as Node3D);
//            Distance = new CameraDistance(Objects);
//        }

//    }
//}
