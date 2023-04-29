using Godot;
using System;
using System.Collections.Generic;

public partial class GravityObj : Node
{
	public static readonly List<GravityObj> All = new List<GravityObj>();

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
	}
}
