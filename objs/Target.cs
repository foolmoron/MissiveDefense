using Godot;
using System;
using System.Collections.Generic;

public partial class Target : Node2D
{
    public static readonly List<Target> All = new List<Target>();

	[Export]
	public Color TargetColor;

    [Export]
	public CanvasItem Item1;
    [Export]
	public CanvasItem Item2;
    [Export]
	public CanvasItem Item3;

    static RandomNumberGenerator R = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var r = R.RandiRange(-1, Colors.Inst.AllColors.Length - 1);
        TargetColor =  r == -1 ? Colors.Inst.BrownColor : Colors.Inst.AllColors[r];
	}

    public override void _EnterTree()
    {
        All.Add(this);
    }

    public override void _ExitTree()
    {
        All.Remove(this);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Item1 != null) {
			Item1.SelfModulate = TargetColor;
		}
		if (Item2 != null) {
			Item2.SelfModulate = TargetColor;
		}
		if (Item3 != null) {
			Item3.SelfModulate = TargetColor;
		}
	}
}
