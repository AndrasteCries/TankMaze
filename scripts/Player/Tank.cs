using Godot;
using System;
using mazetank.scripts.player.towers;

namespace mazetank.scripts.player;
public partial class Tank : CharacterBody2D
{
	[Export] private int _speed = 300;
	[Export] private int _rotationSpeed = 5;

	[Export] private PackedScene _defaultTower;
	[Export] private PackedScene _bigShotTower;
	[Export] private PackedScene _laserTower;
	[Export] private PackedScene _minigunTower;
	[Export] private PackedScene _rocketTower;
	[Export] private PackedScene _shotgunTower;
	
	private bool _alive = true;
	private Timer _respawnTimer = new Timer();
	public string TowerType { get; set; } = "Default";

	public override void _Ready()
	{
		var tower = _defaultTower.Instantiate();
		tower.Name = "Tower";
		AddChild(tower);
		ChangeTower();
		// AddChild(RespawnTimer);
		// GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
		// int colorId = (int)Lobby.Players[int.Parse(Name)]["Color"];
		// Modulate = Lobby.Colors[colorId];
		//
		// RespawnTimer.Timeout += OnRespawnTimeout;
	}

	private void GetInput(float delta)
	{
		int turn = 0;
		if (Input.IsActionPressed("D"))
		{
			turn += 1;
		}
		if (Input.IsActionPressed("A"))
		{
			turn -= 1;
		}
		Rotation += _rotationSpeed * turn * delta;

		Velocity = Vector2.Zero;
		if (Input.IsActionPressed("W"))
		{
			Velocity = new Vector2(0, -_speed).Rotated(Rotation);
		}
		if (Input.IsActionPressed("S"))
		{
			Velocity = new Vector2(0, _speed).Rotated(Rotation);
		}

		if (Input.IsActionJustReleased("LMB"))
		{
			GetNode<ITower>("Tower").Shoot();
			//GetNode("Tower").Call("shoot_rpc");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId() && _alive)
		{
			MoveAndSlide();
			GetInput((float)delta);
		}
	}	

	//[Rpc(CallLocal = true)]
	public void ChangeTower()
	{
		Node newTower = null;
		GetNode("Tower").QueueFree();
		RemoveChild(GetNode("Tower"));

		switch (TowerType)
		{
			case "Default":
				newTower = _defaultTower.Instantiate();
				break;
			case "Big_shot":
				newTower = _bigShotTower.Instantiate();
				break;
			case "Laser":
				newTower = _laserTower.Instantiate();
				break;
			case "Minigun":
				newTower = _minigunTower.Instantiate();
				break;
			case "Rocket":
				newTower = _rocketTower.Instantiate();
				break;
			case "Shotgun":
				newTower = _shotgunTower.Instantiate();
				break;
			default:
				newTower = _defaultTower.Instantiate();
				break;
		}

		AddChild(newTower);
		newTower.Name = "Tower";
	}

	// private async void OnArea2DEntered(Area2D area)
	// {
	//     if (area.Name == "BulletArea" && Alive)
	//     {
	//         var killer = area.GetParent().Name;
	//         area.GetParent().QueueFree();
	//         Hide();
	//         Alive = false;
	//         RespawnTimer.WaitTime = 1.5f;
	//         RespawnTimer.Start();
	//
	//         await ToSignal(RespawnTimer, "timeout");
	//         Alive = true;
	//         EventBus.PlayerDead.Emit(Name, killer);
	//     }
	//     else if (area.Name == "BuffArea" && Alive)
	//     {
	//         TowerType = area.GetParent().Get<string>("type");
	//         ChangeTower();
	//         area.GetParent().QueueFree();
	//     }
	// }

	private void OnRespawnTimeout()
	{
		// Handle respawn logic if needed
	}
}
