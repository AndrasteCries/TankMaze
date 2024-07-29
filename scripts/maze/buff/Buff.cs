using Godot;
using System;

namespace mazetank.scripts.maze.buff;

public partial class Buff : Node2D
{
	public delegate void BuffWasTakenEventHandler(Buff buff);
	public static event BuffWasTakenEventHandler BuffWasTaken;
	
	[Export] private Texture2D _shotgunBuffTexture;
	[Export] private Texture2D _bigShotBuffTexture;
	[Export] private Texture2D _laserBuffTexture;
	[Export] private Texture2D _minigunBuffTexture;
	[Export] private Texture2D _rocketBuffTexture;
	
	private Sprite2D _buffSprite;
	public int BuffType = 0;
	public string BuffName = "noBuff";
	
	public override void _Ready()
	{
		_buffSprite = GetNode<Sprite2D>("BuffSprite");
	}
	
	//public override void _Process(double delta)
	//{
		//Rotation += 0.01f;
	//}
	
	public void SetBuffType(int type)
	{
		switch (type)
		{
			case 0:
				_buffSprite.Texture = _shotgunBuffTexture;
				BuffName = "Shotgun";
				break;
			case 1: 
				_buffSprite.Texture = _bigShotBuffTexture;
				BuffName = "BigShot";
				break;
			case 2:
				_buffSprite.Texture = _laserBuffTexture;
				BuffName = "Laser";
				break;
			case 3: 
				_buffSprite.Texture = _minigunBuffTexture;
				BuffName = "Minigun";
				break;
			case 4:
				_buffSprite.Texture = _rocketBuffTexture;
				BuffName = "Rocket";
				break;
			default:
				_buffSprite.Texture = _shotgunBuffTexture;
				BuffName = "Shotgun";
				break;
		}	
		BuffType = type;
		Name = "Buff" + BuffName;
	}
	
	public override void _ExitTree()
	{
		BuffWasTaken?.Invoke(this);
	}
}
