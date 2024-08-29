using Angel;
using Angel.Helpers;
using ConsoleTables;
using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Third Person Camera. Add as child to a node derived from CharacterBody3D.
/// 
/// How does it work?
/// There's two main objects: a RotationPivot and a Camera3D. 
/// The RotationPivot is "attached" to the Player it follows. The Camera3D renders to the Viewport.
/// Inside this RotationPivot, There's an OffsetPivot and a SpringArm3D.
/// The OffsetPivot lets the camera move away from the player.
/// Inside the SpringArm3D, there's a Marker3D for the Camera. 
/// The marker sets the target on which the Camera looks at.
/// The SpringArm is connected to the Camera.
/// </summary>
[GlobalClass]
public partial class AngelCamera : Node3D
{
    // todo: implement a way to debug values when they change.
    [Signal]
    public delegate void ValueChangedEventHandler(string name, string[] values);

    private Vector3 ParentCoordinates;
    private SpringArm3D springArm;
    private Node3D rotationPivot;
    private Node3D offsetPivot;
    private Marker3D cameraMarker;
    private Camera3D camera;
    private MeshInstance3D gizmo;
    private float initialDiveAngle = -45.0f;

    /// <summary>
    /// Debug mesh. Base points towards the Camera's viewpoint.
    /// </summary>
    [Export]
    public bool EnableGizmo { get; set; } = true;

    /// <summary>
    /// Offset coordinates from the pivot marker.
    /// </summary>
    [Export]
    public Vector2 PivotOffset { get; set; } = Vector2.Zero;

    /// <summary>
    /// RotationPivot X axis. Defines the angle against the Player on which the Camera will start pointing at.
    /// </summary>
    [Export(PropertyHint.Range, "-90.0,90.0")]
    public float InitialDiveAngle
    {
        get
        {
            return initialDiveAngle;
        }

        set
        {
            float v = Mathf.Clamp(value, TiltLowerLimit, TiltUpperLimit);
            initialDiveAngle = v;
        }
    }



    [Export(PropertyHint.Range, "-90.0,90.0")]
    public float TiltLowerLimit { get; set; } = -60.0f;

    [Export(PropertyHint.Range, "-90.0,90.0")]
    public float TiltUpperLimit { get; set; } = 60.0f;

    [Export(PropertyHint.Range, "1.0,1000.0")]
    public float HorizontalRotationSensitivity { get; set; } = 10.0f;

    [Export(PropertyHint.Range, "-90.0,90.0")]
    public float HorizontalRotationAngle { get; set; } = -45.0f;

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
        // Populate fields with new objects.
        cameraMarker = new Marker3D();
        springArm = new SpringArm3D();
        camera = new Camera3D();
        rotationPivot = new Node3D();
        offsetPivot = new Node3D();

        // Build tree:
        // Marker goes inside the SpringArm
        springArm.AddChild(cameraMarker);

        // Add all to RotationPivot
        rotationPivot.AddChild(offsetPivot);
        rotationPivot.AddChild(springArm);

        // Camera3D and RotationPivot go inside Camera.
        AddChild(camera);
        AddChild(rotationPivot);

