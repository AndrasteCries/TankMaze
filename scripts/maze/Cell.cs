using Godot;
using System;
using mazetank.scripts.maze.buff;
using mazetank.scripts.player;

namespace mazetank.scripts.maze;
public partial class Cell : StaticBody2D
{
	[Export] public PackedScene BuffScene { get; set; }
	[Export] public PackedScene PlayerScene { get; set; }
	public Vector2I Pos { get; set; } = new Vector2I(0, 0);
	public bool BottomWall { get; set; } = true;
	public bool LeftWall { get; set; } = true;
	public bool Visited { get; set; } = false;
	public bool HadPlayer { get; set; } = false;
	public bool HadBuff { get; set; } = false;
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

	public Tank SpawnNewPlayer(RandomNumberGenerator rng, long id, string nickname, Color color)
	{
		if (!HadBuff)
		{
			Tank tank = (Tank)PlayerScene.Instantiate();
			tank.GlobalPosition = GetSpawnpointPosition();
			tank.Rotation = rng.RandfRange(0, 2 * Mathf.Pi);
			tank.Name = nickname;
			tank.Modulate = color;
			tank.SetMultiplayerAuthority((int)id);
			GetParent().AddChild(tank);
			return tank;
		}
		return null;
	}
	
	public Tank SpawnPlayer(RandomNumberGenerator rng, Tank tank)
	{
		if (!HadPlayer)
		{
			tank.GlobalPosition = GetSpawnpointPosition();
			tank.Rotation = rng.RandfRange(0, 2 * Mathf.Pi);
			tank.TowerType = "Default";
			tank.ChangeTower();
			return tank;
		}
		return null;
	}
	
	public Buff SpawnBuff(RandomNumberGenerator rng, int buffType)
	{
		if (!HadBuff)
		{
			Buff buff = (Buff)BuffScene.Instantiate();
			GetParent().AddChild(buff);
			buff.GlobalPosition = GetSpawnpointPosition();
			buff.Rotation = rng.RandfRange(0, 2 * Mathf.Pi);
			buff.SetBuffType(0); //random range
			return buff;
		}
		return null;
	}
	
	private void OnCell(Area2D area)
	{
		var areaParent = area.GetParent();
		if (areaParent is Buff buff)
		{
			HadBuff = true;
		} else if (areaParent is Tank tank)
		{
			HadPlayer = true;
		}
	}


	private void OutCell(Area2D area)
	{
		var areaParent = area.GetParent();
		if (areaParent is Buff buff)
		{
			HadBuff = false;
		} else if (areaParent is Tank tank)
		{
			HadPlayer = false;
		}
	}
}


