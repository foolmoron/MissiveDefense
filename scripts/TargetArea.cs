using Godot;
using System;

public partial class TargetArea : Node
{
    [Export]
	public int Max = 9;
    [Export]
	public float SpawnInterval = 2f;
	float spawnTime;

	[Export]
	public Rect2 SpawnRect;

	[Export]
	public PackedScene TargetScn;

	static RandomNumberGenerator R = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!SentimentMachine.Inst.Playing) {
			return;
		}
		spawnTime -= (float)delta;
		if (spawnTime <= 0) {
			if (Target.All.Count >= Max) {
				Target.All[0].QueueFree();
			}
			spawnTime = SpawnInterval;
			var t = TargetScn.Instantiate<Target>();
			GetParent().AddChild(t);
			t.GlobalPosition = new Vector2(R.RandfRange(SpawnRect.Position.X, SpawnRect.End.Y), R.RandfRange(SpawnRect.Position.X, SpawnRect.End.Y));
		}
	}
}