        // Check if DebugGizmo is enabled.
        if (EnableGizmo) gizmo = GetNode<MeshInstance3D>("DebugGizmo");
    }

    /// <summary>
    /// If <see cref="EnableGizmo"/> is true, tweens the gizmo to the Camera's rotation.
    /// </summary>
    private void TweenGizmo()
    {
        if (EnableGizmo && gizmo != null)
        {
            var cameraRotation = camera.Rotation;
            gizmo.Rotation = cameraRotation;
        }
    }

    /// <summary>
    /// Moves the camera by the values of its parent and the offset in <see cref="PivotOffset"/>.
    /// </summary>
    private void SetOffsetPosition()
    {
        // Get the position of the player of which this camera is attached to.
        var parent = offsetPivot.GetParent() as Node3D;

        // Set new position. If there's a PivotOffset value, it's traslated by those values.
        offsetPivot.GlobalPosition = parent.ToGlobal(new Vector3(PivotOffset.X, PivotOffset.Y, 0.0f));
    }

    /// <summary>
    /// Sets the angle and position of the RotationPivot.
    /// </summary>
    private void SetRotationPivot()
    {
        var rotationPivotPosition = rotationPivot.GlobalPosition;

        rotationPivot.GlobalRotation = GlobalPosition;
        rotationPivotPosition.X = InitialDiveAngle;
    }

    private void ProcessHorizontalRotationInput()
    {
        if (InputMap.HasAction("tp_camera_right") && InputMap.HasAction("tp_camera_left"))
        {
            float cameraHorizontalVarition = Input.GetActionStrength("tp_camera_right") - Input.GetActionStrength("tp_camera_left");
            cameraHorizontalVarition = cameraHorizontalVarition * (float)GetProcessDeltaTime() * 30 * HorizontalRotationSensitivity;
            HorizontalRotationAngle += cameraHorizontalVarition;
        }
    }

    private void ProcessTiltInput()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void TweenCameraToMarker()
    {
        var cameraPosition = camera.GlobalPosition;
        var markerPosition = cameraMarker.GlobalPosition;
        cameraPosition.X = Mathf.Lerp(cameraPosition.X, markerPosition.X, 10);
        cameraPosition.Y = Mathf.Lerp(cameraPosition.Y, markerPosition.Y, 10);
        cameraPosition.Z = Mathf.Lerp(cameraPosition.Z, markerPosition.Z, 10);
    }

    [Export]
    float Distance
    {
        get => springArm.SpringLength;
        set
        {
            Variant v = value;
            SetWhenReady(springArm, SpringArm3D.PropertyName.SpringLength, v);
        }
    }

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

    async Task<Variant> GetWhenReady(Node n, StringName property)
    {
        if (!n.IsNodeReady())
        {
            "Node is not ready. Awaiting...".ToConsole();
            await ToSignal(this, SignalName.Ready);
            "Node is ready.".ToConsole();
            $"\t{property} get.".ToConsole();
            return n.Get(property);

        }
        else
        {
            $"Node is ready.".ToConsole();
            $"\t{property} get.".ToConsole();
            return n.Get(property);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        int v = 0;
        TweenGizmo();
        TweenCameraToMarker();
        SetOffsetPosition();
        SetRotationPivot();

        //DbgStr_ConsoleDebug();
    }

    private void DbgStr_ConsoleDebug()
    {
        var table = new ConsoleTable("Name", "X", "Y", "Z");
        table
            .AddRow("Camera Global Position", GlobalPosition.X, GlobalPosition.Y, GlobalPosition.Z)
            .AddRow("Offset Pivot Position", offsetPivot.GlobalPosition.X, offsetPivot.GlobalPosition.Y, offsetPivot.GlobalPosition.Z)
            .AddRow("SpringArm Distance", Distance, 0, 0)
            .AddRow("Rotation Pivot Position", rotationPivot.GlobalPosition.X, rotationPivot.GlobalPosition.Y, rotationPivot.GlobalPosition.Z)
            .AddRow("Rotation Pivot Angle", rotationPivot.GlobalRotationDegrees.X, rotationPivot.GlobalRotationDegrees.Y, rotationPivot.GlobalRotationDegrees.Z);
        table.ToString().ToConsole();
    }

    public override void _Ready()
    {
        CallDeferred(MethodName.CreateTree);
    }
}

// Debugging
public partial class AngelCamera
{
    private static SignalManager SignalManager { get => SignalManager.Get(); }

    private void AngelCamera_ChildExitingTree(Node node) => SignalManager.DbgMsg_ChildExitingTree(this, node);

    private void AngelCamera_ChildEnteredTree(Node node) => SignalManager.DbgMsg_ChildEnteringTree(this, node);

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