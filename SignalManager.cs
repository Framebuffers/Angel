using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Angel.Helpers
{
    /// <summary>
    /// Hooks into Godot signals and offers several boilerplate functions.
    /// </summary>
    public sealed partial class SignalManager : Node
    {
        private SignalManager() { }
        private static SignalManager _instance;
        private static readonly object _lock = new();
        public static SignalManager Get()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new SignalManager();
                    "Manager message: New SignalManager instance\n".ToConsole();
                }
            }
            return _instance;
        }

        public void DbgMsg_VisibilityChanged(Node3D source)
        {
            string visibility = source.IsVisibleInTree() ? "Visible" : "Invisible";
            $"Message from {source.Name}:\n\tNode {source.Name} is now {visibility}".ToConsole();
        }

        public void DbgMsg_VisibilityChanged(Node2D source)
        {
            string visibility = source.IsVisibleInTree() ? "Visible" : "Invisible";
            $"Message from {source.Name}:\n\tNode {source.Name} is now {visibility}".ToConsole();
        }

        public void DbgMsg_ChildExitingTree(Node source, Node target) => $"Message from {source.Name}:\n\tNode exiting tree:\n\t{target.GetPath().GetConcatenatedNames()}\n".ToConsole();

        public void DbgMsg_ChildEnteringTree(Node source, Node target) => $"Message from {source.Name}:\n\tNode entering tree:\n\t{target.GetPath().GetConcatenatedNames()}\n".ToConsole();

        public void DbgMsg_OrderChanged(Node source) => $"Message from {source.Name}:\n\tNode order changed.\n".ToConsole();

        public void DbgMsg_EditorDescriptionChanged(Node source, Node node) => $"Message from {source.Name}:\n\tNode {node.Name} has new description:\n\t{node.EditorDescription}\n".ToConsole();

        public void DbgMsg_OnReady(Node source) => $"Message from {source.Name}:\n\tThis node is ready.\n".ToConsole();

        public void DbgMsg_Renamed(Node source) => $"Message from {source.Name}:\n\tThis node has been renamed.\n".ToConsole();
        public void DbgMsg_Replaced(Node source, Node node) => $"Message from {source.Name}:\n\tThis node is being replaced by: \n\t{node.GetPath().GetConcatenatedNames()}\n".ToConsole();

        public void DbgMsg_EnteredTree(Node source) => $"Message from {source.Name}:\n\tThis node has entered the tree.\n".ToConsole();

        public void DbgMsg_Entering(Node source) => $"Message from {source.Name}:\n\tThis node is entering the tree.\n".ToConsole();

        public void DbgMsg_Exiting(Node source) => $"Message from {source.Name}:\n\tThis node is exiting the tree.\n".ToConsole();

        public void DbgMsg_Exited(Node source) => $"Message from {source.Name}:\n\tThis node has left the tree.\n".ToConsole();

        public void DbgMsg_PropertyChanged(Node source) => $"Message from {source.Name}:\n\tThis object's property list has changed.\n".ToConsole();

        public void DbgMsg_ScriptChanged(Node source) => $"Message from {source.Name}:\n\tThis script has been modified.\n".ToConsole();

    }
}
