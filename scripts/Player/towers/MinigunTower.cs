using Godot;
using System;
namespace mazetank.scripts.player.towers;
public partial class MinigunTower : Node2D, ITower
{
	[Export] public PackedScene Bullet { get; set; }
	public int BulletsCount { get; set; } = 4;
	public Node World { get; set; }
	public Marker2D Muzzle1 { get; set; }
	
	public override void _Ready()
	{
		World = GetNode("/root/Game");
		Muzzle1 = GetNode<Marker2D>("Sprite2D/Muzzle");
	}

	public async void Shoot()
	{
		if (BulletsCount > 0)
		{
			var b = (Node2D)Bullet.Instantiate();
			b.Name = $"{Name} bullet №{BulletsCount}";

			var timerToDeath = new Timer
			{
				Name = b.Name,
				WaitTime = 2
			};

			AddChild(timerToDeath);
			World.AddChild(b);

			timerToDeath.Start();
			b.Call("start", Muzzle1.GlobalPosition,  GetParent<CharacterBody2D>().Rotation);

			BulletsCount -= 1;
			await ToSignal(timerToDeath, "timeout");
			timerToDeath.QueueFree();
			BulletsCount += 1;
			if (BulletsCount == 0)
			{
				var parent = (Tank)GetParent();
				parent.TowerType = "Default";
				parent.ChangeTower();
			}
		}
	}
}