using Godot;
using System;

public partial class ExplosionParticles : CpuParticles2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			GD.Print("starts");
		Emitting = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Emitting) {
			GD.Print("die");
			QueueFree();
		}
	}
}
