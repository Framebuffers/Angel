using Angel;
using Angel.Helpers;
using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Test : Node3D
{
    private SignalManager SignalManager { get => SignalManager.Get(); }
    // Called when the node enters the scene tree for the first time.

    public override void _EnterTree()
    {
        
    }
    public override void _Ready()
    {
        //CallDeferred(MethodName.BindSignalsToManager, SignalManager);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    //async void WaitForNode(Node n /*StringName property, Variant value*/)
    //{
    //    if (!n.IsNodeReady())
    //    {
    //        "Node is not ready. Awaiting...".ToConsole();
    //        await ToSignal(n.GetTree(), SignalName.Ready);
    //        "Node is ready.".ToConsole();
    //        //n.Set(property, value);
    //    }
    //    else
    //    {
    //        "Node is ready.".ToConsole();
    //        //n.Set(property, value);
    //    }
    //}

    // Node3D
    protected virtual void OnVisibilityChanged() => SignalManager.DbgMsg_VisibilityChanged(this);
    protected virtual void OnChildEnteredTree(Node node)
    {
        SignalManager.DbgMsg_ChildEnteringTree(this, node);
    }

    protected virtual void OnChildExitingTree(Node node) => SignalManager.DbgMsg_ChildExitingTree(this, node);

    // Node
    protected virtual void OnOrderChanged() => SignalManager.DbgMsg_OrderChanged(this);

    protected virtual void OnEditorDescriptionChanged(Node node) => SignalManager.DbgMsg_EditorDescriptionChanged(this, node);

    protected virtual void OnReady() => SignalManager.DbgMsg_OnReady(this);

    protected virtual void OnRenamed() => SignalManager.DbgMsg_Renamed(this);

    protected virtual void OnReplacing(Node node) => SignalManager.DbgMsg_Replaced(this, node);

    protected virtual void OnEnteredTree() => SignalManager.DbgMsg_EnteredTree(this);

    protected virtual void OnEnteringTree() => SignalManager.DbgMsg_Entering(this);

    protected virtual void OnExitedTree() => SignalManager.DbgMsg_Exited(this);

    protected virtual void OnTreeExiting() => SignalManager.DbgMsg_Exiting(this);

    // Object
    protected virtual void OnPropertyListChanged() => SignalManager.DbgMsg_PropertyChanged(this);

    protected virtual void OnScriptChanged() => SignalManager.DbgMsg_ScriptChanged(this);
}
