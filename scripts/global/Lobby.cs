using Godot;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using mazetank.scripts.player;
using Color = Godot.Color;

namespace mazetank.scripts.global;

public partial class Lobby : Node
{
	private List<Player> Players { get; set; } = new List<Player>();
	
	public override void _Ready()
	{
	}

	public void AddPlayer(Player player)
	{
		Players.Add(player);
	}

	public void DeletePlayer(long id)
	{
		var player = Players.FirstOrDefault(p => p.Id == id);
		if (player != null)
		{
			Players.Remove(player);
		}
	}

	public int GetPlayerCount() => Players.Count;

	public List<Player> GetPlayersList() => Players;
	
	public Player GetPlayerById(long id) 
		=>  Players.FirstOrDefault(player => player.Id == id);
	
	public Player GetPlayerByNickName(string playerNickname) 
		=>  Players.FirstOrDefault(player => player.Nickname == playerNickname);
	
	public Player RemovePlayerById(long id)
	{
		var playerToRemove = Players.FirstOrDefault(player => player.Id == id);
		if (playerToRemove != null)
		{
			Players.Remove(playerToRemove);
		}
		return playerToRemove;
	}
	
	public void ShowPlayers()
	{
		foreach (var player in Players)
		{
			if(Multiplayer.IsServer()) GD.Print("HOST");
			GD.Print(player.Id + " " + player.Nickname);
		}
	}
}
