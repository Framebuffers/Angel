//using Angel.Helpers;
//using ConsoleTables;
//using Godot;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Angel
//{
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
//                .AddRow("Offset Pivot Position", cameraOffsetPivot.GlobalPosition.X, cameraOffsetPivot.GlobalPosition.Y, cameraOffsetPivot.GlobalPosition.Z)
//                .AddRow("SpringArm Distance", SpringArmDistance, 0, 0)
//                .AddRow("Rotation Pivot Position", cameraRotationPivot.GlobalPosition.X, cameraRotationPivot.GlobalPosition.Y, cameraRotationPivot.GlobalPosition.Z)
//                .AddRow("Rotation Pivot Angle", cameraRotationPivot.GlobalRotationDegrees.X, cameraRotationPivot.GlobalRotationDegrees.Y, cameraRotationPivot.GlobalRotationDegrees.Z);
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
