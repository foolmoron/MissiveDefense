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
    public CanvasItem Item1;
    [Export]
    public CanvasItem Item2;
    [Export]
    public CanvasItem Item3;

    Vector2 originalHandleOffset;

    bool held;
    bool released;

    [Export]
    public Color TargetColor;
    [Export]
    public bool IsMissile;

    [Export]
    public float PlusAmount = 0.03f;
    [Export]
    public float MinusAmount = -0.01f;

    static RandomNumberGenerator R = new RandomNumberGenerator();

    public override void _Ready()
    {
        originalHandleOffset = (MainRigidBody.GlobalPosition - HandleRigidBody.GlobalPosition);
        if (IsMissile)
        {
            TargetColor = Colors.Inst.AllColors[R.RandiRange(0, Colors.Inst.AllColors.Length - 1)];
            if (R.Randf() < Colors.BROWN_CHANCE)
            {
                TargetColor = Colors.Inst.BrownColor;
            }
        }
        else
        {
            var r = R.RandiRange(-1, Colors.Inst.AllColors.Length - 1);
            TargetColor = r == -1 ? Colors.Inst.BrownColor : Colors.Inst.AllColors[r];
        }
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
        if (Item1 != null)
        {
            Item1.SelfModulate = TargetColor;
        }
        if (Item2 != null)
        {
            Item2.SelfModulate = TargetColor;
        }
        if (Item3 != null)
        {
            Item3.SelfModulate = TargetColor;
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
    }

    public override void _PhysicsProcess(double delta)
    {
        if (released && MainRigidBody.GetContactCount() > 0)
        {
            var scn = ExplosionScn.Instantiate<Node2D>();
            GetParent().AddChild(scn);
            scn.GlobalPosition = MainRigidBody.GlobalPosition;

            var bodies = MainRigidBody.GetCollidingBodies();
            foreach (var body in bodies)
            {
                if (body.GetParent() is Target t)
                {
                    var good = TargetColor == t.TargetColor;
                    if (good)
                    {
                        SentimentMachine.Inst.Meter.Amount += PlusAmount;
                    }
                    else
                    {
                        SentimentMachine.Inst.Meter.Amount += MinusAmount;
                    }
                    t.QueueFree();
                    break;
                }
            }

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
