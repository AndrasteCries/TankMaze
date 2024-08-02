using Godot;
using mazetank.scripts.maze.buff;
using mazetank.scripts.player.bullets;
using mazetank.scripts.player.towers;

namespace mazetank.scripts.player;

public partial class Tank : CharacterBody2D
{
	public delegate void PlayerWasKilledEventHandler(string killerNickname, string victimNickname);
	public static event PlayerWasKilledEventHandler PlayerWasKilled;

	[Export] private Camera2D _camera2D;
	[Export] private int _speed = 300;
	[Export] private int _rotationSpeed = 5;
	[Export] private PlayerInputMultiplayer _playerInput;
	
	//todo resource
	[Export] private PackedScene _defaultTower;
	[Export] private PackedScene _bigShotTower;
	[Export] private PackedScene _laserTower;
	[Export] private PackedScene _minigunTower;
	[Export] private PackedScene _rocketTower;
	[Export] private PackedScene _shotgunTower;
	
	private bool _alive = true;
	
	public string TowerType { get; set; } = "Default";

	public override void _EnterTree()
	{
		if (Multiplayer.GetUniqueId() == GetMultiplayerAuthority())
		{
			_camera2D.MakeCurrent();
		}
		else
		{
			_camera2D.Enabled = false;
		}
		
		_playerInput.SetMultiplayerAuthority(Multiplayer.MultiplayerPeer.GetUniqueId());
	}

	public override void _Ready()
	{ 
		
		var tower = _defaultTower.Instantiate();
		tower.Name = "Tower";
		AddChild(tower);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Multiplayer.GetUniqueId() == GetMultiplayerAuthority())
		{
			Rotation += _rotationSpeed * _playerInput.Turn * (float)delta;

			Velocity = Vector2.Zero;
		
			if (_playerInput.Direction == 1)
			{
				Velocity = new Vector2(0, -_speed).Rotated(Rotation);
			}
			if (_playerInput.Direction == -1)
			{
				Velocity = new Vector2(0, _speed).Rotated(Rotation);
			}

			if (_playerInput.Shoot)
			{
				var tower = GetNode<ITower>("Tower");
				Rpc(nameof(ShootRpc));
				_playerInput.Shoot = false;
			}
			
			MoveAndSlide();
		}
	} 
		
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void ShootRpc()
	{
		var tower = GetNode<ITower>("Tower");
		tower.Shoot();
	}
	
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
			case "BigShot":
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

	private void _on_tank_area_area_entered(Area2D area)
	{
		var areaParent = area.GetParent();
		if (areaParent is Buff buff)
		{
			TowerType = buff.BuffName;
			ChangeTower();
			buff.QueueFree();
		}
		else if (areaParent is Bullet bullet)
		{
			PlayerWasKilled?.Invoke(bullet.GetNickname(), Name);
			areaParent.QueueFree();
			SetState(false);
		}
	}

	private void OnRespawnTimeout()
	{
		ChangeTower();
		SetState(true);
	}

	public void SetState(bool status)
	{
		Visible = status;
		SetProcess(status);
		SetPhysicsProcess(status);
	}
}



