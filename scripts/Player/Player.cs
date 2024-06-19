using Godot;
using System;
using Godot.Collections;

namespace mazetank.scripts.player;

public partial class Player : Node2D
{
	public ulong Id { get; set; }
	public string Nickname { get; set; }
	public int Color { get; set; }
	public int Score { get; set; }
	
	private Dictionary<int, Color> colors = new Dictionary<int, Color>()
	{
		{ 0, new Color(0, 1, 0) }, // Green
		{ 1, new Color(0.969f, 0, 0) }, // Red
		{ 2, new Color(0.584f, 0, 1) }, // Purple
		{ 3, new Color(0.172f, 0.404f, 1) } // Blue
	};
	public override void _Ready()
	{
	}
	
	public override void _Process(double delta)
	{
	}
}


