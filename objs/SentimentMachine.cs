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
	public ProgressBar Bar;
	[Export]
	public float TimeMax = 75f;
	public float timeRemaining;
	[Export]
	public bool Playing = true;

	[Export]
	public Curve MissileChanceCurve;

	[Export]
	public PackedScene MailScn;
	[Export]
	public PackedScene MissileScn;

	[Export]
	public Control GameOverPanel;

	[Export]
	public Label ScoreLabel;

	RandomNumberGenerator R = new RandomNumberGenerator();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Inst = this;
		timeRemaining = TimeMax;
    }

	bool pressed;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
		if (!Playing) {
			if (timeRemaining == 0 && pressed && !Input.IsMouseButtonPressed(MouseButton.Left)) {
				GetTree().ReloadCurrentScene();
			}
			pressed = Input.IsMouseButtonPressed(MouseButton.Left);
			return;
		}

		timeRemaining -= (float)delta;
		if (timeRemaining <= 0) {
			timeRemaining = 0;
			Playing = false;
			Bar.Value = 0;

			var mails = 0;
			var missiles = 0;
			foreach (var obj in GravityObj.All) {
				if (!obj.IsMissile) {
					mails++;
				} else {
					missiles++;
				}
			}
			var total = mails * 1.5f + missiles * 1200;
			ScoreLabel.Text = ScoreLabel.Text.Replace("XXX", mails.ToString()).Replace("ZZZ", missiles.ToString()) + total.ToString("N2");
			GameOverPanel.Visible = true;
		} else {
			Bar.Value = timeRemaining / TimeMax;
		}

		Meter.Amount = Mathf.Clamp(Meter.Amount, 0, 1);

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
