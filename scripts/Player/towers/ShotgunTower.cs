using Godot;

namespace mazetank.scripts.player.towers;
public partial class ShotgunTower : Node2D, ITower
{
	[Export] public PackedScene Bullet { get; set; }
	public int BulletsCount { get; set; } = 6;
	public Node World { get; set; }
	public Marker2D Muzzle1 { get; set; }
	public Marker2D Muzzle2 { get; set; }
	private int _flag = 0;
	
	
	public override void _Ready()
	{
		World = GetNode("/root/Game");
		Muzzle1 = GetNode<Marker2D>("Sprite2D/Muzzle1");
		Muzzle2 = GetNode<Marker2D>("Sprite2D/Muzzle2");
	}

	// [Rpc("any_peer", CallLocal = true)]
	public void Shoot()
	{
		if (BulletsCount > 0)
		{
			var b = (Node2D)Bullet.Instantiate();
			b.Name = Name + "ShotgunBullet bullet №" + BulletsCount.ToString();

			World.AddChild(b);

			if (_flag == 0)
			{
				b.Call("Start", Muzzle1.GlobalPosition, GetParent<CharacterBody2D>().Rotation);
				_flag = 1;
			}
			else
			{
				b.Call("Start", Muzzle2.GlobalPosition, GetParent<CharacterBody2D>().Rotation);
				_flag = 0;
			}

			BulletsCount -= 1;
			if (BulletsCount == 0)
			{
				var parent = (Tank)GetParent();
				parent.TowerType = "Default";
				parent.ChangeTower();
			}
		}
	}
}
