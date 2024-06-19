using Godot;
using System;

namespace mazetank.scripts.player;
public partial class Tank : CharacterBody2D
{
	[Export] public int Speed = 300;
	[Export] public int RotationSpeed = 5;

	[Export] public PackedScene DefaultTower;
	[Export] public PackedScene BigShotTower;
	[Export] public PackedScene LaserTower;
	[Export] public PackedScene MinigunTower;
	[Export] public PackedScene RocketTower;
	[Export] public PackedScene ShotgunTower;

	private Node World;
	private float SteerAngle;
	private int RotationDirection = 0;
	private bool Alive = true;
	private Timer RespawnTimer = new Timer();
	private string TowerType = "Normal";

	public override void _Ready()
	{
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
		Rotation += RotationSpeed * turn * delta;

		Velocity = Vector2.Zero;
		if (Input.IsActionPressed("W"))
		{
			Velocity = new Vector2(0, -Speed).Rotated(Rotation);
		}
		if (Input.IsActionPressed("S"))
		{
			Velocity = new Vector2(0, Speed).Rotated(Rotation);
		}

		if (Input.IsActionJustReleased("LMB"))
		{
			GetNode("Tower").Call("shoot_rpc");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId() && Alive)
		{
			MoveAndSlide();
			GetInput((float)delta);
		}
	}

	[Rpc(CallLocal = true)]
	public void ChangeTower()
	{
		Node newTower = null;
		GetNode("Tower").QueueFree();
		RemoveChild(GetNode("Tower"));

		switch (TowerType)
		{
			case "Default":
				newTower = DefaultTower.Instantiate();
				break;
			case "Big_shot":
				newTower = BigShotTower.Instantiate();
				break;
			case "Laser":
				newTower = LaserTower.Instantiate();
				break;
			case "Minigun":
				newTower = MinigunTower.Instantiate();
				break;
			case "Rocket":
				newTower = RocketTower.Instantiate();
				break;
			case "Shotgun":
				newTower = ShotgunTower.Instantiate();
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
