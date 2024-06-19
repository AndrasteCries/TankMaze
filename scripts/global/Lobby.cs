using Godot;
using System.Collections.Generic;
using System.Linq;
using mazetank.scripts.player;

namespace mazetank.scripts.global;

public partial class Lobby : Node
{
	public List<Player> Players { get; set; } = new List<Player>();
	
	public override void _Ready()
	{
		Player player = new Player();
		AddPlayer(player);
		
	}

	public void AddPlayer(Player player)
	{
		Players.Add(player);
	}

	public void DeletePlayer(ulong id)
	{
		var player = Players.FirstOrDefault(p => p.Id == id);
		if (player != null)
		{
			Players.Remove(player);
		}
	}

	public int GetPlayerCount() => Players.Count;

}
