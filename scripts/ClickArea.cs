using Godot;
using System;

public partial class ClickArea : Area2D
{

	[Export(PropertyHint.Range, "0, 20000, 1000,or_greater,or_lesser")]
	public float Force = 10000f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            HandleLeftClick(mouseEvent.Position);
        }
    }

    private void HandleLeftClick(Vector2 position)
    {
		GD.Print($"z {GravityObj.All.Count}");
        foreach (var go in GravityObj.All)
        {
            foreach (var obj in go.GetChildren())
            {
                if (obj is RigidBody2D rb)
                {
					GD.Print("ClickArea._InputEvent: Left Click: ApplyImpulse");
					var dist = rb.GlobalPosition.DistanceTo(position);
                    rb.ApplyImpulse(((rb.GlobalPosition - position).Normalized() + Vector2.Up * 0.5f).Normalized() * Force / Mathf.Pow(dist, 0.8f));
                }
            }
        }
    }
}
