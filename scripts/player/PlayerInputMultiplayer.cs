using Godot;

namespace mazetank.scripts.player;

public partial class PlayerInputMultiplayer : MultiplayerSynchronizer
{

	public bool Shoot = false;
	public int Direction = 0;
	public int Turn = 0;
	
	public override void _Ready()
	{
		if (Multiplayer.GetUniqueId() != GetMultiplayerAuthority())
		{
			SetProcessInput(false);
		}
	}

	public override void _Input(InputEvent @event)
	{
		Turn = 0;
		Direction = 0;
		
		if (Input.IsActionPressed("D"))
		{
			Turn = 1;
		}
		if (Input.IsActionPressed("A"))
		{
			Turn = -1;
		}
		
		if (Input.IsActionPressed("W"))
		{
			Direction = 1;
		}
		if (Input.IsActionPressed("S"))
		{
			Direction = -1;
		}

		if (Input.IsActionJustReleased("LMB"))
		{
			Shoot = true;
		}
	}
}
