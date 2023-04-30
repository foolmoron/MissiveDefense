using Godot;
using System;
using System.Collections.Generic;

public partial class GravityObj : Node
{
    public static readonly List<GravityObj> All = new List<GravityObj>();

    [Export]
    public RigidBody2D MainRigidBody;
    [Export]
    public RigidBody2D HandleRigidBody;
    [Export]
    public Joint2D Joint;

	Vector2 originalHandlePos;

    bool held;

    public override void _Ready()
    {
        originalHandlePos = HandleRigidBody.Position;
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
        if (held && !Input.IsMouseButtonPressed(MouseButton.Left))
        {
            held = false;
            MainRigidBody.RemoveChild(Joint);
            HandleRigidBody.Position = originalHandlePos;
        }

        if (held)
        {
            HandleRigidBody.GlobalPosition =
				GetViewport().GetMousePosition();
        }
    }

    public void InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            held = true;
            MainRigidBody.AddChild(Joint);
        }
    }
}
