﻿using Angel;
using Angel.Helpers;
using Godot;
using System;

[GlobalClass]
public partial class AngelCamera : Node3D
{
    private SignalManager SignalManager;
    public override void _Ready() => GetTree().Root.AddChild(this);
    protected virtual void Enter(Node node) => SignalManager.DbgMsg_ChildEnteringTree(this, node);

    protected virtual void Exit(Node node) => SignalManager.DbgMsg_ChildExitingTree(this, node);

    protected virtual void OrderChanged() => SignalManager.DbgMsg_OrderChanged(this);

    protected virtual void OnEditorDescriptionChanged(Node node) => SignalManager.DbgMsg_EditorDescriptionChanged(this, node);

    protected virtual void OnReady() => SignalManager.DbgMsg_OnReady(this);

    protected virtual void OnRenamed() => SignalManager.DbgMsg_Renamed(this);

    protected virtual void OnReplacing(Node node) => SignalManager.DbgMsg_Replaced(this, node);

    protected virtual void OnEnteredTree() => SignalManager.DbgMsg_EnteredTree(this);

    protected virtual void OnEnteringTree() => SignalManager.DbgMsg_Entering(this);

    protected virtual void OnExitedTree() => SignalManager.DbgMsg_Exited(this);

    protected virtual void OnTreeExiting() => SignalManager.DbgMsg_Exiting(this);

    protected virtual void OnPropertyListChanged() => SignalManager.DbgMsg_PropertyChanged(this);

    protected virtual void OnScriptChanged() => SignalManager.DbgMsg_ScriptChanged(this);
}
