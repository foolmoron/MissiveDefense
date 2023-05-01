using Godot;
using System;

public partial class Start : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	bool pressed;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (pressed && !Input.IsMouseButtonPressed(MouseButton.Left)) {
			QueueFree();
			SentimentMachine.Inst.Playing = true;
		}
		pressed = Input.IsMouseButtonPressed(MouseButton.Left);
	}
}
