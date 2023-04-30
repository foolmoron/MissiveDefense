using Godot;
using System;

public partial class SentimentMachine : Node2D
{
	public static SentimentMachine Inst;

    [Export]
    public Node2D SpawnPoint;
    [Export]
	public float SpawnInterval = 3f;
    [Export]
	public float SpawnForce = 1000f;
	float spawnTime;

    [Export]
	public Meter Meter;

	[Export]
	public Curve MissileChanceCurve;

	[Export]
	public PackedScene MailScn;
	[Export]
	public PackedScene MissileScn;

	RandomNumberGenerator R = new RandomNumberGenerator();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Inst = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
		spawnTime -= (float)delta;
		if (spawnTime <= 0) {
			spawnTime = SpawnInterval;

			var chance = MissileChanceCurve.Sample(Meter.Amount);
			var scn = GD.RandRange(0f, 1f) < chance ? MissileScn : MailScn;
			var obj = scn.Instantiate<GravityObj>();
			GetParent().AddChild(obj);
			obj.GlobalPosition = SpawnPoint.GlobalPosition;
			obj.MainRigidBody.ApplyImpulse(new Vector2(1f, -0.3f) * R.RandfRange(0.8f, 1.2f) * SpawnForce);
		}
    }
}
