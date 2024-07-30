using System.Collections.Generic;
using Godot;
using mazetank.scripts.player.bullets;

namespace mazetank.scripts.player.towers;

public partial class DefaultTower : Node2D, ITower
{
	[Export] public PackedScene Bullet { get; set; }
	public int BulletsCount { get; set; } = 4;
	public Node World { get; set; }
	public Marker2D Muzzle1 { get; set; }
	private string _playerNickname;

	private List<Vector2> rayPoints = new List<Vector2>();
	
	public override void _Ready()
	{
		_playerNickname = GetParent().Name;
		World = GetNode("/root/Game");
		Muzzle1 = GetNode<Marker2D>("Sprite2D/Muzzle");
	}

	public async void Shoot()
	{
		if (BulletsCount > 0)
		{
			var b = (Bullet)Bullet.Instantiate();
			b.SetNickname(_playerNickname);
			b.Name = _playerNickname + " bullet";
			var timerToDeath = new Timer
			{
				WaitTime = 2
			};
			AddChild(timerToDeath);

			b.Rotation = GetParent<CharacterBody2D>().Rotation;
			b.Position = Muzzle1.GlobalPosition;
			
			World.AddChild(b);

			timerToDeath.Start();

			BulletsCount -= 1;
			await ToSignal(timerToDeath, "timeout");
			timerToDeath.QueueFree();
			BulletsCount += 1;
		}
	}
}
