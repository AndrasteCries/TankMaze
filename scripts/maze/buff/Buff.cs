using Godot;
using System;

namespace mazetank.scripts.player.buff;

public partial class Buff : Node2D
{
	[Export] private Texture2D _shotgunBuffTexture;
	[Export] private Texture2D _bigShotBuffTexture;
	[Export] private Texture2D _laserBuffTexture;
	[Export] private Texture2D _minigunBuffTexture;
	[Export] private Texture2D _rocketBuffTexture;
	
	private Sprite2D _buffSprite;
	public int BuffType = 0;
	
	public override void _Ready()
	{
		_buffSprite = GetNode<Sprite2D>("BuffSprite");
	}

	public void SetBuffType(int type)
	{
		switch (type)
		{
			case 0:
				_buffSprite.Texture = _shotgunBuffTexture;
				break;
			case 1: 
				_buffSprite.Texture = _bigShotBuffTexture;
				break;
			case 2:
				_buffSprite.Texture = _laserBuffTexture;
				break;
			case 3: 
				_buffSprite.Texture = _minigunBuffTexture;
				break;
			case 4:
				_buffSprite.Texture = _rocketBuffTexture;
				break;
			default:
				_buffSprite.Texture = _shotgunBuffTexture;
				break;
		}	
		BuffType = type;
		Name = "Buff" + BuffType;
	}
}
