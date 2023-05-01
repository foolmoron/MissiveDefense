using Godot;
using System;

public partial class Colors : Node
{
	public static Colors Inst;

	[Export]
	public Color[] AllColors;
	[Export]
	public Color BrownColor;
	public const float BROWN_CHANCE = 0.9831545f; // https://watson.brown.edu/costsofwar/figures/2021/WarDeathToll

	public override void _Ready()
	{
		Inst = this;
	}
}
