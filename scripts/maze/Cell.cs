using Godot;
using System;

namespace mazetank.scripts.maze;
public partial class Cell : StaticBody2D
{
	public Vector2I Pos { get; set; } = new Vector2I(0, 0);
	public bool BottomWall { get; set; } = true;
	public bool LeftWall { get; set; } = true;
	public bool Visited { get; set; } = false;
	private Marker2D _spawnPoint;
	
	public override void _Ready()
	{
		_spawnPoint = GetNode<Marker2D>("SpawnPoint");
		var floorColor = GD.Randi() % 3;
		Color color;

		switch (floorColor)
		{
			case 0:
				color = new Color(0.318f, 0.318f, 0.318f);
				break;
			case 1:
				color = new Color(0.447f, 0.447f, 0.447f);
				break;
			case 2:
				color = new Color(0.408f, 0.408f, 0.408f);
				break;
			default:
				color = new Color(0.318f, 0.318f, 0.318f);
				break;
		}

		GetNode<ColorRect>("Floor").Modulate = color;
	}

	public void Init(int x, int y)
	{
		Pos = new Vector2I(x, y);
		Position = Pos * 100;
	}

	public void DestroyLeftWall()
	{
		GetNode<CollisionShape2D>("LeftWallShape").QueueFree();
		LeftWall = false;
	}

	public void DestroyBottomWall()
	{
		GetNode<CollisionShape2D>("BottomWallShape").QueueFree();
		BottomWall = false;
	}

	public void DestroyColumnWall()
	{
		GetNode<CollisionShape2D>("CollumnShape").QueueFree();
	}

	public void DestroyFloor()
	{
		GetNode<ColorRect>("Floor").QueueFree();
	}
	
	public Vector2 GetSpawnpointPosition() => _spawnPoint.GlobalPosition;
	
}
