using Godot;
using System;

public partial class RotateParent : Node
{
	[Export(PropertyHint.Range, "-720, 720, 1, or_greater, or_lesser")]
	public float SpeedRad = 360f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetParent<Node2D>().Rotate((float)(delta * Mathf.DegToRad(SpeedRad)));
	}
}
