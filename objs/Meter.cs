using Godot;
using System;

public partial class Meter : Node2D
{
	[Export]
	public Node2D MeterBar;
	[Export(PropertyHint.Range, "0, 1, 0.01")]
	public float Amount;
	[Export(PropertyHint.Range, "-180, 180, 1")]
	public float MinRot = -90f;
	[Export(PropertyHint.Range, "-180, 180, 1")]
	public float MaxRot = 90f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MeterBar.RotationDegrees = Mathf.Lerp(MinRot, MaxRot, Amount);
	}
}
