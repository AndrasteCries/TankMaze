using Godot;
using System;

namespace mazetank.scripts.global;

public partial class GameSettings : Node
{
	private Vector2I Size { get; set; } = new Vector2I(5, 5);
	private int Seed  { get; set; } = 1;
	private int PlayerCount  { get; set; } = 1;
	private int BuffsCount  { get; set; } = 5;
	private double RespawnTime { get; set; } = 1;
	private int RandomWallsDestroy { get; set; } = 5;
	public Vector2I GetSize() => Size;
	public int GetRandomWallsDestroy() => RandomWallsDestroy;
	public double GetRespawnTime() => RespawnTime;
	public int GetSeed() => Seed;
	public int GetBuffsCount() => BuffsCount;
	
	
	public override void _Ready()
	{
		
	}
}
