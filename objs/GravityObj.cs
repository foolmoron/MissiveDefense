using Godot;
using System;
using System.Collections.Generic;

public partial class GravityObj : Node2D
{
    public static readonly List<GravityObj> All = new List<GravityObj>();

    [Export]
    public RigidBody2D MainRigidBody;
    [Export]
    public RigidBody2D HandleRigidBody;
    [Export]
    public Joint2D Joint;
    [Export]
    public PackedScene ExplosionScn;

    [Export]
    public CanvasItem[] ItemsToColor;

    Vector2 originalHandleOffset;

    bool held;
    bool released;

    [Export]
    public Color TargetColor;


    static RandomNumberGenerator R = new RandomNumberGenerator();

    public override void _Ready()
    {
        originalHandleOffset = (MainRigidBody.GlobalPosition - HandleRigidBody.GlobalPosition);
        var r = R.RandiRange(-1, Colors.Inst.AllColors.Length - 1);
        TargetColor =  r == -1 ? Colors.Inst.BrownColor : Colors.Inst.AllColors[r];
    }

    public override void _EnterTree()
    {
        All.Add(this);
        MainRigidBody.RemoveChild(Joint);
    }

    public override void _ExitTree()
    {
        All.Remove(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        foreach (var item in ItemsToColor)
        {
            item.Modulate = TargetColor;
        }

        if (held && !Input.IsMouseButtonPressed(MouseButton.Left))
        {
            held = false;
            released = true;
            MainRigidBody.RemoveChild(Joint);
        }

        if (held)
        {
            HandleRigidBody.GlobalPosition =
                GetViewport().GetMousePosition();
        }

        if (released && MainRigidBody.GetContactCount() > 0)
        {
            var scn = ExplosionScn.Instantiate<Node2D>();
            GetParent().AddChild(scn);
            scn.GlobalPosition = MainRigidBody.GlobalPosition;

            QueueFree();
        }
    }

    public void InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (!released && @event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            held = true;
            HandleRigidBody.GlobalPosition = MainRigidBody.GlobalPosition + originalHandleOffset;
            MainRigidBody.AddChild(Joint);
        }
    }
}
